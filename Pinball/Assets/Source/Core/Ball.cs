using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    [SerializeField] private LayerMask springMask;

    private Rigidbody _rb;

    public Rigidbody Rb => _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
}
