using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn")]
    public List<GameObject> inimigos;
    public Transform playerTransform;
    public float tempoEntreSpawns = 3f;
    public float raioSpawn = 8f;
    public int distanciaMinimaDoJogador = 5;

    [Header("Round Settings")]
    public int inimigosBaseNoRound1 = 5;
    public int incrementoPorRound = 2;
    public int incrementoEspecialACada5Rounds = 5;

    [Header("UI")]
    public TextMeshProUGUI textoRound;
    public TextMeshProUGUI textoInimigosRestantes;

    private float tempoProximoSpawn;
    private int roundAtual = 1;
    public int RoundAtual => roundAtual;
    private int inimigosPorRound;
    private int inimigosRestantesNoRound;
    private int inimigosVivos;

    void Start()
    {
        AtualizarRound(roundAtual);
    }

    void Update()
    {
        if (playerTransform == null) return;

        tempoProximoSpawn -= Time.deltaTime;

        if (tempoProximoSpawn <= 0f && inimigosRestantesNoRound > 0 && inimigosVivos < inimigosPorRound)
        {
            SpawnInimigo();
            tempoProximoSpawn = tempoEntreSpawns;
        }
    }

    void SpawnInimigo()
    {
        if (inimigos.Count == 0) return;

        Vector2 spawnPosition;
        int tentativas = 0;
        do
        {
            spawnPosition = playerTransform.position + (Vector3)(Random.insideUnitCircle.normalized * Random.Range(distanciaMinimaDoJogador, raioSpawn));
            tentativas++;
        } while (Vector2.Distance(spawnPosition, playerTransform.position) < distanciaMinimaDoJogador && tentativas < 10);

        GameObject inimigoPrefab = inimigos[Random.Range(0, inimigos.Count)];
        GameObject novoInimigo = Instantiate(inimigoPrefab, spawnPosition, Quaternion.identity);
        inimigosVivos++;
        inimigosRestantesNoRound--;
        AtualizarTextoInimigos();

        EnemyMovement movimento = novoInimigo.GetComponent<EnemyMovement>();
        if (movimento != null)
        {
            movimento.spawner = this;
        }
    }

    public void InimigoMorreu()
    {
        inimigosVivos--;
        AtualizarTextoInimigos();

        if (inimigosVivos <= 0 && inimigosRestantesNoRound <= 0)
        {
            roundAtual++;
            AtualizarRound(roundAtual);
        }
    }

    void AtualizarRound(int round)
    {
        inimigosPorRound = inimigosBaseNoRound1 + (round - 1) * incrementoPorRound;

        if (round % 5 == 0)
        {
            inimigosPorRound += incrementoEspecialACada5Rounds;
        }

        inimigosRestantesNoRound = inimigosPorRound;
        inimigosVivos = 0;

        if (textoRound != null)
            textoRound.text = "Round: " + round;

        AtualizarTextoInimigos();
    }

    void AtualizarTextoInimigos()
    {
        if (textoInimigosRestantes != null)
        {
            int totalRestantes = inimigosRestantesNoRound + inimigosVivos;
            textoInimigosRestantes.text = "Inimigos: " + totalRestantes;
        }
    }

    public int GetRoundAtual()
    {
        return roundAtual;
    }
}
