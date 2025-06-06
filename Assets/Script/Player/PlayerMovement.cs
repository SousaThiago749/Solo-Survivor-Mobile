using UnityEngine;

// Certifique-se de que o seu projeto tenha o namespace correto para o Joystick. 
// Geralmente, o Joystick Pack fornece uma classe “Joystick” diretamente, 
// então não é necessário adicionar using específico para isso.

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Movimentação")]
    public float speed = 3f;                   // Velocidade base do personagem
    public Joystick joystick;                  // Arraste aqui o FixedJoystick (ou outro) do seu Canvas

    [Header("Cura e Velocidade Extra")]
    public int cura = 15;                      // Quantidade de cura ao coletar itens
    public static float AumentoVelocidade = 0.25f;

    [Header("Disparo de FireBalls")]
    public Transform firePoint;                // Ponto de disparo das FireBalls

    [Header("Áudios")]
    public AudioSource coletaAudio;            // Som de coletável
    public AudioSource danoAudio;              // (caso precise tocar áudio de dano)

    private float firePointOffsetX = 0.5f;      // Distância lateral fixa do FirePoint

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Garante que, se o firePoint estiver à direita, a posição inicial seja positiva
        if (firePoint != null)
        {
            firePoint.localPosition = new Vector3(
                Mathf.Abs(firePoint.localPosition.x),
                firePoint.localPosition.y,
                firePoint.localPosition.z
            );
            firePointOffsetX = Mathf.Abs(firePoint.localPosition.x);
        }

        // Opcional: se você quiser garantir que não chamará joystick null:
        if (joystick == null)
        {
            Debug.LogWarning("PlayerMovement: o campo 'joystick' não está atribuído no Inspector!");
        }
    }

    void FixedUpdate()
    {
        // Lê diretamente do joystick (o FixedJoystick ou outro que estiver em cena)
        float moveHorizontal = 0f;
        float moveVertical = 0f;

        if (joystick != null)
        {
            moveHorizontal = joystick.Horizontal;
            moveVertical = joystick.Vertical;
        }

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);

        // Animação de movimento: Andando se houver input no joystick
        bool estaAndando = moveHorizontal != 0f || moveVertical != 0f;
        anim.SetBool("Andando", estaAndando);

        // Flip do personagem + ajuste do FirePoint
        if (movement.x > 0f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            if (firePoint != null)
                firePoint.localPosition = new Vector3(
                    firePointOffsetX,
                    firePoint.localPosition.y,
                    firePoint.localPosition.z
                );
        }
        else if (movement.x < 0f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            if (firePoint != null)
                firePoint.localPosition = new Vector3(
                    -firePointOffsetX,
                    firePoint.localPosition.y,
                    firePoint.localPosition.z
                );
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coletavel"))
        {
            Destroy(other.gameObject);

            if (coletaAudio != null)
                coletaAudio.Play();

            PlayerLife playerLife = GetComponent<PlayerLife>();
            if (playerLife != null)
                playerLife.Curar(cura);
        }
    }
}
