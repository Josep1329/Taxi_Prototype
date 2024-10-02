using UnityEngine;
using System.Collections; // Necesario para IEnumerator

public class scriptSoloIzquierda : MonoBehaviour
{
    public float moveSpeed = 2f; // Velocidad constante del movimiento
    public string cieloSortingLayer = "Cielo"; // Asigna la capa de orden "Cielo"

    private Vector3 targetPosition; // Posición objetivo
    private bool isMoving = false; // Controla si el objeto está en movimiento
    private Collider2D obstacleCollider; // Collider del obstáculo para controlarlo

    void Start()
    {
        obstacleCollider = GetComponent<Collider2D>();
        obstacleCollider.enabled = false; // Desactivar colisiones al inicio
        StartCoroutine(MoveSequence());
    }

    private IEnumerator MoveSequence()
    {
        // 1. Mover hacia arriba en Y (3.12)
        targetPosition = new Vector3(transform.position.x, 3.12f, transform.position.z);
        yield return MoveToTarget(targetPosition);

        // 2. Cambiar el Sorting Layer a "Cielo"
        GetComponent<Renderer>().sortingLayerName = cieloSortingLayer;

        // 3. Mover hacia abajo en Y (2.15)
        targetPosition = new Vector3(transform.position.x, 2.15f, transform.position.z);
        yield return MoveToTarget(targetPosition);

        // 4. Mover en diagonal hacia (-3.91, -5.53)
        targetPosition = new Vector3(-3.91f, -5.53f, transform.position.z);
        yield return MoveToTargetWithCollision(targetPosition);

        // 5. Destruirse a sí mismo
        Destroy(gameObject);
    }

    private IEnumerator MoveToTarget(Vector3 target)
    {
        while (transform.position != target)
        {
            // Mover el objeto a la posición objetivo a una velocidad constante
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null; // Espera un frame
        }

        transform.position = target; // Asegurarse de que se establezca en la posición final
    }

    private IEnumerator MoveToTargetWithCollision(Vector3 target)
    {
        while (transform.position != target)
        {
            // Mover el objeto a la posición objetivo a una velocidad constante
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            // Activar el collider solo cuando esté en el último movimiento y Y <= -0.5
            if (transform.position.y <= -0.5f)
            {
                obstacleCollider.enabled = true; // Activar las colisiones
            }

            yield return null; // Espera un frame
        }

        transform.position = target; // Asegurarse de que se establezca en la posición final
    }

    public void ChangeLevel()
    {
        moveSpeed += .05f;
    }
}