using UnityEngine;
using System.Collections; // Necesario para IEnumerator

public class scriptEsquivaArribayAbajo : MonoBehaviour
{
    public float moveSpeed = 2f; // Velocidad constante del movimiento
    public string cieloSortingLayer = "Cielo"; // Asigna la capa de orden "Cielo"
    public float collisionActivationY = -0.5f; // Posici�n en Y para activar colisiones

    private Vector3 targetPosition; // Posici�n objetivo
    private Collider2D obstacleCollider; // Collider del obst�culo para controlarlo

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

        // 3. Mover hacia abajo en Y (-6.5) y activar colisiones si est� en Y <= -0.5
        targetPosition = new Vector3(transform.position.x, -6.5f, transform.position.z);
        yield return MoveToTargetWithCollision(targetPosition);

        // 4. Destruir el objeto al terminar el movimiento
        Destroy(gameObject);
    }

    private IEnumerator MoveToTarget(Vector3 target)
    {
        while (transform.position != target)
        {
            // Mover el objeto a la posici�n objetivo a una velocidad constante
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null; // Espera un frame
        }

        transform.position = target; // Asegurarse de que se establezca en la posici�n final
    }

    private IEnumerator MoveToTargetWithCollision(Vector3 target)
    {
        while (transform.position != target)
        {
            // Mover el objeto a la posici�n objetivo a una velocidad constante
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            // Activar colisiones solo cuando la posici�n Y sea menor o igual a collisionActivationY
            if (transform.position.y <= collisionActivationY)
            {
                obstacleCollider.enabled = true; // Activar las colisiones
            }

            yield return null; // Espera un frame
        }

        transform.position = target; // Asegurarse de que se establezca en la posici�n final
    }

    public void ChangeLevel()
    {
        moveSpeed += .05f;
    }
}
