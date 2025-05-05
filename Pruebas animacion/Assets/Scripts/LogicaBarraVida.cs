using UnityEngine;
using UnityEngine.UI;

public class LogicaBarraVida : MonoBehaviour
{
    public int vidaMax;
    public int vidaActual;
    public Image imagenBarraVida;

    public void ActualizarBarra()
    {
        if (imagenBarraVida != null && vidaMax > 0)
        {
            imagenBarraVida.fillAmount = (float)vidaActual / vidaMax;
        }
    }
}
