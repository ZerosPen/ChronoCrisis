using UnityEngine;

public class DeleteObject : MonoBehaviour
{
    public float lifetime = 6f; // Default time before destruction

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
