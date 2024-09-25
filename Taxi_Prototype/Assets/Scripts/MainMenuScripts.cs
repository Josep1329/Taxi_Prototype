using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScripts : MonoBehaviour
{
   public void LevelStart(string LevelName)
   {
      SceneManager.LoadScene(LevelName);

   }


   public void Exit()
   {
      Application.Quit();
      Debug.Log("The game closes here");
   }
}
