using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void Lore()
    {
        SceneManager.LoadScene("Lore");
    }

    public void BackMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}