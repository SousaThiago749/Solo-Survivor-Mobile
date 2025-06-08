using UnityEngine;
using System.Collections.Generic;

public class GameSession : MonoBehaviour
{
    public static GameSession instancia;
    public int ultimoRoundAlcancado;

    // (revive)
    public float savedPlayerSpeed;
    public int savedPlayerLevel;
    public List<string> savedPowers = new List<string>();
    public bool isReviving = false;

    // → NOVOS CAMPOS PARA O UPGRADE VIA AD
    public bool hasWatchedAdUpgrade = false;
    public string nextSceneAfterAd = "";

    // (revive) referências salvas
    public PlayerMovement playerMovement;
    public PlayerLevelSystem playerLevelSystem;
    public PowerManager powerManager;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SalvarRound(int round)
    {
        ultimoRoundAlcancado = round;
    }

    public void SavePlayerState(PlayerMovement pm, PlayerLevelSystem pl, PowerManager pw)
    {
        savedPlayerSpeed = pm.speed;
        savedPlayerLevel = pl.nivel;
        savedPowers.Clear();
        savedPowers.AddRange(pw.powersEscolhidos);
        isReviving = true;
    }

    public void ClearReviveFlag()
    {
        isReviving = false;
    }
}
