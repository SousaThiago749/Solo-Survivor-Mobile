using UnityEngine;

public class PunchManager : MonoBehaviour
{
    [Header("Punch")]
    [SerializeField] private GameObject punchPrefab;
    public Transform punchPoint;

    [Header("Escala visual do soco")]
    public Vector3 punchScale = new Vector3(1f, 1f, 1f);

    [Header("Configuração do Tempo")]
    public float delay = 1.2f;

    [Header("Configuração do Ataque")]
    public float alcance = 1f;
    public float dano = 5f;
    public float knockbackForce = 5f;

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
            ExecutarSoco();
            timer = 0f;
        }
    }

    void ExecutarSoco()
    {
        if (punchPrefab == null || punchPoint == null)
        {
            Debug.LogWarning("PunchPrefab ou PunchPoint não atribuídos!");
            return;
        }

        float direction = transform.localScale.x > 0 ? 1f : -1f;

        GameObject punch = Instantiate(punchPrefab, punchPoint.position, Quaternion.identity);

        // ✅ Aplica a escala visual, considerando o flip horizontal
        Vector3 scale = new Vector3(punchScale.x * direction, punchScale.y, punchScale.z);
        punch.transform.localScale = scale;

        // Configura dados do ataque
        Punch punchScript = punch.GetComponent<Punch>();
        if (punchScript != null)
        {
            punchScript.SetStats(dano, alcance, knockbackForce);
        }
    }



}

