using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Source:
//https://www.youtube.com/watch?v=w3KeuE-11GU&t=733s

public class PauseMenu : MonoBehaviour
{

    public static bool isPaused = false;
    public GameObject pauseMenuUI;

    //settings menu set up
    public GameObject settingsMenuUI;
    //

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) { 
            if(isPaused) { 
            ResumeGame();   
            }
            else{
                PauseGame();    
            }
        }    
    }

    void ResumeGame()
    {
       pauseMenuUI.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }
    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }
    //settings button set up
   public void SettingsButton()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }
    public void BackButton()
    {
        pauseMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
        isPaused = true;
        Time.timeScale = 0f;
    }
    //settings button set up
    public void PlayNowButton()
    {
        pauseMenuUI.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void QuitButton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene("Mainmenu");
    }

    public void RestartButton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainArea");
    }
}
