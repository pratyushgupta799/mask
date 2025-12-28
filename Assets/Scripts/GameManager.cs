using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private Image gameOverScreen;

    public void Reload()
    {
        gameOverScreen.gameObject.SetActive(true);   
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
