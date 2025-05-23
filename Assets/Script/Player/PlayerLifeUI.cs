using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeUI : MonoBehaviour
{
    public Slider healthSlider;
    public PlayerLife playerLife;
void Start()
{
    if (playerLife != null && healthSlider != null)
    {
        healthSlider.maxValue = playerLife.vidaMaxima;
    }
}

void Update()
{
    if (playerLife != null && healthSlider != null)
    {
        // Garante que a barra nunca passe de 0 ou do valor máximo
        healthSlider.value = Mathf.Clamp(playerLife.GetVidaAtual(), 0, playerLife.vidaMaxima);
    }
}

}
