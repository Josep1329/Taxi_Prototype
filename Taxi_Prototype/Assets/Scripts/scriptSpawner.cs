using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int level = 1; // Variable del nivel
    public float frequency = 5f; // Frecuencia de spawn en segundos
    public float frequencyHighSubstraction = 0.5f; // Resta para frecuencias altas
    public float frequencyLowSubstraction = 0.05f; // Resta para frecuencias bajas
    public GameObject[] leftObjects; // Objetos de la izquierda (0: Izquierda 1, 1: Izquierda 2)
    public GameObject[] centerObjects; // Objetos del centro (0: Centro 1, 1: Centro 2)
    public GameObject[] rightObjects; // Objetos de la derecha (0: Derecha 1, 1: Derecha 2)

    private float spawnTimer;

    void Start()
    {
        spawnTimer = frequency; // Iniciar el timer
    }

    void Update()
    {
        // Actualizar el timer
        spawnTimer -= Time.deltaTime;

        // Verificar si el timer ha llegado a 0
        if (spawnTimer <= 0f)
        {
            SpawnObject(); // Llamar a la función de spawn
            spawnTimer = frequency; // Reiniciar el timer
        }
        
    }

    void SpawnObject()
    {
        int positionRandom = Random.Range(1, 4); // 1: Izquierda, 2: Centro, 3: Derecha
        GameObject objectToSpawn = null;

        int objectRandom = Random.Range(0, 2); // Cambiar a 0 y 2 para que se ajuste a los índices del array

        if (positionRandom == 1) // Izquierda
        {
            if (leftObjects.Length > 0)
            {
                objectToSpawn = leftObjects[objectRandom]; // 0 o 1 para elegir entre los dos objetos
            }
        }
        else if (positionRandom == 2) // Centro
        {
            if (centerObjects.Length > 0)
            {
                objectToSpawn = centerObjects[0]; // Solo un objeto en el centro
            }
        }
        else if (positionRandom == 3) // Derecha
        {
            if (rightObjects.Length > 0)
            {
                objectToSpawn = rightObjects[objectRandom]; // 0 o 1 para elegir entre los dos objetos
            }
        }
        // Instanciar el objeto
        if (objectToSpawn != null)
        {
            Vector3 spawnPosition = objectToSpawn.transform.position;


            // Instanciar el objeto en esa posición exacta
            GameObject clone = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

            // Obtener todos los scripts adjuntos al objeto clonado
            MonoBehaviour[] cloneScripts = clone.GetComponents<MonoBehaviour>();

            // Habilitar cada script encontrado
            foreach (MonoBehaviour script in cloneScripts)
            {
                script.enabled = true;
            }
        }

        
    }

    void UpdateFrequency()
    {
        if (frequency <= 1f)
        {
            frequency -= frequencyLowSubstraction; // Restar para frecuencias bajas
        }
        else if (frequency <= 5f)
        {
            frequency -= frequencyHighSubstraction; // Restar para frecuencias altas
        }
        if (frequency < 0.05f)
        {
            frequency = 0.05f; // Limitar la frecuencia al mínimo deseado
        }

        // Redondear la frecuencia a dos decimales
        frequency = Mathf.Round(frequency * 100f) / 100f;
    }


    // Este método se llamará cuando se cambie el nivel
    public void ChangeLevel()
    {
        level++;
        UpdateFrequency();
    }
}

