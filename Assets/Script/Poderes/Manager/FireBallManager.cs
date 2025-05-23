using UnityEngine;

public class FireBallManager : MonoBehaviour
{
    [SerializeField] private GameObject fireBallPrefab;
    // public GameObject fireBallPrefab;
    public Transform firePoint;
    public float delay = 2f; // tempo entre tiros
    public int level = 1;

    private float timer;

    void OnEnable()
    {
        timer = delay; // espera o tempo cheio antes do primeiro disparo
    }


    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= delay)
        {
            LaunchFireBall();
            timer = 0f;
        }
    }

    void LaunchFireBall()
    {
        float direction = transform.localScale.x > 0 ? 1f : -1f;

        // Posição de spawn ajustada manualmente com base na direção
        Vector3 offset = new Vector3(0.5f * direction, 0f, 0f);
        Vector3 spawnPosition = transform.position + offset;

        GameObject fb = Instantiate(fireBallPrefab, spawnPosition, Quaternion.identity);

        // Visual flip
        fb.transform.localScale = new Vector3(direction, 1, 1);

        FireBall fireballScript = fb.GetComponent<FireBall>();
        if (fireballScript != null)
        {
            float speed = 4f + level * 1f;
            float damage = 10f + level * 5f;
            float lifetime = 3f + level * 0.5f;

            fireballScript.SetStats(speed, damage, lifetime);
            fireballScript.direction = direction;
        }
    }

    public void LevelUp()
    {
        level++;
        delay = Mathf.Max(0.5f, delay - 0.2f); // quanto mais level, menor o delay
    }
}
