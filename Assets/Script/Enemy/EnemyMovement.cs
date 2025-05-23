using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    [Header("Atributos Iniciais por Classe")]
    public int vidaGoblin = 10;
    public int vidaOrc = 20;
    public int vidaGolem = 40;
    public int vidaDemon = 30;

    public int danoGoblin = 5;
    public int danoOrc = 10;
    public int danoGolem = 15;
    public int danoDemon = 20;

    public float velocidadeGoblin = 2f;
    public float velocidadeOrc = 1.8f;
    public float velocidadeGolem = 1.2f;
    public float velocidadeDemon = 2.2f;

    private SpriteRenderer spriteRenderer;

    [Header("Limites Finais por Classe")]
    public int vidaMaximaGoblin = 9999;
    public int vidaMaximaOrc = 9999;
    public int vidaMaximaGolem = 9999;
    public int vidaMaximaDemon = 9999;

    public int danoMaximoGoblin = 99999;
    public int danoMaximoOrc = 9999;
    public int danoMaximoGolem = 9999;
    public int danoMaximoDemon = 9999;

    public float velocidadeMaximaGoblin = 9999f;
    public float velocidadeMaximaOrc = 9999f;
    public float velocidadeMaximaGolem = 9999f;
    public float velocidadeMaximaDemon = 9999f;

    [Header("Escalonamento por Round")]
    public int incrementoVidaPorRound = 3;
    public int incrementoDanoPorRound = 3;
    public float incrementoVelocidadePorRound = 0.15f;

    public int incrementoExtraCada5Rounds = 8;

    private float velocidadeOriginal;
    private bool velocidadeModificada = false;

    [Header("Outros")]
    public AudioClip somDeMonstro;
    private AudioSource audioSource;
    private float tempoEntreSons = 10f;
    private float cronometroSom;

    private int vida;
    private int dano;
    private float speed;
    private int vidaBase, danoBase, vidaMaxPermitida, danoMaxPermitido;
    private float velocidadeBase, velocidadeMaxPermitida;

    private int vidaMaxima;
    private Transform player;
    private PlayerLife playerLife;
    private Slider barraDeVida;
    private EnemyStatus enemyStatus;


    


    [HideInInspector] public EnemySpawner spawner;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerLife = FindFirstObjectByType<PlayerLife>();
        audioSource = GetComponent<AudioSource>();
        cronometroSom = Random.Range(0f, tempoEntreSons);
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        enemyStatus = GetComponent<EnemyStatus>();


        DefinirAtributosPorClasse();

        int roundAtual = spawner != null ? spawner.RoundAtual : 1;
        int bonusVida = (roundAtual - 1) * incrementoVidaPorRound;
        int bonusDano = (roundAtual - 1) * incrementoDanoPorRound;
        float bonusVelocidade = (roundAtual - 1) * incrementoVelocidadePorRound;

        velocidadeOriginal = speed;

        if (roundAtual % 5 == 0)
        {
            bonusVida += incrementoExtraCada5Rounds;
            bonusDano += incrementoExtraCada5Rounds;
            bonusVelocidade += incrementoExtraCada5Rounds * 0.05f;
        }

        vida = Mathf.Min(vidaBase + bonusVida, vidaMaxPermitida);
        dano = Mathf.Min(danoBase + bonusDano, danoMaxPermitido);
        speed = Mathf.Min(velocidadeBase + bonusVelocidade, velocidadeMaxPermitida);
        velocidadeOriginal = speed; // salva o final correto após todos os bônus

        vidaMaxima = vida;

        barraDeVida = GetComponentInChildren<Slider>(true);
        if (barraDeVida != null)
        {
            barraDeVida.value = 1f;
            barraDeVida.minValue = 0f;
            barraDeVida.maxValue = 1f;
        }
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            float direcaoX = player.position.x - transform.position.x;
            if (spriteRenderer != null)
            {
                if (direcaoX > 0.1f)
                    spriteRenderer.flipX = false;
                else if (direcaoX < -0.1f)
                    spriteRenderer.flipX = true;
            }
        }

        cronometroSom -= Time.deltaTime;
        if (cronometroSom <= 0f && somDeMonstro != null)
        {
            audioSource.PlayOneShot(somDeMonstro);
            cronometroSom = tempoEntreSons + Random.Range(-1f, 1f);
        }

        if (speed <= 0.3f && enemyStatus != null){
            enemyStatus.RemoverLentidao();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerLife != null)
        {
            playerLife.TomarDano(dano);
        }
    }

    void DefinirAtributosPorClasse()
    {
        if (name.Contains("Goblin"))
        {
            vidaBase = vidaGoblin;
            danoBase = danoGoblin;
            velocidadeBase = velocidadeGoblin;

            vidaMaxPermitida = vidaMaximaGoblin;
            danoMaxPermitido = danoMaximoGoblin;
            velocidadeMaxPermitida = velocidadeMaximaGoblin;
        }
        else if (name.Contains("Orc"))
        {
            vidaBase = vidaOrc;
            danoBase = danoOrc;
            velocidadeBase = velocidadeOrc;

            vidaMaxPermitida = vidaMaximaOrc;
            danoMaxPermitido = danoMaximoOrc;
            velocidadeMaxPermitida = velocidadeMaximaOrc;
        }
        else if (name.Contains("Golem"))
        {
            vidaBase = vidaGolem;
            danoBase = danoGolem;
            velocidadeBase = velocidadeGolem;

            vidaMaxPermitida = vidaMaximaGolem;
            danoMaxPermitido = danoMaximoGolem;
            velocidadeMaxPermitida = velocidadeMaximaGolem;
        }
        else if (name.Contains("Demon"))
        {
            vidaBase = vidaDemon;
            danoBase = danoDemon;
            velocidadeBase = velocidadeDemon;

            vidaMaxPermitida = vidaMaximaDemon;
            danoMaxPermitido = danoMaximoDemon;
            velocidadeMaxPermitida = velocidadeMaximaDemon;
        }
        else
        {
            vidaBase = 10;
            danoBase = 5;
            velocidadeBase = 2f;

            vidaMaxPermitida = 100;
            danoMaxPermitido = 30;
            velocidadeMaxPermitida = 4f;
        }
    }

    public void TomarDano(int danoRecebido)
    {
        vida -= danoRecebido;

        if (barraDeVida != null)
            barraDeVida.value = (float)vida / vidaMaxima;

        if (vida <= 0)
        {
            PlayerLevelSystem playerXP = FindFirstObjectByType<PlayerLevelSystem>();
            if (playerXP != null)
                playerXP.RegistrarKill();

            if (spawner != null)
                spawner.InimigoMorreu();

            Destroy(gameObject);
        }
    }

    public void ModificarVelocidade(float fator)
    {
        if (!velocidadeModificada)
        {
            speed *= fator;
            velocidadeModificada = true;
        }
    }

    public void RestaurarVelocidade()
    {
        Debug.Log($"Restaurando velocidade: {speed} → {velocidadeOriginal}");
        if (velocidadeModificada)
        {
            speed = velocidadeOriginal; // agora vai restaurar corretamente
            velocidadeModificada = false;
        }

    }
}
