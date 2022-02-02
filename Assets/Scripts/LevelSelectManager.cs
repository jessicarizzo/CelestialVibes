using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelSelectManager : MonoBehaviour
{
    public uint levelId;
    public void PlaySongById()
    {
        LoadSong(levelId);
    }

    public void LoadNextLevel()
    {
        var nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        var canPlayNextLevel = SceneManager.sceneCountInBuildSettings < nextLevelIndex;

        if (canPlayNextLevel)
        {
            SceneManager.LoadScene(nextLevelIndex);
        }
        else
        {
            SceneManager.LoadScene("Scenes/StartMenu");
        }
    }
    
    private void LoadSong(uint songId)
    {
        SceneManager.LoadSceneAsync($"Scenes/Level { songId }");
    }
}
