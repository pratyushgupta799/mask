using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    [SerializeField] private float lifeTime = 3f;

    private int headDamage;
    private int bodyDamage;
    private int playerDamage;
    
    private Vector3 direction;

    private bool isPlayerBullet;

    public void SetPlayerDamage(int damage)
    {
        playerDamage = damage;
    }

    public void SetPlayerBullet(bool value)
    {
        isPlayerBullet = value;
        if (isPlayerBullet)
        {
            AudioManager.Instance.PlayPlayerMagic();
        }
        else
        {
            AudioManager.Instance.PlayEnemyMagic(transform.position);
        }
    }

    public void SetHeadDamage(int damage)
    {
        headDamage = damage;
    }

    public void SetBodyDamage(int damage)
    {
        bodyDamage = damage;
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    
    void Update()
    {
        // move forward
        transform.position += direction * speed * Time.deltaTime;

        // face camera (billboard)
        transform.forward = Camera.main.transform.forward;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile") return;
        if (!isPlayerBullet && other.GetComponentInParent<Enemy>() != null)
            return;

        if (isPlayerBullet)
        {
            if (other.tag == "Head")
            {
                other.gameObject.GetComponentInParent<Enemy>().TakeDamage(headDamage);
            }
            else if (other.tag == "Body")
            {
                other.gameObject.GetComponentInParent<Enemy>().TakeDamage(bodyDamage);
            }
        }
        else
        {
            if (other.tag == "Player")
            {
                other.gameObject.GetComponent<PlayerHealth>().TakeDamage(playerDamage);
            }
        }

        Destroy(gameObject);
    }
}
