using UnityEngine;

public class BlackHoleManager : MonoBehaviour
{
    [SerializeField] private GameObject blackHolePrefab;

    public float delay = 6f;
    public float spawnRaioMaximo = 5f;
    public float spawnRaioMinimo = 1.5f;

    public int level = 1;

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
            SpawnBlackHole();
            timer = 0f;
        }
    }

    void SpawnBlackHole()
    {
        Vector2 offset;

        do
        {
            offset = Random.insideUnitCircle * spawnRaioMaximo;
        } while (offset.magnitude < spawnRaioMinimo);

        Vector3 spawnPosition = transform.position + new Vector3(offset.x, offset.y, 0f);

        GameObject go = Instantiate(blackHolePrefab, spawnPosition, Quaternion.identity);
        BlackHole script = go.GetComponent<BlackHole>();

        if (script != null)
        {
            float damage = 1f + level * 1.5f;
            float pull = 6f + level * 1f;
            float duration = 4f + level * 0.5f;

            script.damage = damage;
            script.pullForce = pull;
            script.lifetime = duration;
            script.damageInterval = Mathf.Max(0.2f, 0.8f - 0.03f * level);
        }
    }

    public void LevelUp()
    {
        level++;
        delay = Mathf.Max(2f, delay - 0.5f);
    }
}
