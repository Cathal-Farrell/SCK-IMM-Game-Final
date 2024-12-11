using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    private Button button;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        button = GetComponent<Button>();
        button.onClick.AddListener(Clicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    void Clicked(){
        Debug.Log(button.gameObject.name + " was clicked");

        if (button.gameObject.name == "Play"){
            gameManager.StartGame();
        }

        else if (button.gameObject.name == "Settings"){
            gameManager.Settings();
        }

        else if (button.gameObject.name == "Controls"){
            gameManager.Controls();
        }

        else if (button.gameObject.name == "Github Link"){
            gameManager.GithubLink();
        }

        else if (button.gameObject.name == "Exit"){
            gameManager.Exit();
        }

        else if (button.gameObject.name == "Back"){
            gameManager.Back();
        }

        else if (button.gameObject.name == "Play Again"){
            // Stop playing the game over music 
            gameManager.StopAllMusic();

            // Start playing the background music
            gameManager.PlayBackgroundMusic();

            // Show game over screen
            gameManager.gameOverScreen.SetActive(false);

            // Unfreeze the screen
            Time.timeScale = 1f;

            // Start the game 
            gameManager.StartGame();
        }

        else if (button.gameObject.name == "Main Menu"){
            // Stop playing the game over music 
            gameManager.StopAllMusic();

            // Start playing the background music
            gameManager.PlayBackgroundMusic();

            //Stop showing game over screen and all UI
            gameManager.gameOverScreen.SetActive(false);
            gameManager.health.SetActive(false); 
            gameManager.wave.SetActive(false); 
            gameManager.ammo.SetActive(false); 

            // Show title screen
            gameManager.titleScreen.SetActive(true);

            // Unfreeze the game 
            Time.timeScale = 1f;

            // Load Scene
            SceneManager.LoadScene("Main Menu");
        }
    }   
}
