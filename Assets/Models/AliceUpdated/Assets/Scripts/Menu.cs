using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
  public  void Update()
    {
        
    }

    public  void Playgame()
    {
        SceneManager.LoadScene("Main");
        //SceneManager.LoadScene("UI_InGame", LoadSceneMode.Additive);
    }

    public void NextLevel()
    {
        //Next Level Scence should be here 
        SceneManager.LoadScene("Middle");

    }
    public void LastLevel()
    {
        SceneManager.LoadScene("Level 2");
    }
  public  void playCredit()
    {

        SceneManager.LoadScene("Credit");

 
    }

    public void WinScreen()
    {
        SceneManager.LoadScene("WinGame");

    }
   public void QuitGame()
    {

        Application.Quit();


    }
}
