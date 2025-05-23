using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel; // arraste o PausePanel aqui

    private bool jogoPausado = false;
    void Start()
    {
        pausePanel.SetActive(false); // esconde o painel ao iniciar o jogo
    }

    public void PausarJogo()
    {
        Time.timeScale = 0f;
        jogoPausado = true;
        pausePanel.SetActive(true);
    }

    public void RetomarJogo()
    {
        Time.timeScale = 1f;
        jogoPausado = false;
        pausePanel.SetActive(false);
    }
}
