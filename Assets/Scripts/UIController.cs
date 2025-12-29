using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button playAgain;
    [SerializeField] private Image hurtUI;

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
    
    public void PlayHurt()
    {
        StopAllCoroutines();
        StartCoroutine(HurtBlink());
    }

    IEnumerator HurtBlink()
    {
        Color c = hurtUI.color;

        c.a = 0.5f;
        hurtUI.color = c;

        yield return new WaitForSeconds(0.1f);

        c.a = 0f;
        hurtUI.color = c;
    }
}