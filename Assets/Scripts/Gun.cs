using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private bool addBulletSpread = true;
    [SerializeField] private Vector3 bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private ParticleSystem shootingSystem;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private ParticleSystem impactParticleSystem;
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private float shootDelay = 0.1f;
    [SerializeField] private LayerMask mask;
    [SerializeField] private Camera playerCamera;

    private float lastShootTime;

    private void Update()
    {
        if (Input.GetMouseButton(0))
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

        if (Physics.Raycast(camRay, out RaycastHit camHit, 1000f, mask))
        {
            TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.position, Quaternion.identity);
            if (camHit.collider.gameObject.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = camHit.collider.gameObject.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(20);
                }
            }
            
            StartCoroutine(SpawnTrail(trail, camHit));
        }
        else
        {
            targetPoint = camRay.origin + camRay.direction * 100f;
            TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.position, Quaternion.identity);
            
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

        while (time < 1)
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
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 10)
        {
            trail.transform.position = Vector3.Lerp(startPosition, targetPoint, time);
            time += Time.deltaTime / trail.time;
            
            yield return null;
        }
        trail.transform.position = targetPoint;
        
        Destroy(trail.gameObject, trail.time);
    }
}
