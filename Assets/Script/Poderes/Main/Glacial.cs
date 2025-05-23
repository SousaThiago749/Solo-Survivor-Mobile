using UnityEngine;
using System.Collections.Generic;

public class Glacial : MonoBehaviour
{
    public List<Sprite> glacialSprites;
    public List<float> frameDurations;
    public SpriteRenderer spriteRenderer;
    public bool loop = true;
    public bool canPlay = true;

    public float speed = 5f;
    public float damage = 8f;
    public float lifetime = 2.5f;
    public float direction = 1f;
    public float slowDuration = 1f;

    private float timer = 0f;
    private float frameTimer = 0f;
    private int currentFrame = 0;

    private void Start()
    {
        timer = 0f;
        frameTimer = 0f;

        if (frameDurations.Count != glacialSprites.Count)
        {
            Debug.LogWarning("frameDurations não corresponde ao número de sprites.");
            frameDurations = new List<float>();
            for (int i = 0; i < glacialSprites.Count; i++)
                frameDurations.Add(0.1f);
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.right * speed * direction * Time.deltaTime);

        if (canPlay && spriteRenderer != null && glacialSprites.Count > 0)
        {
            frameTimer += Time.deltaTime;

            if (frameTimer >= frameDurations[currentFrame])
            {
                frameTimer = 0f;
                currentFrame++;

                if (currentFrame >= glacialSprites.Count)
                {
                    if (loop)
                        currentFrame = 0;
                    else
                    {
                        currentFrame = glacialSprites.Count - 1;
                        canPlay = false;
                    }
                }

                spriteRenderer.sprite = glacialSprites[currentFrame];
            }
        }

        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void SetStats(float newSpeed, float newDamage, float newLifetime, float newSlowDuration)
    {
        speed = newSpeed;
        damage = newDamage;
        lifetime = newLifetime;
        slowDuration = newSlowDuration;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyMovement inimigo = collision.GetComponent<EnemyMovement>();
            if (inimigo != null)
            {
                inimigo.TomarDano(Mathf.RoundToInt(damage));
            }

            EnemyStatus status = collision.GetComponent<EnemyStatus>();
            if (status != null)
            {
                status.AplicarLentidao(slowDuration, 0.3f); // 30% mais lento
            }
        }
    }

}
