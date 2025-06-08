using UnityEngine;

public class PlayerLevelSystem : MonoBehaviour{
    
    [Header("Limites de Poder")]
    public int maxPoderes = 2;

    [Header("Sistema de Nível")]
    public int nivel = 1;
    public int killsPorNivel = 5;

    private int killsAtual = 0;
    private UpgradeMenuUI upgradeMenu;

    void Start(){
        upgradeMenu = FindFirstObjectByType<UpgradeMenuUI>();
        if (upgradeMenu == null)
            Debug.LogError("UpgradeMenuUI não encontrado!");

        if (GameSession.instancia != null && GameSession.instancia.isReviving)
        {
            nivel = GameSession.instancia.savedPlayerLevel;
        }
    }

    public void RegistrarKill(){
        killsAtual++;
        if (killsAtual >= killsPorNivel){
            SubirNivel();
        }
    }

    void SubirNivel(){
        nivel++;
        killsAtual = 0;
        killsPorNivel += 3; // pode ajustar a dificuldade aqui

        if (upgradeMenu != null){
            upgradeMenu.AbrirMenu();
        }
    }

    // Chamados pelos botões
    public void AumentarVelocidade(){
        PlayerMovement movimento = FindFirstObjectByType<PlayerMovement>();
        if (movimento != null){
            movimento.speed += PlayerMovement.AumentoVelocidade;
            Debug.Log($"Velocidade aumentada para: {movimento.speed}");
        } else {
            Debug.LogWarning("PlayerMovement não encontrado!");
        }
    }

    public void AumentarVida()
    {
        PlayerLife vida = FindFirstObjectByType<PlayerLife>();
        if (vida != null)
        {
            vida.vidaMaxima += PlayerLife.AumentoVidaMaxima;
            int cura = Mathf.RoundToInt(PlayerLife.AumentoVidaMaxima * 0.5f); // cura 50% do aumento, por exemplo
            vida.Curar(cura);

            Debug.Log($"Vida máxima aumentada para: {vida.vidaMaxima}, curado: {cura} pontos.");
        }
        else
        {
            Debug.LogWarning("PlayerLife não encontrado!");
        }
    }

    public void AumentarFireBall(){
        Debug.Log("FireBall aumentado!");
        // Aumentar lógica real aqui
    }

    public void AumentarSunStrike(){
        Debug.Log("SunStrike aumentado!");
        // Aumentar lógica real aqui
    }

    public void AumentarLightning(){
        Debug.Log("Lightning aumentado!");
        // Aumentar lógica real aqui
    }

    public void AumentarBlackHole(){
        Debug.Log("Black Hole aumentado!");
        // Aumentar lógica real aqui
    }

    public void AumentarWaterBall(){
        Debug.Log("Water Ball aumentado!");
        // Aumentar lógica real aqui
    }

    public void AumentarGlacial(){
        Debug.Log("Glacial aumentado!");
        // Aumentar lógica real aqui
    }
}