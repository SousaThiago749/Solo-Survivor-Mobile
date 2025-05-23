using UnityEngine;

public class LightningManager : MonoBehaviour
{
    [Header("Prefabs por direção")]
    [SerializeField] private GameObject lightningPrefabUpRight;
    [SerializeField] private GameObject lightningPrefabUpLeft;
    [SerializeField] private GameObject lightningPrefabDownRight;
    [SerializeField] private GameObject lightningPrefabDownLeft;

    [Header("Fire Points por direção")]
    public Transform firePointUpRight;
    public Transform firePointUpLeft;
    public Transform firePointDownRight;
    public Transform firePointDownLeft;

    public float delay = 2f;
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
            SpawnAllDiagonalLightnings();
            timer = 0f;
        }
    }

    void SpawnAllDiagonalLightnings()
    {
        float speed = 4f + level * 1.2f;
        float damage = 8f + level * 1.5f;
        float lifetime = 2f + level * 0.2f;

        SpawnLightning(lightningPrefabUpRight,  firePointUpRight,  new Vector2(1, 1), speed, damage, lifetime);   // ↗️
        SpawnLightning(lightningPrefabUpLeft,   firePointUpLeft,   new Vector2(-1, 1), speed, damage, lifetime);  // ↖️
        SpawnLightning(lightningPrefabDownRight,firePointDownRight,new Vector2(1, -1), speed, damage, lifetime);  // ↘️
        SpawnLightning(lightningPrefabDownLeft, firePointDownLeft, new Vector2(-1, -1), speed, damage, lifetime); // ↙️
    }

    void SpawnLightning(GameObject prefab, Transform firePoint, Vector2 dir, float speed, float damage, float lifetime)
    {
        GameObject go = Instantiate(prefab, firePoint.position, prefab.transform.rotation);
        Lightning script = go.GetComponent<Lightning>();

        if (script != null)
        {
            script.SetStats(dir, speed, damage, lifetime);
        }
    }

    public void LevelUp()
    {
        level++;
        delay = Mathf.Max(0.4f, delay - 0.2f);
    }
}
