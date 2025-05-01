using UnityEngine;
using UnityEngine.SceneManagement; 

public class TitleScreen : MonoBehaviour
{
  
   
    public void PlayGame()
    { 
        //loads whatever scene is in order, maybe the choose your character screen? 
    SceneManager.LoadScene(1); 
    }
    public void QuitGame()
    { 
        //quits game, does not work in unity editor 
    Application.Quit(); 
    }
}
