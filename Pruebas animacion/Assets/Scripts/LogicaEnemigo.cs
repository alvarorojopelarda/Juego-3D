using UnityEngine;

public class LogicaEnemigo : MonoBehaviour
{
    public int hp;
    public int dañoPuño;
    public Animator anim;
    public LogicaBarraVida barraVida;

    private void Start()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }

        if (barraVida != null)
        {
            barraVida.vidaActual = hp;
            barraVida.ActualizarBarra();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("golpeImpacto"))
        {
            hp -= dañoPuño;

            if (anim != null)
            {
                anim.SetTrigger("AnimacionEnemigoPrueba1");
            }

            // Actualizar barra de vida
            if (barraVida != null)
            {
                barraVida.vidaActual = hp;
                barraVida.ActualizarBarra();
            }

            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
