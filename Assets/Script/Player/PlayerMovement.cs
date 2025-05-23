using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public float speed = 3f;
    public int cura = 15; // quantidade de cura
    public static float AumentoVelocidade = 0.25f;
    
    public Transform firePoint; // 🔥 Ponto de disparo das FireBalls

    // Áudios
    public AudioSource coletaAudio;
    public AudioSource danoAudio;

    private float firePointOffsetX = 0.5f; // Valor fixo de distância lateral (pode ajustar no Inspector se quiser)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Garante que o firePoint começa do lado direito
        if (firePoint != null)
        {
            firePoint.localPosition = new Vector3(Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, firePoint.localPosition.z);
            firePointOffsetX = Mathf.Abs(firePoint.localPosition.x); // salva a distância original
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);

        // Animação de movimento
        bool estaAndando = moveHorizontal != 0 || moveVertical != 0;
        anim.SetBool("Andando", estaAndando);

        // Flip do personagem + ajuste do FirePoint
        if (movement.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            if (firePoint != null)
                firePoint.localPosition = new Vector3(firePointOffsetX, firePoint.localPosition.y, firePoint.localPosition.z);
        }
        else if (movement.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            if (firePoint != null)
                firePoint.localPosition = new Vector3(-firePointOffsetX, firePoint.localPosition.y, firePoint.localPosition.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coletavel")
        {
            Destroy(other.gameObject);
            if (coletaAudio != null) coletaAudio.Play();

            PlayerLife playerLife = GetComponent<PlayerLife>();
            if (playerLife != null)
            {
                playerLife.Curar(cura);
            }
        }
    }
}
