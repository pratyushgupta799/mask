using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Enemy")
        {
            if (other.tag == "Player")
            {
                // Debug.Log("Player shot");
                other.gameObject.GetComponent<PlayerHealth>().TakeDamage(20);
            }
            Invoke(nameof(DestroyObject), 0.1f);
        }
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
