using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    [SerializeField] private float lifeTime = 3f;

    private int headDamage;
    private int bodyDamage;
    private Vector3 direction;

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
        
        if (other.tag == "Head")
        {
            other.gameObject.GetComponentInParent<Enemy>().TakeDamage(headDamage);
        }
        else if (other.tag == "Body")
        {
            other.gameObject.GetComponentInParent<Enemy>().TakeDamage(bodyDamage);
        }
        Destroy(gameObject);
    }
}
