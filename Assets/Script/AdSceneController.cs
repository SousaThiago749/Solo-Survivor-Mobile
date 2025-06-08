using UnityEngine;
using UnityEngine.SceneManagement;

public class AdSceneController : MonoBehaviour
{
    public float adDuration = 5f;
    float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= adDuration)
        {
            // Se foi chamado de UpgradeMenuUI, volta pra lá,
            // senão, vai pra Main
            string next = GameSession.instancia.nextSceneAfterAd;
            if (!string.IsNullOrEmpty(next))
            {
                GameSession.instancia.nextSceneAfterAd = "";
                SceneManager.LoadScene(next);
            }
            else
            {
                SceneManager.LoadScene("Main");
            }
        }
    }
}
