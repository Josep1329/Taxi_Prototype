using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptAcelerator : MonoBehaviour
{
    public Transform Acelerator; // El objeto que representa el acelerador
    public float pressedRotationX = -10f; // Rotación en X cuando el acelerador está presionado
    public float releaseRotationX = 0f; // Rotación en X cuando el acelerador está suelto
    public float aceleratorSpeed = 5f; // Velocidad de rotación del acelerador
    public float decelerationSpeed = 2f; // Velocidad de desaceleración (más lento que acelerar)
    public scriptManecilla manecillaScript; // Referencia al script de la manecilla

    private bool isAccPressed = false; // Para controlar si el acelerador está presionado
    private bool isDecelerating = false; // Para controlar si se está desacelerando




    void Update()
{
    // Detectar si se presiona o suelta la tecla Space (acelerador)
    if (Input.GetKeyDown(KeyCode.Space))
    {
        isAccPressed = true;
        isDecelerating = false; // Detener desaceleración
        manecillaScript.enabled = true; // Activar el script de la manecilla
    }
    else if (Input.GetKeyUp(KeyCode.Space))
    {
        isAccPressed = false;
        isDecelerating = true; // Iniciar desaceleración
        // Desactivar el script de la manecilla y detener el temblor
        manecillaScript.enabled = false;
        manecillaScript.isShaking = false; // Asegurar que se detiene el temblor
    }

    // Rotar el acelerador basado en si está presionado o no
    float targetRotationX = isAccPressed ? pressedRotationX : releaseRotationX;
    RotateAcelerator(targetRotationX);

    // Si se deja de presionar el acelerador, aplicar desaceleración a la manecilla
    if (isDecelerating && !manecillaScript.isShaking) // Solo desacelerar si no está temblando
    {
        DecelerateNeedle();
    }
}


    // Función para rotar el acelerador suavemente en el eje X
    private void RotateAcelerator(float targetRotationX)
    {
        Vector3 targetRotation = new Vector3(targetRotationX, Acelerator.localEulerAngles.y, Acelerator.localEulerAngles.z);
        Acelerator.localEulerAngles = Vector3.Lerp(Acelerator.localEulerAngles, targetRotation, Time.deltaTime * aceleratorSpeed);
    }

    // Función para aplicar desaceleración al velocímetro
    private void DecelerateNeedle()
    {
        if (manecillaScript.currentRotationZ < manecillaScript.minSpeedRotation) // Mientras no esté en la posición inicial
        {
            manecillaScript.currentRotationZ += decelerationSpeed * Time.deltaTime;
            manecillaScript.needle.localEulerAngles = new Vector3(0, 0, manecillaScript.currentRotationZ); // Actualizar la rotación
        }
        else
        {
            isDecelerating = false; // Detener la desaceleración una vez que ha vuelto a la posición inicial
        }
    }
}