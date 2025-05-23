using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession instancia;
    public int ultimoRoundAlcancado;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject); // persiste entre cenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void SalvarRound(int round)
    {
        ultimoRoundAlcancado = round;
    }
}
