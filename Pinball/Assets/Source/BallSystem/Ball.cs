using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [Space]
    [SerializeField] private LayerMask interactiveObjectMask;
    [SerializeField] private LayerMask disableZoneMask;

    private BallPool _pool;

    public event Action OnInteractiveObjectCollision;
    public event Action OnLeaveGameField;

    public Rigidbody Rb => rb;

    public void Construct(BallPool pool) 
    {
        _pool = pool;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((interactiveObjectMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            OnInteractiveObjectCollision?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((disableZoneMask.value & (1 << other.gameObject.layer)) != 0) 
        {
            OnLeaveGameField?.Invoke();
            _pool.ReturnBall(this);
        }
    }

    private void Destruct() 
    {
        _pool = null;

        OnInteractiveObjectCollision = null;
        OnLeaveGameField = null;
    }

    private void OnDisable()
    {
        Destruct();
    }
}
