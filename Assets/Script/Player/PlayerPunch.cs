using UnityEngine;
using System.Collections.Generic;

public class PlayerPunch : MonoBehaviour
{
    [Header("Combate")]
    public float alcance = 1.2f;
    public float dano = 8f;
    public float intervalo = 1f;
    public float anguloMaximo = 60f;

    private float cronometro = 0f;

    [Header("Animação")]
    public List<Sprite> sprites;
    public List<float> frameDurations;
    public SpriteRenderer spriteRenderer;
    public bool loop = false;
    public bool canPlay = true;

    private int currentFrame = 0;
    private float frameTimer = 0f;

    void Start()
    {
        cronometro = 0f;
        frameTimer = 0f;

        if (frameDurations.Count != sprites.Count)
        {
            frameDurations = new List<float>();
            for (int i = 0; i < sprites.Count; i++)
                frameDurations.Add(0.1f);
        }

        if (spriteRenderer != null && sprites.Count > 0)
        {
            spriteRenderer.sprite = sprites[0];
        }
    }

    void Update()
    {
        cronometro += Time.deltaTime;

        if (cronometro >= intervalo)
        {
            TentarDarSoco();
            cronometro = 0f;

            // Reinicia animação
            if (sprites.Count > 0 && spriteRenderer != null)
            {
                currentFrame = 0;
                frameTimer = 0f;
                canPlay = true;
                spriteRenderer.sprite = sprites[0];
            }
        }

        // Controle da animação
        if (canPlay && spriteRenderer != null && sprites.Count > 0)
        {
            frameTimer += Time.deltaTime;

            if (frameTimer >= frameDurations[currentFrame])
            {
                frameTimer = 0f;
                currentFrame++;

                if (currentFrame >= sprites.Count)
                {
                    if (loop)
                        currentFrame = 0;
                    else
                    {
                        currentFrame = sprites.Count - 1;
                        canPlay = false;
                    }
                }

                spriteRenderer.sprite = sprites[currentFrame];
            }
        }
    }

    void TentarDarSoco()
    {
        Vector2 direcaoPlayer = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, alcance);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Vector2 direcaoInimigo = (hit.transform.position - transform.position).normalized;
                float angulo = Vector2.Angle(direcaoPlayer, direcaoInimigo);

                if (angulo <= anguloMaximo / 2f)
                {
                    EnemyMovement enemy = hit.GetComponent<EnemyMovement>();
                    if (enemy != null)
                    {
                        enemy.TomarDano(Mathf.RoundToInt(dano));
                    }
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, alcance);
    }
}
