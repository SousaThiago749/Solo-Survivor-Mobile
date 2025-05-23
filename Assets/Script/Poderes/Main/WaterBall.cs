using UnityEngine;
using System.Collections.Generic;

public class WaterBall : MonoBehaviour
{
    [Header("Animação")]
    public List<Sprite> sprites;                   // frames da animação
    public List<float> frameDurations;             // duração de cada frame
    public SpriteRenderer spriteRenderer;
    public bool loop = false;
    public bool canPlay = true;

    [Header("Comportamento")]
    public float damage = 10f;
    public float maxScale = 2f;
    public float growSpeed = 2f;

    private float currentScale = 0f;
    private float frameTimer = 0f;
    private int currentFrame = 0;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.localScale = Vector3.zero;

        if (frameDurations.Count != sprites.Count)
        {
            frameDurations = new List<float>();
            for (int i = 0; i < sprites.Count; i++)
                frameDurations.Add(0.1f);
        }

        // Define o primeiro sprite ao iniciar
        if (spriteRenderer != null && sprites.Count > 0)
            spriteRenderer.sprite = sprites[0];
    }

    void Update()
    {
        if (player == null) return;

        // Segue o jogador
        transform.position = player.position;

        // Crescimento
        if (transform.localScale.x < maxScale)
        {
            float increment = growSpeed * Time.deltaTime;
            currentScale += increment;
            currentScale = Mathf.Min(currentScale, maxScale);
            transform.localScale = new Vector3(currentScale, currentScale, 1f);
        }
        else
        {
            Destroy(gameObject);
        }

        // Animação dos sprites
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyMovement enemy = other.GetComponent<EnemyMovement>();
            if (enemy != null)
            {
                enemy.TomarDano(Mathf.RoundToInt(damage));
            }
        }
    }
}
