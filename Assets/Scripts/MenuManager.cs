using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject optionsMenu;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void PauseGame()
    {
        
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
    // void Awake()
    // {
    //     DontDestroyOnLoad(this);
    // }
    // public void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Escape))
    //     {
    //         // Set menu to opposite of existing state for subsequent key presses
    //         optionsMenu.gameObject.SetActive(!optionsMenu.gameObject.activeSelf);
    //     }
    // }
}
