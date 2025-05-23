using UnityEngine;
using UnityEngine.UI;

public class DamageFlash : MonoBehaviour
{
    public Image damageImage;
    public float flashDuration = 0.2f;
    public Color flashColor = new Color(1, 0, 0, 0.4f);

    private float flashTimer;

    void Update()
    {
        if (damageImage == null) return;

        // Suaviza o alpha de volta para 0
        if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;
            damageImage.color = Color.Lerp(flashColor, Color.clear, 1 - (flashTimer / flashDuration));
        }
        else
        {
            damageImage.color = Color.clear;
        }
    }

    public void Flash()
    {
        flashTimer = flashDuration;
        damageImage.color = flashColor;
    }
}
