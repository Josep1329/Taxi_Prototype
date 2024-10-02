using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class scriptDiffController : MonoBehaviour
{
    public Text levelText; // Referencia al texto del nivel
    public Text timerText; // Referencia al texto del temporizador
    public GameObject pauseMenu; // Referencia al menú de pausa
    // Referencia al script del Spawner


    private int currentLevel = 1;
    private float timer = 90f; // 1 minuto y 30 segundos
    private bool isPaused = false;



    void Start()
    {
        UpdateLevelText();
        pauseMenu.SetActive(false); // Asegurarse de que el menú de pausa esté oculto al inicio
    }

    void Update()
    {
        if (!isPaused)
        {
            // Contador regresivo
            timer -= Time.deltaTime;
            UpdateTimerText();

            // Si el tiempo se agota, pausar el juego
            if (timer <= 0)
            {
                PauseGame();
            }
        }
    }



    private void UpdateTimerText()
    {
        // Actualizar el texto del temporizador en formato mm:ss
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void PauseGame()
    {
        isPaused = true;
        pauseMenu.SetActive(true); // Mostrar el menú de pausa
        Time.timeScale = 0f; // Pausar el tiempo en el juego
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false); // Ocultar el menú de pausa
        timer = 90f; // Reiniciar el temporizador para el siguiente nivel
        Time.timeScale = 1f; // Reanudar el tiempo
    }
    public void LevelTextUpdate()
    {
        currentLevel++; // Incrementar el nivel
        UpdateLevelText(); // Actualizar el texto del nivel
    }

    private void UpdateLevelText()
    {
        levelText.text = currentLevel.ToString();
    }


}
