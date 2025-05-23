using UnityEngine;
using System.Collections.Generic;

public class FireBall : MonoBehaviour
{
    public List<Sprite> fireBallSprites;                // Sprites da animação
    public List<float> frameDurations;                  // Duração de cada sprite individualmente
    public SpriteRenderer spriteRenderer;
    public bool loop = false;
    public bool canPlay = true;

    public float speed = 5f;
    public float damage = 10f;
    public float lifetime = 2f;
    public float direction = 1f;

    private float timer = 0f;
    private float frameTimer = 0f;
    private int currentFrame = 0;

    private void Start(){
        timer = 0f;
        frameTimer = 0f;

        // Segurança: impedir falhas se lista estiver incompleta
        if (frameDurations.Count != fireBallSprites.Count)
        {
            Debug.LogWarning("frameDurations não corresponde à quantidade de sprites! Preenchendo com 0.1s por padrão.");
            frameDurations = new List<float>();
            for (int i = 0; i < fireBallSprites.Count; i++)
            {
                frameDurations.Add(0.1f);
            }
        }
    }

    private void Update(){
        // Movimento
        transform.Translate(Vector3.right * speed * direction * Time.deltaTime);

        // Animação por frame com durações customizadas
        if (canPlay && spriteRenderer != null && fireBallSprites.Count > 0)
        {
            frameTimer += Time.deltaTime;

            if (frameTimer >= frameDurations[currentFrame])
            {
                frameTimer = 0f;
                currentFrame++;

                if (currentFrame >= fireBallSprites.Count)
                {
                    if (loop)
                    {
                        currentFrame = 0;
                    }
                    else
                    {
                        currentFrame = fireBallSprites.Count - 1;
                        canPlay = false;
                    }
                }

                spriteRenderer.sprite = fireBallSprites[currentFrame];
            }
        }

        // Auto-destruição após lifetime
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void SetStats(float newSpeed, float newDamage, float newLifetime){
        speed = newSpeed;
        damage = newDamage;
        lifetime = newLifetime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyMovement inimigo = collision.GetComponent<EnemyMovement>();
            if (inimigo != null)
            {
                inimigo.TomarDano(Mathf.RoundToInt(damage)); // agora usa o valor de dano
            }

            Destroy(gameObject);
        }
    }


}
