using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float runSpeed = 7f;

    private bool moveForward;
    private bool moveBackward;
    private bool isCrouching;

    public float fallMultiplier = 2.5f;
    public Animator animator;
    public Rigidbody rb;
    public float fuerzaDeSalto = 8f;
    public bool puedoSaltar;

    public bool estoyAtacando;
    public bool avanzoSolo;
    public float impulsoDeGolpe = 10f;

    private float velocidadInicial;
    private float velocidadAgachado;

    void Start()
    {
        puedoSaltar = false;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No se encontró Rigidbody en el GameObject.");
        }
        rb.freezeRotation = true;

        velocidadInicial = runSpeed;
        velocidadAgachado = runSpeed * 0.5f;
    }

    void Update()
    {
        // Movimiento ofensivo
        if (avanzoSolo)
        {
            rb.linearVelocity = transform.forward * impulsoDeGolpe;
        }

        // Mejora de salto con gravedad
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        // Estado de salto y caída
        animator.SetBool("tocoSuelo", puedoSaltar);
        if (!puedoSaltar) animator.SetBool("salte", false);

        // Agacharse
        if (isCrouching)
        {
            animator.SetBool("agachado", true);
            runSpeed = velocidadAgachado;
        }
        else
        {
            animator.SetBool("agachado", false);
            runSpeed = velocidadInicial;
        }

        // Movimiento normal
        Vector3 move = Vector3.zero;

        if (moveForward)
        {
            move = transform.forward;
            animator.SetFloat("VelY", 1);
        }
        else if (moveBackward)
        {
            move = -transform.forward;
            animator.SetFloat("VelY", -1);
        }
        else
        {
            animator.SetFloat("VelY", 0);
        }

        rb.MovePosition(rb.position + move * runSpeed * Time.deltaTime);
    }

    public void JumpButton()
    {
        if (puedoSaltar)
        {
            animator.SetBool("salte", true);
            rb.AddForce(Vector3.up * fuerzaDeSalto, ForceMode.Impulse);
            puedoSaltar = false;
        }
    }

    // Métodos UI o para teclas
    public void MoveForwardDown() => moveForward = true;
    public void MoveForwardUp() => moveForward = false;

    public void MoveBackwardDown() => moveBackward = true;
    public void MoveBackwardUp() => moveBackward = false;

    public void AgacharseDown() => isCrouching = true;
    public void AgacharseUp() => isCrouching = false;

    public void Atacar()
    {
        if (!estoyAtacando)
        {
            estoyAtacando = true;
            animator.SetTrigger("golpeo");
        }
    }

    // Llamado por un Animation Event al final del golpe
    public void DejeDeGolpear()
    {
        estoyAtacando = false;
        avanzoSolo = false;
    }

    public void AvanzoSolo()
    {
        avanzoSolo = true;
    }

    public void DejoDeAvanzar()
    {
        avanzoSolo = false;
    }
}
