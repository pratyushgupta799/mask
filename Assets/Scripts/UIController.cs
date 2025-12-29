using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button playAgain;

    void OnEnable()
    {
        playAgain.onClick.AddListener(PlayAgain);
    }

    void OnDisable()
    {
        playAgain.onClick.RemoveListener(PlayAgain);
    }

    void PlayAgain()
    {
        GameManager.Instance.Reload();
    }
}