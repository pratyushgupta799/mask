using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private bool addBulletSpread = true;
    [SerializeField] private Vector3 bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private ParticleSystem shootingSystem;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private ParticleSystem impactParticleSystem;
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private TrailRenderer bulletTrailThick;
    [SerializeField] private float shootDelay = 0.1f;
    [SerializeField] private LayerMask mask;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private int shootDamage = 20;
    [SerializeField] private int runDamage = 10;
    [SerializeField] private PlayerController playerController;

    private float lastShootTime;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && (playerController.GetCurrentMask() != PlayerController.Mask.Heal))
        {
            // Debug.Log("Shoot");
            Shoot();
        }
    }

    private void Shoot()
    {
        if (Time.time < lastShootTime + shootDelay) return;
        lastShootTime = Time.time;

        shootingSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        shootingSystem.Play();

        Ray camRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        Vector3 targetPoint;
        TrailRenderer trailType;
        if (playerController.GetCurrentMask() == PlayerController.Mask.Shoot)
        {
            trailType = bulletTrailThick;
        }
        else
        {
            trailType = bulletTrail;
        }
        TrailRenderer trail;

        if (Physics.Raycast(camRay, out RaycastHit camHit, 1000f, mask))
        {
            trail = Instantiate(trailType, bulletSpawnPoint.position, Quaternion.identity);
            
            if (camHit.collider.gameObject.CompareTag("Head"))
            {
                Enemy enemy = camHit.collider.gameObject.GetComponentInParent<Enemy>();
                if (enemy != null)
                {
                    if (playerController.GetCurrentMask() == PlayerController.Mask.Shoot)
                    {
                        enemy.TakeDamage(shootDamage + 40);
                    }
                    else
                    {
                        enemy.TakeDamage(runDamage + 30);
                    }
                }
            }
            else if (camHit.collider.gameObject.CompareTag("Body"))
            {
                Enemy enemy = camHit.collider.gameObject.GetComponentInParent<Enemy>();
                if (enemy != null)
                {
                    if (playerController.GetCurrentMask() == PlayerController.Mask.Shoot)
                    {
                        enemy.TakeDamage(shootDamage);
                    }
                    else
                    {
                        enemy.TakeDamage(runDamage);
                    }
                }
            }
            
            StartCoroutine(SpawnTrail(trail, camHit));
        }
        else
        {
            targetPoint = camRay.origin + camRay.direction * 100f;
            trail = Instantiate(trailType, bulletSpawnPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, targetPoint));
        }

        lastShootTime = Time.time;
    }

    private Vector3 GetDirection(Vector3 direction)
    {
        if (addBulletSpread)
        {
            direction += new Vector3(
                Random.Range(-bulletSpreadVariance.x, bulletSpreadVariance.x),
                Random.Range(-bulletSpreadVariance.y, bulletSpreadVariance.y),
                Random.Range(-bulletSpreadVariance.z, bulletSpreadVariance.z)
            );

            direction.Normalize();
        }

        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1000f)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;
            
            yield return null;
        }
        trail.transform.position = hit.point;
        Instantiate(impactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
        
        Destroy(trail.gameObject, trail.time);
    }
    
    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 targetPoint)
    {
        float timeToTravel = Vector3.Distance(bulletSpawnPoint.position, targetPoint) / 500f;
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < timeToTravel)
        {
            trail.transform.position = Vector3.Lerp(startPosition, targetPoint, time);
            time += Time.deltaTime / trail.time;
            
            yield return null;
        }
        trail.transform.position = targetPoint;
        
        Destroy(trail.gameObject, trail.time);
    }
}
