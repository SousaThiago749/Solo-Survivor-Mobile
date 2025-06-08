using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUIManager : MonoBehaviour
{
    public void OnReviveButtonPressed()
    {
        SceneManager.LoadScene("Ad");
    }
}
