using UnityEngine;

public class GlacialManager : MonoBehaviour
{
    [SerializeField] private GameObject glacialBeamPrefab;
    public Transform firePoint;
    public float delay = 2.5f;
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
            LaunchGlacial();
            timer = 0f;
        }
    }

    void LaunchGlacial()
    {
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        Vector3 offset = new Vector3(0.5f * direction, 0f, 0f);
        Vector3 spawnPosition = transform.position + offset;

        GameObject beam = Instantiate(glacialBeamPrefab, spawnPosition, Quaternion.identity);
        beam.transform.localScale = new Vector3(direction, 1, 1);

        Glacial script = beam.GetComponent<Glacial>();
        if (script != null)
        {
            float speed = 4f + level * 0.5f;
            float damage = 3f + level * 2f;
            float lifetime = 2.5f + level * 0.3f;
            float slowDuration = 2f + level * 0.4f;

            script.SetStats(speed, damage, lifetime, slowDuration);
            script.direction = direction;
        }
    }

    public void LevelUp()
    {
        level++;
        delay = Mathf.Max(0.6f, delay - 0.2f);
    }
}
