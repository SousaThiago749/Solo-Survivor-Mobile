using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI vidaTexto;
    public PlayerLife playerLife;

    void Update()
    {
        if (playerLife != null && vidaTexto != null)
        {
            vidaTexto.text = "" + playerLife.GetVidaAtual();
        }
    }
}
