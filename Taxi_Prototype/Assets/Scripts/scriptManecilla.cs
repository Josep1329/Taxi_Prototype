using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptManecilla : MonoBehaviour
{
    public Transform needle; // El objeto de la aguja del velocímetro
    public float maxSpeedRotation = -127.392f; // Rotación máxima (-127.392)
    public float minSpeedRotation = 127.392f;  // Rotación mínima (127.392)
    public float rotationSpeed; // Velocidad actual de rotación
    public float decelerationNearMax = 0.5f; // Factor de desaceleración cerca del límite máximo
    public float gearDeceleration = 0.5f; // Desaceleración específica para marchas
    public float shakeMagnitude = 0.1f; // Magnitud del temblor
    public int currentGear = 1; // Marcha actual
    public float currentRotationZ; // Rotación actual en Z
    public bool isShaking = false; // Estado del temblor

    private int maxGear = 5; // Máxima marcha
    private Coroutine shakeCoroutine;
    private float ghostPosition; // Posición fantasma para el temblor
    private bool touchfinalgear = false;

    // Velocidades predeterminadas para cada marcha
    private float[] gearSpeeds = new float[]
    {
        40f, // Primera marcha
        50f, // Segunda marcha
        60f, // Tercera marcha
        70f, // Cuarta marcha
        80f  // Quinta marcha
    };

    // Límites de rotación para cada marcha
    private float[] gearLimits = new float[]
    {
        66.345f,   // Marcha 1 (60 km/h)
        18.209f,   // Marcha 2 (110 km/h)
        -37.959f,  // Marcha 3
        -86.717f,  // Marcha 4
        -127.392f  // Marcha 5
    };

    void Start()
    {
        // Establecer la rotación inicial de la aguja y velocidad para la primera marcha
        currentRotationZ = needle.localEulerAngles.z;
        ghostPosition = currentRotationZ; // Inicializar la posición fantasma
        rotationSpeed = gearSpeeds[currentGear - 1]; // Inicializar la velocidad de la marcha actual
    }

    void Update()
    {
        // Simular el movimiento continuo de la aguja
        if (currentRotationZ > maxSpeedRotation)
        {
            // Ajustar la velocidad de desaceleración según la proximidad al límite máximo
            float distanceToMax = Mathf.Abs(currentRotationZ - maxSpeedRotation);
            float adjustedSpeed = rotationSpeed;

            // Aplicar desaceleración específica para las primeras 3 marchas
            if (currentGear <= 3)
            {
                adjustedSpeed += gearDeceleration; // Desaceleración de marcha
            }

            // Aplicar desaceleración cerca del límite máximo
            adjustedSpeed *= (distanceToMax / Mathf.Abs(minSpeedRotation - maxSpeedRotation));

            // Actualizar la rotación actual
            currentRotationZ -= adjustedSpeed * Time.deltaTime;

            // Limitar la rotación en función de la marcha actual
            if (currentGear > 0 && currentGear <= maxGear)
            {
                if (currentRotationZ <= gearLimits[currentGear - 1])
                {
                    currentRotationZ = gearLimits[currentGear - 1]; // Limitar a la posición de la marcha

                    // Agregar efecto de temblor si alcanza un límite de marcha
                    if (!isShaking)
                    {
                        touchfinalgear = true;
                        StartCoroutine(ShakeNeedle());
                    }
                }
            }

            // Actualizar la posición fantasma
            ghostPosition = currentRotationZ;

            // Agregar efecto de temblor si alcanza un valor específico
            if (currentRotationZ <= -47.545f && !isShaking)
            {
                StartCoroutine(ShakeNeedle());
            }
        }

        // Actualizar la rotación en el editor para ver el cambio inmediato
        needle.localEulerAngles = new Vector3(0, 0, currentRotationZ);
    }

    public void ChangeGear(int newGear)
    {
        if (newGear <= maxGear)
        {
            // Cambiar a la velocidad predeterminada de la marcha nueva
            rotationSpeed = gearSpeeds[newGear - 1];
            currentGear = newGear;
            touchfinalgear = false;
            isShaking = false;

            Debug.Log("Marcha cambiada a: " + currentGear);
        }
    }

    private IEnumerator ShakeNeedle()
    {
        isShaking = true;

        while (isShaking)
        {
            // Generar un desplazamiento aleatorio en Z alrededor de la posición fantasma
            float shakeZ = Random.Range(-shakeMagnitude, shakeMagnitude);
            needle.localEulerAngles = new Vector3(0, 0, ghostPosition + shakeZ);

            // Esperar un frame
            yield return null;
        }

        isShaking = false;
        touchfinalgear = false;
    }

    public void StopShake()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            isShaking = false;
            // Resetear la posición de la aguja al valor de ghostPosition
            needle.localEulerAngles = new Vector3(0, 0, ghostPosition);
        }
    }
}
