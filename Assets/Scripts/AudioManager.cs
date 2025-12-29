using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource playerWalkSource;

    [SerializeField] private AudioClip enemyMagic;
    [SerializeField] private AudioClip ghostDeath;
    [SerializeField] private AudioClip playerHurt;
    [SerializeField] private AudioClip playerMagic;
    [SerializeField] private AudioClip playerWalk;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        playerWalkSource.loop = true;
        playerWalkSource.clip = playerWalk;
    }

    public void PlayEnemyMagic(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(enemyMagic, position);
    }
    
    public void PlayGhostDeath(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(ghostDeath, position);
    }
    
    public void PlayPlayerHurt()
    {
        sfxSource.PlayOneShot(playerHurt);
    }
    
    public void PlayPlayerMagic()
    {
        sfxSource.PlayOneShot(playerMagic);
    }
    
    public void StartPlayPlayerWalk()
    {
        playerWalkSource.Play();
    }
    
    public void StopPlayPlayerWalk()
    {
        playerWalkSource.Stop();
    }
}
