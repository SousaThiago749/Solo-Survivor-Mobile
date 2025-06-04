using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FakeAdController : MonoBehaviour
{
    public Button botaoFechar;
    public float tempoDeEspera = 5f;
    public string cenaParaVoltar = "Main"; 

    void Start()
    {
        botaoFechar.interactable = false; 
        Invoke(nameof(AtivarBotao), tempoDeEspera); 
    }

    void AtivarBotao()
    {
        botaoFechar.interactable = true;
    }

    public void FecharAnuncio()
    {
        SceneManager.LoadScene(cenaParaVoltar);
    }
}
