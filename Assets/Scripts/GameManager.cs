using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private Image gameOverScreen;

    private void Awake()
    {
        PlayerPrefs.SetInt("waveHighScore", 0);
        DontDestroyOnLoad(gameObject);
    }
    
    public void Reload()
    {
        gameOverScreen = GameObject.FindGameObjectWithTag("GameOverScreen").GetComponent<Image>();
        gameOverScreen.gameObject.SetActive(true);   
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
