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
        playerMovement = GetComponent<PlayerMovement>();
        damageFlash    = FindFirstObjectByType<DamageFlash>();

        if (GameSession.instancia != null && GameSession.instancia.isReviving)
        {
            vidaAtual = 50; // revive
            Debug.Log("Revive detectado → Vida inicial = 50");
        }
        else
        {
            vidaAtual = vidaMaxima;
        }
    }

    public int GetVidaAtual()  => vidaAtual;
    public int GetVidaMaxima() => vidaMaxima;

    public void TomarDano(int dano)
    {
        vidaAtual -= dano;
        Debug.Log("Vida atual: " + vidaAtual);

        if (playerMovement?.danoAudio != null)
            playerMovement.danoAudio.Play();
        damageFlash?.Flash();

        if (vidaAtual <= 0)
        {
            Debug.Log("Jogador morreu!");

            // 1) salva o round
            var spawner = FindFirstObjectByType<EnemySpawner>();
            if (spawner != null)
                GameSession.instancia.SalvarRound(spawner.GetRoundAtual());

            // 2) salva referências E estado enquanto os objetos ainda existem
            var pmov = GetComponent<PlayerMovement>();
            var plvl = FindFirstObjectByType<PlayerLevelSystem>();
            var pwr  = FindFirstObjectByType<PowerManager>();

            GameSession.instancia.playerMovement    = pmov;
            GameSession.instancia.playerLevelSystem = plvl;
            GameSession.instancia.powerManager      = pwr;

            GameSession.instancia.SavePlayerState(pmov, plvl, pwr);

            // 3) finalmente, vai para a tela de Game Over
            SceneManager.LoadScene("End");
        }
    }

    public void Curar(int quantidade)
    {
        vidaAtual = Mathf.Min(vidaAtual + quantidade, vidaMaxima);
        Debug.Log($"Vida curada. Atual: {vidaAtual}/{vidaMaxima}");
    }
}
