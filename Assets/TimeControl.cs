using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeControl : MonoBehaviour
{
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject houseButton;
    [SerializeField] GameObject againButton;
    [SerializeField] GameObject soundButton;
    private float initialMaxHealth = 1f;
    

    
    public void Pause()
    {
        pauseButton.SetActive(false);
        continueButton.SetActive(true);
        houseButton.SetActive(true);
        againButton.SetActive(true);
        soundButton.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Continue()
    {
        pauseButton.SetActive(true);
        continueButton.SetActive(false);
        houseButton.SetActive(false);
        againButton.SetActive(false);
        soundButton.SetActive(false);
        Time.timeScale = 1f;
    }
    public void House()
    {
        SceneManager.LoadScene("Start Screen");
        Time.timeScale = 1f;
    }
    public void Again()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        Time.timeScale = 1f;
        Health.totalHealth = initialMaxHealth;     
    }
}

