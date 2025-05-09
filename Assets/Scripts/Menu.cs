using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private int _gameSceneIndex;

    public void PlayButtonOnClick()
    {
        SceneManager.LoadScene(_gameSceneIndex);
    }

    public void ApplicationQuit()
    {
        Application.Quit();
    }
}
