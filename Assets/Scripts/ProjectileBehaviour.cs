using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Enemy" && other.tag != "Head" && other.tag != "Body")
        {
            // Debug.Log("Projectile striked with " + other.gameObject.name + " with tag: " + other.tag);
            if (other.tag == "Player")
            {
                // Debug.Log("Player shot");
                other.gameObject.GetComponent<PlayerHealth>().TakeDamage(10);
            }
            Invoke(nameof(DestroyObject), 0.1f);
        }
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
