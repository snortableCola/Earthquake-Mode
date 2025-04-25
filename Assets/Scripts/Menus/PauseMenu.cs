using UnityEngine;

public class PauseMenu : MonoBehaviour
{
   private bool isPaused = false;
    public GameObject PauseMenuUI;

    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Paused();
            }
        }
    }
    public void Paused()
    {
        Time.timeScale = 0; //freezes the game 
        PauseMenuUI.SetActive(true);   
        isPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        PauseMenuUI.SetActive(false);
        isPaused = false; 
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
