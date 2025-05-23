using UnityEngine;

public class WaterBallManager : MonoBehaviour
{
    [SerializeField] private GameObject WaterBallPrefab;

    public float delay = 1.5f;
    public float cooldownReductionPerLevel = 0.1f;
    public float minDelay = 0.4f;

    public float baseRange = 3f;
    public float rangePerLevel = 0.5f;

    public float baseDamage = 8f;
    public float damagePerLevel = 2f;

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
            SpawnWaterBall();
            timer = 0f;
        }
    }

    void SpawnWaterBall()
    {
        Vector3 spawnPosition = transform.position;
        GameObject ball = Instantiate(WaterBallPrefab, spawnPosition, Quaternion.identity);

        WaterBall script = ball.GetComponent<WaterBall>();
        if (script != null)
        {
            script.damage = baseDamage + level * damagePerLevel;
            script.maxScale = baseRange + level * rangePerLevel;
        }
    }

    public void LevelUp()
    {
        level++;
        delay = Mathf.Max(minDelay, delay - cooldownReductionPerLevel);
    }
}
