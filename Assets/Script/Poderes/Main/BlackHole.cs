using UnityEngine;
using System.Collections.Generic;

public class BlackHole : MonoBehaviour
{
    [Header("Animação")]
    public List<Sprite> blackHoleSprites;
    public List<float> frameDurations;
    public SpriteRenderer spriteRenderer;
    public bool loop = true;
    public bool canPlay = true;

    [Header("Parâmetros de combate")]
    public float radius = 3f;
    public float pullForce = 5f;
    public float damage = 10f;
    public float lifetime = 4f;
    public float damageInterval = 0.5f;

    private float timer = 0f;
    private Dictionary<EnemyMovement, float> cooldowns = new Dictionary<EnemyMovement, float>();

    private float frameTimer = 0f;
    private int currentFrame = 0;

    void Start()
    {
        timer = 0f;
        frameTimer = 0f;

        if (frameDurations.Count != blackHoleSprites.Count)
        {
            Debug.LogWarning("frameDurations não corresponde à quantidade de sprites! Preenchendo com 0.1s por padrão.");
            frameDurations = new List<float>();
            for (int i = 0; i < blackHoleSprites.Count; i++)
                frameDurations.Add(0.1f);
        }
    }

    void Update()
    {
        // Animação
        if (canPlay && spriteRenderer != null && blackHoleSprites.Count > 0)
        {
            frameTimer += Time.deltaTime;

            if (frameTimer >= frameDurations[currentFrame])
            {
                frameTimer = 0f;
                currentFrame++;

                if (currentFrame >= blackHoleSprites.Count)
                {
                    if (loop)
                        currentFrame = 0;
                    else
                    {
                        currentFrame = blackHoleSprites.Count - 1;
                        canPlay = false;
                    }
                }

                spriteRenderer.sprite = blackHoleSprites[currentFrame];
            }
        }

        // Auto-destruição
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }

        // Puxar inimigos e aplicar dano
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyMovement enemy = hit.GetComponent<EnemyMovement>();
                if (enemy == null) continue;

                Vector2 direction = (transform.position - enemy.transform.position).normalized;
                enemy.transform.position += (Vector3)(direction * pullForce * Time.deltaTime);

                if (!cooldowns.ContainsKey(enemy))
                    cooldowns[enemy] = 0f;

                cooldowns[enemy] += Time.deltaTime;

                if (cooldowns[enemy] >= damageInterval)
                {
                    enemy.TomarDano(Mathf.RoundToInt(damage));
                    cooldowns[enemy] = 0f;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
