using UnityEngine;

public class FastDeleteObject : MonoBehaviour
{
    public float lifetime = 1f; // Default time before destruction

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}