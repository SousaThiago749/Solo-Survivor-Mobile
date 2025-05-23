using UnityEngine;
using System.Collections.Generic;

public class SunStrike : MonoBehaviour
{
    public List<Sprite> sunStrikeSprites;
    public List<float> frameDurations;
    public SpriteRenderer spriteRenderer;
    public bool loop = false;
    public bool canPlay = true;

    public float damage = 10f;
    public float lifetime = 2f;
    public float damageInterval = 0.5f; // tempo entre danos ao mesmo inimigo

    private float timer = 0f;
    private float frameTimer = 0f;
    private int currentFrame = 0;

    private Dictionary<EnemyMovement, float> inimigosDanoTimer = new Dictionary<EnemyMovement, float>();

    void Start()
    {
        timer = 0f;
        frameTimer = 0f;

        if (frameDurations.Count != sunStrikeSprites.Count)
        {
            frameDurations = new List<float>();
            for (int i = 0; i < sunStrikeSprites.Count; i++)
                frameDurations.Add(0.1f);
        }
    }

    void Update()
    {
        // Animação
        if (canPlay && spriteRenderer != null && sunStrikeSprites.Count > 0)
        {
            frameTimer += Time.deltaTime;

            if (frameTimer >= frameDurations[currentFrame])
            {
                frameTimer = 0f;
                currentFrame++;

                if (currentFrame >= sunStrikeSprites.Count)
                {
                    if (loop)
                        currentFrame = 0;
                    else
                    {
                        currentFrame = sunStrikeSprites.Count - 1;
                        canPlay = false;
                    }
                }

                spriteRenderer.sprite = sunStrikeSprites[currentFrame];
            }
        }

        // Auto-destruição
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyMovement enemy = other.GetComponent<EnemyMovement>();
            if (enemy != null)
            {
                if (!inimigosDanoTimer.ContainsKey(enemy))
                    inimigosDanoTimer[enemy] = 0f;

                inimigosDanoTimer[enemy] += Time.deltaTime;

                if (inimigosDanoTimer[enemy] >= damageInterval)
                {
                    enemy.TomarDano(Mathf.RoundToInt(damage));
                    inimigosDanoTimer[enemy] = 0f;
                }
            }
        }
    }
}
