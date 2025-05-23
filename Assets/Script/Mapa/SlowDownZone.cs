using UnityEngine;

public class SlowZone : MonoBehaviour
{
    public float slowFactor = 0.5f; // 0.5 = reduz pela metade

    private void OnTriggerStay2D(Collider2D other)
    {
        var rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity *= slowFactor;
        }
    }
}
