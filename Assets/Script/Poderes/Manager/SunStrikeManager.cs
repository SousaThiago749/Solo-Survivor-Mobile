using UnityEngine;

public class SunStrikeManager : MonoBehaviour
{
    [SerializeField] private GameObject sunStrikePrefab;

    public float delay = 2f;
    public float spawnRaioMaximo = 4f;
    public float spawnRaioMinimo = 1f;

    public int level = 1;

    // Configuração base de progressão gradual
    public float damageBase = 10f;
    public float damagePerLevel = 1.5f;

    public float lifetimeBase = 2f;
    public float lifetimePerLevel = 0.2f;

    public float damageIntervalBase = 0.6f;
    public float damageIntervalPerLevel = 0.04f;

    private float timer;


    void OnEnable()
    {
        timer = delay; // espera o tempo cheio antes do primeiro disparo
    }



    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= delay)
        {
            SpawnSunStrike();
            timer = 0f;
        }
    }

    void SpawnSunStrike()
    {
        Vector2 offset;

        do
        {
            offset = Random.insideUnitCircle * spawnRaioMaximo;
        } while (offset.magnitude < spawnRaioMinimo);

        Vector3 spawnPosition = transform.position + new Vector3(offset.x, offset.y, 0f);

        GameObject go = Instantiate(sunStrikePrefab, spawnPosition, Quaternion.identity);

        SunStrike script = go.GetComponent<SunStrike>();
        if (script != null)
        {
            float damage = damageBase + level * damagePerLevel;
            float lifetime = lifetimeBase + level * lifetimePerLevel;
            float intervalo = Mathf.Max(0.25f, damageIntervalBase - level * damageIntervalPerLevel);

            script.damage = damage;
            script.lifetime = lifetime;
            script.damageInterval = intervalo;
        }
    }

    public void LevelUp()
    {
        level++;
        delay = Mathf.Max(0.5f, delay - 0.1f);
    }
}
