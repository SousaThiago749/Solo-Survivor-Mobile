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
                roundFinalTexto.text = "Round alcan�ado: " + roundFinal;
        }
        else
        {
            Debug.LogWarning("GameSession.instancia est� nulo! Round n�o carregado.");
        }
    }
    public void EndGame()
    {
        if (GameSession.instancia != null)
            GameSession.instancia.ultimoRoundAlcancado = 0;

        SceneManager.LoadScene("Menu");
    }
}