using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    private EnemyMovement movement;
    private bool estaLento = false;
    private float tempoLento = 0f;
    private float fatorLentidao = 0.5f;

    void Start()
    {
        movement = GetComponent<EnemyMovement>();
    }

    void Update()
    {
        if (estaLento)
        {
            tempoLento -= Time.deltaTime;

            if (tempoLento <= 0f)
            {
                RemoverLentidao();
            }
        }
    }

    public void AplicarLentidao(float duracao, float intensidade = 0.5f)
    {
        if (!estaLento)
        {
            if (movement != null)
            {
                movement.ModificarVelocidade(intensidade); // reduz a velocidade
                estaLento = true;
                tempoLento = duracao;
                fatorLentidao = intensidade;
            }
        }
        else
        {
            tempoLento = Mathf.Max(tempoLento, duracao); // renova dura��o se j� estiver lento
        }
    }

    public void RemoverLentidao()
    {
        if (movement != null)
        {
            movement.RestaurarVelocidade();
        }

        estaLento = false;
    }
}
