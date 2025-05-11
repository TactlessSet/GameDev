using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene(2);
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}
