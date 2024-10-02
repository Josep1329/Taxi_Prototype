using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptMango : MonoBehaviour
{
    private Vector3 offset;
    private Rigidbody2D rb;
    private bool isDragging = false;

    public CircleCollider2D mangoCircleCollider;
    public GameObject loseMenu;

    public Collider2D marcha1Collider; // Collider de la marcha 1
    public Collider2D marcha2Collider; // Collider de la marcha 2
    public Collider2D marcha3Collider; // Collider de la marcha 3
    public Collider2D marcha4Collider; // Collider de la marcha 4
    public Collider2D marcha5Collider; // Collider de la marcha 5
    public Collider2D marchaRCollider; // Collider de la marcha Reversa

    public scriptManecilla manecillaScript; // Referencia al script de la manecilla

     void Start()
    {
        // Obtener el Rigidbody2D
        rb = GetComponent<Rigidbody2D>();

        // Asegurarse de que el Rigidbody no sea cinemático
        rb.isKinematic = false;
    }

    void Update()
    {
        // Detectar clic en cualquier parte del sprite
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Hacer un raycast desde la posición del mouse en 2D
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            // Verificar si el raycast golpeó este objeto
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;

                // Calcular el offset entre el mouse y la posición del objeto
                offset = transform.position - GetMouseWorldPosition();
            }
        }

        // Dejar de arrastrar cuando se suelta el botón
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            // Detener el movimiento al soltar el mouse
            rb.velocity = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        // Si estamos arrastrando el objeto, moverlo con el mouse
        if (isDragging)
        {
            Vector3 mousePos = GetMouseWorldPosition() + offset;

            // Calcular la velocidad para mover el objeto hacia la posición del mouse
            Vector2 direction = (mousePos - transform.position);
            rb.velocity = direction * 10f; // Ajusta el multiplicador según lo rápido que quieras que se mueva
        }
    }

    // Convierte la posición del mouse en el espacio del mundo
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z; // Mantén la profundidad Z
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }





     // Detectar si el mango entra en un collider que es un trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.IsTouching(mangoCircleCollider))
        {
            if (other == marcha1Collider)
            {
                HandleGearChange(1);
            }
            else if (other == marcha2Collider)
            {
                HandleGearChange(2);
            }
            else if (other == marcha3Collider)
            {
                HandleGearChange(3);
            }
            else if (other == marcha4Collider)
            {
                HandleGearChange(4);
            }
            else if (other == marcha5Collider)
            {
                HandleGearChange(5);
            }
            else if (other == marchaRCollider)
            {
                Time.timeScale = 0f;
                loseMenu.SetActive(true);
            }
        }
    }

    private void HandleGearChange(int newGear)
    {
        if (manecillaScript.currentGear != newGear) 
        {
            if (Input.GetKey(KeyCode.F)) 
            {
                manecillaScript.ChangeGear(newGear); // Cambiar marcha si 'F' está presionada
            } 
            else 
            {
                Debug.Log("Cambiaste mal la marcha");
            }
            }
              }
}

