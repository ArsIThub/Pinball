using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField] private float bounceForce = 20f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Ball ball))
        {
            Vector3 direction = (ball.transform.position - transform.position).normalized;
            ball.Rb.AddForce(direction * bounceForce, ForceMode.Impulse);
        }
    }
}
