using UnityEngine;
using System.Collections.Generic;

public class PowerManager : MonoBehaviour
{
    public List<string> powersEscolhidos = new List<string>();
    public FireBallManager    fireBallManager;
    public SunStrikeManager   sunStrikeManager;
    public LightningManager   lightningStrikeManager;
    public WaterBallManager   waterManager;
    public BlackHoleManager   blackHoleManager;
    public GlacialManager     glacialManager;

    void Awake()
    {
        // garante que o PowerManager sobreviva ao carregar outra cena
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (GameSession.instancia != null && GameSession.instancia.isReviving)
        {
            // reaplica todos os poderes salvos
            foreach (string nome in GameSession.instancia.savedPowers)
                ChoosePower(nome);

            GameSession.instancia.ClearReviveFlag();
        }
    }

    public void ChoosePower(string nomePoder)
    {
        if (!powersEscolhidos.Contains(nomePoder))
            powersEscolhidos.Add(nomePoder);

        switch (nomePoder)
        {
            case "FireBall":         fireBallManager?.LevelUp();       break;
            case "SunStrike":        sunStrikeManager?.LevelUp();      break;
            case "LightningStrike":  lightningStrikeManager?.LevelUp();break;
            case "WaterBall":        waterManager?.LevelUp();          break;
            case "BlackHole":        blackHoleManager?.LevelUp();      break;
            case "Glacial":          glacialManager?.LevelUp();        break;
            default:
                Debug.LogWarning($"ChoosePower: poder '{nomePoder}' não reconhecido.");
                break;
        }
    }
}
