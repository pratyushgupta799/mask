using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int highestWave = 0;
    [SerializeField] private Image gameOverScreen;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public void Reload()
    {
        gameOverScreen = GameObject.FindGameObjectWithTag("GameOverScreen").GetComponent<Image>();
        gameOverScreen.gameObject.SetActive(true);   
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
