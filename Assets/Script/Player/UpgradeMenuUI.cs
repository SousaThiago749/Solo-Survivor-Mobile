using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpgradeMenuUI : MonoBehaviour
{
    [Header("Painel/Botões")]
    public GameObject painel;
    public Button[] botoesDePoder;
    public string[] nomesDosPoderes; // =  ["Health", "Speed", "FireBall", "SunStrike", "Lightning","BlackHole", "WaterBall", "Glacial"];

    [Header("Botão de Reviver")]
    public Button watchAdButton;

    [Header("Painel de Ad")]
    public GameObject painelAd;

    private HashSet<string> poderesEscolhidos = new HashSet<string>();
    private PlayerLevelSystem playerLevel;
    private Dictionary<string, MonoBehaviour> poderesManagers = new Dictionary<string, MonoBehaviour>();

    void Start()
    {
        playerLevel = FindFirstObjectByType<PlayerLevelSystem>();
        painel.SetActive(false);

        poderesManagers = new Dictionary<string, MonoBehaviour>();

        GameObject jogador = GameObject.FindWithTag("Player");

        if (jogador != null)
        {
            MonoBehaviour[] todosOsManagers = jogador.GetComponents<MonoBehaviour>();

            foreach (var script in todosOsManagers)
            {
                string tipo = script.GetType().Name;

                if (tipo.EndsWith("Manager"))
                {
                    string chave = tipo.Replace("Manager", "").ToLower(); // ex: FireBallManager → "fireball"

                    // PunchManager deve continuar ativo
                    if (tipo.ToLower().Contains("punch"))
                    {
                        Debug.Log($"[Init] Punch detectado e mantido ativo: {tipo}");
                        continue;
                    }

                    poderesManagers[chave] = script;
                    script.enabled = false;
                    Debug.Log($"[Init] Manager desativado: {tipo} como chave '{chave}'");
                }
            }
        }
        else
        {
            Debug.LogWarning("Jogador não encontrado! Confirme se ele tem a tag 'Player'.");
        }

        watchAdButton.gameObject.SetActive(!GameSession.instancia.hasWatchedAdUpgrade);
        watchAdButton.onClick.AddListener(OnWatchAdPressed);

        AtualizarEstadoDosBotoes();
    }

    public void AbrirMenu()
    {
        Debug.Log("Abrindo menu de upgrade...");
        AtualizarEstadoDosBotoes();
        painel.SetActive(true);
        StartCoroutine(CongelarTempoNoProximoFrame());
    }

    System.Collections.IEnumerator CongelarTempoNoProximoFrame()
    {
        yield return null;
        Time.timeScale = 0f;
        Debug.Log("Tempo pausado após abrir o menu.");
    }

    public void OnWatchAdPressed()
    {
        GameSession.instancia.hasWatchedAdUpgrade = true;
        watchAdButton.gameObject.SetActive(false);

        Time.timeScale = 0f;  // pausa o jogo
        painelAd.SetActive(true);

        // Inicia a coroutine para "esperar o Ad acabar"
        StartCoroutine(SimularAd());
    }





    System.Collections.IEnumerator SimularAd()
    {
        yield return new WaitForSecondsRealtime(5f);

        painelAd.SetActive(false);
        Time.timeScale = 1f;

        Debug.Log("Ad finalizado, jogo retomado e extra upgrade liberado.");
        AtualizarEstadoDosBotoes();  // <-- aqui!
    }




    // ===========================
    // ESCOLHAS DE PODER
    // ===========================

    public void EscolherFireBall()    { if (TentarEscolher("FireBall")) playerLevel.AumentarFireBall(); }
    public void EscolherSun()         { if (TentarEscolher("SunStrike")) playerLevel.AumentarSunStrike(); }
    public void EscolherLightning()   { if (TentarEscolher("LightningStrike")) playerLevel.AumentarLightning(); }
    public void EscolherBlackHole()   { if (TentarEscolher("BlackHole")) playerLevel.AumentarBlackHole(); }
    public void EscolherWaterBall()   { if (TentarEscolher("WaterBall")) playerLevel.AumentarWaterBall(); }
    public void EscolherGlacial()     { if (TentarEscolher("Glacial")) playerLevel.AumentarGlacial(); }
    public void EscolherVida()        { playerLevel.AumentarVida(); FecharMenu(); }
    public void EscolherVelocidade()  { playerLevel.AumentarVelocidade(); FecharMenu(); }

    // ===========================
    // Funções auxiliares
    // ===========================

    bool TentarEscolher(string nomePoder)
    {
        string nomePadronizado = nomePoder.ToLower().Replace(" ", "");

        // Vida e Velocidade são upgrades básicos ilimitados
        if (nomePadronizado == "health" || nomePadronizado == "speed")
        {
            Debug.Log($"Upgrade básico escolhido: {nomePoder}");
            AtualizarEstadoDosBotoes();
            FecharMenu();
            return true;
        }

        // Se já foi escolhido antes, chama LevelUp()
        else if (poderesEscolhidos.Contains(nomePadronizado))
        {
            Debug.Log($"Upgrade no poder já escolhido: {nomePoder}");
            if (poderesManagers.ContainsKey(nomePadronizado))
            {
                var metodo = poderesManagers[nomePadronizado].GetType().GetMethod("LevelUp");
                if (metodo != null)
                {
                    metodo.Invoke(poderesManagers[nomePadronizado], null);
                    Debug.Log($"Evolução chamada em: {nomePadronizado}");
                }
                else
                {
                    Debug.LogWarning($"Método LevelUp() não encontrado em {nomePadronizado}.");
                }
            }

            // 🚀 Chamar o ChoosePower de forma segura
            var pm = FindFirstObjectByType<PowerManager>();
            if (pm != null)
            {
                pm.ChoosePower(nomePoder);
            }
            else
            {
                Debug.LogWarning("PowerManager não encontrado ao tentar escolher poder!");
            }

            AtualizarEstadoDosBotoes();
            FecharMenu();
            return true;
        }

        // Calcula quantos upgrades o jogador pode pegar:
        //   maxPoderes normal + 1 extra se já viu o Ad
        int limiteEfetivo = playerLevel.maxPoderes + (GameSession.instancia.hasWatchedAdUpgrade ? 1 : 0);

        if (poderesEscolhidos.Count >= limiteEfetivo)
        {
            Debug.Log("Limite de poderes atingido.");
            return false;
        }

        // Novo poder escolhido pela primeira vez
        poderesEscolhidos.Add(nomePadronizado);
        Debug.Log($"Novo poder escolhido: {nomePoder}");

        if (poderesManagers.ContainsKey(nomePadronizado))
        {
            poderesManagers[nomePadronizado].enabled = true;
            Debug.Log($"Poder ativado: {nomePadronizado}");
        }
        else
        {
            Debug.LogWarning($"Manager não encontrado para: {nomePadronizado}");
        }

        // 🚀 Chamar o ChoosePower de forma segura
        var pmNovo = FindFirstObjectByType<PowerManager>();
        if (pmNovo != null)
        {
            pmNovo.ChoosePower(nomePoder);
        }
        else
        {
            Debug.LogWarning("PowerManager não encontrado ao tentar escolher poder!");
        }

        AtualizarEstadoDosBotoes();
        FecharMenu();
        return true;
    }

    void AtualizarEstadoDosBotoes()
    {

        int limiteEfetivo = playerLevel.maxPoderes + (GameSession.instancia.hasWatchedAdUpgrade ? 1 : 0);

        for (int i = 0; i < botoesDePoder.Length; i++)
        {
            string nomeOriginal = nomesDosPoderes[i];
            string nomePadronizado = nomeOriginal.ToLower().Replace(" ", "");

            // Vida e Velocidade nunca são barrados
            if (nomePadronizado == "health" || nomePadronizado == "speed")
            {
                botoesDePoder[i].interactable = true;
                continue;
            }

            bool jaFoiEscolhido = poderesEscolhidos.Contains(nomePadronizado);
            bool limiteAtingido = poderesEscolhidos.Count >= limiteEfetivo;
            botoesDePoder[i].interactable = jaFoiEscolhido || !limiteAtingido;
        }
    }

    void FecharMenu()
    {
        painel.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("Menu fechado e tempo restaurado.");
    }
}
