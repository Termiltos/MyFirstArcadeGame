using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    private Button exitButton;
    private Button retryButton;

    private void OnEnable()
    {
        exitButton = GameObject.Find("ExitButton").GetComponent<Button>();
        retryButton = GameObject.Find("RetryButton").GetComponent<Button>();

        exitButton.onClick.AddListener(CloseGame);
        retryButton.onClick.AddListener(RetryGame);
    }

    private void OnDisable()
    {
        exitButton.onClick.RemoveAllListeners();
        retryButton.onClick.RemoveAllListeners();
        exitButton = null;
        retryButton = null;
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
