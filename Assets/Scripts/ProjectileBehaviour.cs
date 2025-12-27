using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Enemy")
        {
            Invoke(nameof(DestroyObject), 0.1f);
        }
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
