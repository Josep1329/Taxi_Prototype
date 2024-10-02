using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource; // Asigna el componente AudioSource en el inspector
    public List<AudioClip> audioClips; // Lista de clips de audio

    private void Start()
    {
        PlayRandomSong(); // Inicia la reproducci�n de una canci�n aleatoria al comienzo
    }

    private void PlayRandomSong()
    {
        // Selecciona un clip de audio aleatorio de la lista
        AudioClip randomClip = audioClips[Random.Range(0, audioClips.Count)];
        audioSource.clip = randomClip;
        audioSource.Play();

        // Espera a que la canci�n termine y luego reproduce otra aleatoria
        StartCoroutine(WaitForSongToEnd());
    }

    private IEnumerator<WaitForSeconds> WaitForSongToEnd()
    {
        // Espera el tiempo de duraci�n del clip
        yield return new WaitForSeconds(audioSource.clip.length);
        PlayRandomSong(); // Reproduce otra canci�n aleatoria
    }
}
