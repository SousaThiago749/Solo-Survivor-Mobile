using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class Lightning : MonoBehaviour
{
    public List<Sprite> lightningSprites;
    public List<float> frameDurations;
    public SpriteRenderer spriteRenderer;
    public bool loop = false;
    public bool canPlay = true;

    public float damage = 12f;
    public float lifetime = 1.5f;
    public Vector2 moveDirection;

    private float timer = 0f;
    private float frameTimer = 0f;
    private int currentFrame = 0;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.linearVelocity = moveDirection;

        timer = 0f;
        frameTimer = 0f;

        if (frameDurations.Count != lightningSprites.Count)
        {
            Debug.LogWarning("frameDurations do Lightning não corresponde à quantidade de sprites. Preenchendo com 0.1s por padrão.");
            frameDurations = new List<float>();
            for (int i = 0; i < lightningSprites.Count; i++)
                frameDurations.Add(0.1f);
        }
    }

    private void Update()
    {
        if (canPlay && spriteRenderer != null && lightningSprites.Count > 0)
        {
            frameTimer += Time.deltaTime;

            if (frameTimer >= frameDurations[currentFrame])
            {
                frameTimer = 0f;
                currentFrame++;

                if (currentFrame >= lightningSprites.Count)
                {
                    if (loop)
                        currentFrame = 0;
                    else
                    {
                        currentFrame = lightningSprites.Count - 1;
                        canPlay = false;
                    }
                }

                spriteRenderer.sprite = lightningSprites[currentFrame];
            }
        }

        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void SetStats(Vector2 direction, float speed, float damage, float lifetime)
    {
        moveDirection = direction.normalized * speed;
        this.damage = damage;
        this.lifetime = lifetime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyMovement inimigo = collision.GetComponent<EnemyMovement>();
            if (inimigo != null)
                inimigo.TomarDano(Mathf.RoundToInt(damage));

            Destroy(gameObject);
        }
    }
}
