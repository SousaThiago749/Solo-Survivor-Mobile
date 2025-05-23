using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    public TextMeshProUGUI roundFinalTexto;

    void Start()
    {
        if (GameSession.instancia != null)
        {
            int roundFinal = GameSession.instancia.ultimoRoundAlcancado;
            Debug.Log("Round final: " + roundFinal);
            if (roundFinalTexto != null)
                roundFinalTexto.text = "Round alcançado: " + roundFinal;
        }
        else
        {
            Debug.LogWarning("GameSession.instancia está nulo! Round não carregado.");
        }
    }
    public void EndGame()
    {
        if (GameSession.instancia != null)
            GameSession.instancia.ultimoRoundAlcancado = 0;

        SceneManager.LoadScene("Menu");
    }
}