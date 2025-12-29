using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int highestWave = 0;
    [SerializeField] private Image gameOverScreen;
    [SerializeField] private Image startScreen;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        StartGame();
    }

    private void StartGame()
    {
        Time.timeScale = 0;
        startScreen.gameObject.SetActive(true);
    }

    public void PlayGame()
    {
        Debug.Log("Playing Game");
        startScreen.gameObject.SetActive(false);
        Time.timeScale = 1;
        
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void Reload()
    {
        gameOverScreen = GameObject.FindGameObjectWithTag("GameOverScreen").GetComponent<Image>();
        gameOverScreen.gameObject.SetActive(true);   
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
