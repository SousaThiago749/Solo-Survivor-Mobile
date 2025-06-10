using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    public int vidaMaxima = 100;
    public static int AumentoVidaMaxima = 10;
    private int vidaAtual;

    private PlayerMovement playerMovement;
    private DamageFlash damageFlash;

    void Start()
    {
        vidaAtual = vidaMaxima;
        playerMovement = GetComponent<PlayerMovement>();
        damageFlash = FindFirstObjectByType<DamageFlash>();

    }

    public int GetVidaAtual() => vidaAtual;
    public int GetVidaMaxima() => vidaMaxima;

    public void TomarDano(int dano)
    {
        vidaAtual -= dano;
        Debug.Log("Vida atual: " + vidaAtual);

        // Toca o som de dano e ativa o efeito de flash
        if (playerMovement != null && playerMovement.danoAudio != null) playerMovement.danoAudio.Play();
        if (damageFlash != null) damageFlash.Flash();

        // Se a vida do jogador for menor ou igual a zero, o jogador morre
        if (vidaAtual <= 0)
        {
            Debug.Log("Jogador morreu!");
            EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();

            if (spawner != null && GameSession.instancia != null)
            {
                GameSession.instancia.SalvarRound(spawner.GetRoundAtual());
            }
            SceneManager.LoadScene("End");
        }
    }

    public void Curar(int quantidade)
    {
        vidaAtual = Mathf.Min(vidaAtual + quantidade, vidaMaxima);
        Debug.Log($"Vida curada. Atual: {vidaAtual}/{vidaMaxima}");
    }


}
