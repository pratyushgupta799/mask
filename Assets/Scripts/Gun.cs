using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private ParticleSystem shootingSystem;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject magicProjectile;
    [SerializeField] private ParticleSystem impactParticleSystem;
    [SerializeField] private ParticleSystem bloodParticleSystem;
    [SerializeField] private float shootDelay = 0.1f;
    [SerializeField] private LayerMask mask;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private int shootDamage = 20;
    [SerializeField] private int shootDamageHead = 60;
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
        if (lastShootTime + shootDelay > Time.time) return;
        
        var camRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        
        if (Physics.Raycast(camRay, out RaycastHit camHit, 1000f, mask))
        {
            Vector3 dir = (camHit.point - bulletSpawnPoint.position).normalized;
            var bullet = Instantiate(magicProjectile, bulletSpawnPoint.position, Quaternion.identity);
            bullet.gameObject.GetComponent<MagicProjectile>().SetDirection(dir);
            bullet.gameObject.GetComponent<MagicProjectile>().SetHeadDamage(shootDamageHead);
            bullet.gameObject.GetComponent<MagicProjectile>().SetBodyDamage(shootDamage);
            bullet.gameObject.GetComponent<MagicProjectile>().SetPlayerBullet(true);
        }
        else
        {
            var bullet = Instantiate(
                magicProjectile,
                bulletSpawnPoint.position,
                playerCamera.transform.rotation
            );
            
            bullet.gameObject.GetComponent<MagicProjectile>().SetDirection(playerCamera.transform.forward);
            bullet.gameObject.GetComponent<MagicProjectile>().SetHeadDamage(shootDamageHead);
            bullet.gameObject.GetComponent<MagicProjectile>().SetBodyDamage(shootDamage);
            bullet.gameObject.GetComponent<MagicProjectile>().SetPlayerBullet(true);
        }

        lastShootTime = Time.time;
    }
}
