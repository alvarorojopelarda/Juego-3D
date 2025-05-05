using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // El jugador que la cámara seguirá
    public Vector3 offset = new Vector3(-5f, 3f, 0f); // Ajusta la posición de la cámara
    public float followSpeed = 5f; // Suavidad del seguimiento

    void Start()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.LookAt(target);
        }
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * followSpeed);

            transform.LookAt(target);
        }
    }
}
