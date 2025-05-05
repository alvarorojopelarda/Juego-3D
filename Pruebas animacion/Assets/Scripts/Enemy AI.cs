using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public enum Estado { Persiguiendo, Atacando, Esquivando, Reposar, Muerto }

    [Header("Configuración")]
    public float velocidad = 3f;
    public float distanciaDeAtaque = 2f;
    public float fuerzaDeGolpe = 10f;
    public float tiempoEntreAtaques = 1.5f;
    public int hp = 5;
    public int dañoRecibido = 1;

    [Header("Referencias")]
    public Animator animator;
    public Rigidbody rb;
    public Transform objetivo;
    public LogicaBarraVida barraVida;
    public bool puedoSaltar = false;

    private Estado estadoActual = Estado.Persiguiendo;
    private float tiempoUltimoAtaque;
    private bool puedeActuar = true;
    private float tiempoDecidir = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        if (barraVida != null)
        {
            barraVida.vidaMax = hp;
            barraVida.vidaActual = hp;
            barraVida.ActualizarBarra();
        }
    }

    void Update()
    {
        if (objetivo == null || estadoActual == Estado.Muerto || !puedeActuar) return;

        float distancia = Vector3.Distance(transform.position, objetivo.position);

        // Siempre mirar al jugador
        MirarAlJugador();

        // Cada cierto tiempo decide qué hacer
        if (Time.time >= tiempoDecidir)
        {
            tiempoDecidir = Time.time + Random.Range(0.5f, 1.5f);

            if (distancia <= distanciaDeAtaque)
            {
                float decision = Random.value;

                if (decision < 0.6f && Time.time >= tiempoUltimoAtaque + tiempoEntreAtaques)
                {
                    CambiarEstado(Estado.Atacando);
                }
                else if (decision < 0.8f)
                {
                    CambiarEstado(Estado.Esquivando);
                    StartCoroutine(VolverTrasEsquivar());
                }
                else
                {
                    CambiarEstado(Estado.Reposar);
                    StartCoroutine(ReanudarTrasReposo());
                }
            }
            else
            {
                CambiarEstado(Estado.Persiguiendo);
            }
        }

        EjecutarEstado(distancia);
    }

    void EjecutarEstado(float distancia)
    {
        switch (estadoActual)
        {
            case Estado.Persiguiendo:
                if (distancia > distanciaDeAtaque)
                {
                    MoverHaciaJugador();
                }
                break;

            case Estado.Atacando:
                RealizarAtaque();
                tiempoUltimoAtaque = Time.time;
                CambiarEstado(Estado.Reposar);
                StartCoroutine(ReanudarTrasReposo());
                break;
        }
    }

    void MoverHaciaJugador()
    {
        animator.SetFloat("VelY", 1);
        Vector3 direccion = (objetivo.position - transform.position).normalized;
        Vector3 movimiento = direccion * velocidad * Time.deltaTime;
        rb.MovePosition(rb.position + new Vector3(movimiento.x, 0, movimiento.z));
    }

    void MirarAlJugador()
    {
        Vector3 lookDir = (objetivo.position - transform.position).normalized;
        lookDir.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), 10f * Time.deltaTime);
    }

    void RealizarAtaque()
    {
        animator.SetFloat("VelY", 0);
        animator.SetTrigger("golpeo");
    }

    public void AplicarGolpe()
    {
        if (objetivo != null)
        {
            Rigidbody rbObjetivo = objetivo.GetComponent<Rigidbody>();
            if (rbObjetivo != null)
            {
                Vector3 direccion = (objetivo.position - transform.position).normalized;
                rbObjetivo.AddForce(direccion * fuerzaDeGolpe, ForceMode.Impulse);
            }
        }
    }

    public void FinDeAtaque()
    {
        // Llamado por animación
        CambiarEstado(Estado.Reposar);
        StartCoroutine(ReanudarTrasReposo());
    }

    IEnumerator ReanudarTrasReposo()
    {
        puedeActuar = false;
        animator.SetFloat("VelY", 0);
        yield return new WaitForSeconds(0.6f);
        puedeActuar = true;
        CambiarEstado(Estado.Persiguiendo);
    }

    IEnumerator VolverTrasEsquivar()
    {
        puedeActuar = false;
        animator.SetTrigger("esquivar");
        yield return new WaitForSeconds(0.5f);
        puedeActuar = true;
        CambiarEstado(Estado.Persiguiendo);
    }

    void CambiarEstado(Estado nuevoEstado)
    {
        estadoActual = nuevoEstado;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (estadoActual == Estado.Muerto) return;

        if (other.CompareTag("golpeImpacto"))
        {
            animator.SetTrigger("Golpeado");
            hp -= dañoRecibido;

            if (barraVida != null)
            {
                barraVida.vidaActual = hp;
                barraVida.ActualizarBarra();
            }

            if (hp <= 0)
            {
                Morir();
            }
            else if (Random.value < 0.4f)
            {
                CambiarEstado(Estado.Esquivando);
                StartCoroutine(VolverTrasEsquivar());
            }
        }
    }

    void Morir()
    {
        estadoActual = Estado.Muerto;
        animator.SetTrigger("Morir");
        Destroy(gameObject, 2f);
    }

    public void DejeDeGolpear()
    {
        FinDeAtaque();
    }
}
