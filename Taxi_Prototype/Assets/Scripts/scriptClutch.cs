using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptClutch : MonoBehaviour
{
    public Transform clutch; // El objeto que representa el clutch (mango de velocidades)
    public float pressedRotationX = -10f; // Rotación en X cuando el clutch está presionado
    public float releaseRotationX = 0f; // Rotación en X cuando el clutch está suelto
    public float clutchSpeed = 5f; // Velocidad de rotación

    private bool isClutchPressed = false; // Para controlar si el clutch está presionado

    void Update()
    {
        // Detectar si se presiona o suelta la tecla F
        if (Input.GetKeyDown(KeyCode.F))
        {
            isClutchPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            isClutchPressed = false;
        }

        // Rotar el clutch basado en si está presionado o no
        float targetRotationX = isClutchPressed ? pressedRotationX : releaseRotationX;
        RotateClutch(targetRotationX);
    }

    // Función para rotar el clutch suavemente en el eje X
    private void RotateClutch(float targetRotationX)
    {
        Vector3 targetRotation = new Vector3(targetRotationX, clutch.localEulerAngles.y, clutch.localEulerAngles.z);
        clutch.localEulerAngles = Vector3.Lerp(clutch.localEulerAngles, targetRotation, Time.deltaTime * clutchSpeed);
    }
}
