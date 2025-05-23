using UnityEngine;
using System.Collections.Generic;

public class Punch : MonoBehaviour{
    [Header("Animação")]
    public List<Sprite> punchSprites;
    public List<float> frameDurations;
    public SpriteRenderer spriteRenderer;
    public bool loop = false;
    public bool canPlay = true;

    private float alcance;
    private float dano;
    private float knockbackForce;

    private float lifetime;
    private float timer = 0f;
    private float frameTimer = 0f;
    private int currentFrame = 0;

    public void SetStats(float danoRecebido, float alcanceRecebido, float knockbackRecebido)
    {
        dano = danoRecebido;
        alcance = alcanceRecebido;
        knockbackForce = knockbackRecebido;
    }

    void Start(){
        // Lifetime calculado baseado nas durações dos frames
        lifetime = 0f;
        if (frameDurations.Count != punchSprites.Count)
        {
            Debug.LogWarning("frameDurations não corresponde à quantidade de sprites. Preenchendo com 0.1s.");
            frameDurations = new List<float>();
            for (int i = 0; i < punchSprites.Count; i++)
                frameDurations.Add(0.1f);
        }

        foreach (float dur in frameDurations)
            lifetime += dur;

        if (spriteRenderer != null && punchSprites.Count > 0)
            spriteRenderer.sprite = punchSprites[0];

        // Dano imediato
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, alcance);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyMovement enemy = hit.GetComponent<EnemyMovement>();
                if (enemy != null)
                    enemy.TomarDano(Mathf.RoundToInt(dano));


                    Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
                    if (rb != null){
                        Vector2 dir = (enemy.transform.position - transform.position).normalized;
                        rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
                    }
            }
        }
    }

    void Update(){
        if (canPlay && spriteRenderer != null && punchSprites.Count > 0)
        {
            frameTimer += Time.deltaTime;

            if (frameTimer >= frameDurations[currentFrame])
            {
                frameTimer = 0f;
                currentFrame++;

                if (currentFrame >= punchSprites.Count)
                {
                    if (loop)
                        currentFrame = 0;
                    else
                    {
                        canPlay = false;
                        return;
                    }
                }

                spriteRenderer.sprite = punchSprites[currentFrame]; // Atualiza o sprite

                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, alcance); // Da dano na troca de frames
                foreach (Collider2D hit in hits)
                {
                    if (hit.CompareTag("Enemy"))
                    {
                        EnemyMovement enemy = hit.GetComponent<EnemyMovement>();
                        if (enemy != null)
                            enemy.TomarDano(Mathf.RoundToInt(dano));
                    }
                }

        
            }
        }



        timer += Time.deltaTime;
        if (timer >= lifetime)
            Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, alcance);
    }
}
