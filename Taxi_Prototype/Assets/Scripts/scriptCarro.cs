using UnityEngine;

public class ScriptCarro : MonoBehaviour
{
    public Transform carTransform;
    public GameObject loseMenu;
    public GameObject calacaChida; // Referencia al objeto "calaca chida"
    public scriptMango scriptMango; // Referencia al script del mango
    public scriptManecilla manecillaScript; // Referencia al script de la manecilla

    public Vector3 targetPositionLeft = new Vector3(-2.42f, -0.24f, 0f);
    public Vector3 initialPosition = new Vector3(0.24f, -1.26f, 0f);
    public Quaternion targetRotationLeft = Quaternion.Euler(0, 0, 43.984f);
    public Quaternion initialRotation = Quaternion.identity;

    public Vector3 targetPositionRight = new Vector3(2.42f, -0.24f, 0f);
    public Quaternion targetRotationRight = Quaternion.Euler(0, 0, -43.984f);

    public Vector3 targetPositionUp = new Vector3(0.24f, 1.85f, 0f);
    public float moveDuration = 1.0f;
    public float staggerDuration = 5.0f;

    private bool isMoving = false;
    private bool reachedTarget = false;
    private bool isReturning = false;
    private bool isInStagger = false;

    private float moveTimer = 0f;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private float startTime;

    void Update()
    {
        if (!isMoving && !isReturning)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                StartMovement(targetPositionLeft, targetRotationLeft);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                StartMovement(targetPositionRight, targetRotationRight);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                StartJumpMovement();
            }
            else if (Input.GetKeyDown(KeyCode.S) && isInStagger)
            {
                Debug.Log("Retorno forzado con S.");
                StartReturnMovement();
            }
        }

        if (isMoving)
        {
            moveTimer += Time.deltaTime;
            float progress = moveTimer / moveDuration;

            if (targetPosition == targetPositionUp)
            {
                carTransform.position = Vector3.Lerp(carTransform.position, targetPosition, progress);
            }
            else
            {
                carTransform.position = Vector3.Lerp(carTransform.position, targetPosition, progress);
                carTransform.rotation = Quaternion.Lerp(carTransform.rotation, targetRotation, progress);
            }

            if (Vector3.Distance(carTransform.position, targetPosition) < 0.01f && !reachedTarget && !isReturning)
            {
                float elapsedTime = Time.time - startTime;
                Debug.Log($"Llegué al objetivo en {elapsedTime} segundos.");

                moveTimer = 0f;
                isMoving = false;
                reachedTarget = true;
                isInStagger = true;

                Debug.Log($"Esperando {staggerDuration} segundos para el retorno.");
                Invoke(nameof(StartReturnMovement), staggerDuration);
            }
        }

        if (isInStagger && !isReturning)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                Debug.Log("Forzando retorno durante el stagger.");
                CancelInvoke(nameof(StartReturnMovement));
                StartReturnMovement();
            }
        }

        if (isReturning)
        {
            moveTimer += Time.deltaTime;
            float progress = moveTimer / moveDuration;

            carTransform.position = Vector3.Lerp(carTransform.position, initialPosition, progress);
            carTransform.rotation = Quaternion.Lerp(carTransform.rotation, initialRotation, progress);

            if (Vector3.Distance(carTransform.position, initialPosition) < 0.01f)
            {
                isMoving = false;
                reachedTarget = false;
                isReturning = false;
                isInStagger = false;
                Debug.Log("Posición inicial alcanzada, isMoving es falso.");
            }
        }
    }

    private void StartMovement(Vector3 targetPos, Quaternion targetRot)
    {
        if (isMoving || isReturning) return;

        targetPosition = targetPos;
        targetRotation = targetRot;
        moveTimer = 0f;
        isMoving = true;
        reachedTarget = false;
        startTime = Time.time;

        Debug.Log($"Iniciando movimiento hacia {targetPos} a las {startTime} segundos.");
    }

    private void StartJumpMovement()
    {
        if (isMoving || isReturning) return;

        targetPosition = targetPositionUp;
        moveTimer = 0f;
        isMoving = true;
        reachedTarget = false;
        startTime = Time.time;

        Debug.Log($"Iniciando salto hacia {targetPosition} a las {startTime} segundos.");
    }

    private void StartReturnMovement()
    {
        if (isReturning) return;

        moveTimer = 0f;
        isMoving = true;
        isReturning = true;
        isInStagger = false;

        targetPosition = initialPosition;
        targetRotation = initialRotation;

        Debug.Log("Iniciando retorno a la posición inicial.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Colisión detectada con un objeto.");
            Destroy(other.gameObject);
            // 1. Obtener la marcha actual del objeto "manecilla"
            int currentGear = manecillaScript.currentGear;

            // 2. Si estamos en una marcha superior a 3, retroceder 3 marchas
            int newGear = currentGear - 3;
            if (newGear <= 0)
            {
                loseMenu.SetActive(true); // Activar el menú de pérdida
                Debug.Log("Has perdido perdiste mucha velocidad");
                Time.timeScale = 0f;
            }
            else
            {
                // 3. Mover el collider del mango a la posición del collider de la nueva marcha
                MoveMangoToNewGear(newGear);
                Debug.Log($"Retrocediendo a la marcha {newGear}.");
            }
        }
    }

    private void MoveMangoToNewGear(int gear)
    {
        switch (gear)
        {
            case 1:
                scriptMango.transform.position = scriptMango.marcha1Collider.transform.position;
                manecillaScript.currentGear = 1; // Cambiar oficialmente la marcha
                break;
            case 2:
                scriptMango.transform.position = scriptMango.marcha2Collider.transform.position;
                manecillaScript.currentGear = 2; // Cambiar oficialmente la marcha
                break;
            case 3:
                scriptMango.transform.position = scriptMango.marcha3Collider.transform.position;
                manecillaScript.currentGear = 3; // Cambiar oficialmente la marcha
                break;
            case 4:
                scriptMango.transform.position = scriptMango.marcha4Collider.transform.position;
                manecillaScript.currentGear = 4; // Cambiar oficialmente la marcha
                break;
            case 5:
                scriptMango.transform.position = scriptMango.marcha5Collider.transform.position;
                manecillaScript.currentGear = 5; // Cambiar oficialmente la marcha
                break;
            default:
                Debug.LogWarning("Marcha inválida.");
                break;
        }
    }
}