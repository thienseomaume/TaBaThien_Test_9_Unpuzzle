using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    [SerializeField] AudioClip brokenSound;
    [SerializeField] AudioClip correctSound;
    [SerializeField] AudioClip errorSound;
    AudioSource audioSource;
    public static SoundManager Instance()
    {
        return instance;
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayBrokenSound()
    {
        audioSource.PlayOneShot(brokenSound);
    }
    public void PlayCorrectSound()
    {
        audioSource.PlayOneShot(correctSound);
    }
    public void PlayErrorSound()
    {
        audioSource.PlayOneShot(errorSound);
    }
    
}
