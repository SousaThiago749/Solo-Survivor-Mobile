using UnityEngine;

public class ColetavelSpawner : MonoBehaviour
{
    public GameObject coletavelPrefab;
    public Transform playerTransform;
    public float tempoEntreSpawns = 2f;
    public float raioSpawn = 5f;
    public float tempoDesaparecer = 30f;

    private GameObject coletavelAtual;
    private float tempoDesdeSpawn = 0f;
    private bool podeSpawnar = true;

    void Update()
    {
        if (coletavelAtual == null && podeSpawnar)
        {
            tempoDesdeSpawn += Time.deltaTime;

            if (tempoDesdeSpawn >= tempoEntreSpawns)
            {
                SpawnColetavel();
            }
        }

        // Se j� tem uma po��o na cena, inicia contagem para desaparecer
        if (coletavelAtual != null)
        {
            tempoDesdeSpawn += Time.deltaTime;

            if (tempoDesdeSpawn >= tempoDesaparecer)
            {
                Destroy(coletavelAtual);
                coletavelAtual = null;
                tempoDesdeSpawn = 0f;
            }
        }
    }

    void SpawnColetavel()
    {
        if (playerTransform == null || coletavelPrefab == null) return;

        Vector2 offset = Random.insideUnitCircle * raioSpawn;
        Vector2 spawnPos = (Vector2)playerTransform.position + offset;

        coletavelAtual = Instantiate(coletavelPrefab, spawnPos, Quaternion.identity);
        tempoDesdeSpawn = 0f;
    }
}