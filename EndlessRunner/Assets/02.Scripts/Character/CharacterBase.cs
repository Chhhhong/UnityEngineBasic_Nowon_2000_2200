using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public float jumpForce;
    protected Rigidbody rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}