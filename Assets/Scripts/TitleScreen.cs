using UnityEngine;
using UnityEngine.SceneManagement; 

public class TitleScreen : MonoBehaviour
{
  
   
    public void LoadRules()
    {

        //loads the rulebook
        //#TODO create a scene for the rulebook with a return button function that allows players to return to the title screen 
        SceneManager.LoadScene("Rulebook");
    }
    public void PlayGame()
    { 
        //loads whatever scene is in order, maybe the choose your character screen? 
    SceneManager.LoadScene(0); 
    }
    public void QuitGame()
    { 
        //quits game, does not work in unity editor 
    Application.Quit(); 
    }
}
