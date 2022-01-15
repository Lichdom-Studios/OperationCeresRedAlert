using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playlist : MonoBehaviour
{
    public static Playlist instance;

    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> songs;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {

        if (!audioSource)
            audioSource = GetComponent<AudioSource>();

        if(songs.Count > 0 && audioSource)
        {
            int index = Random.Range(0, songs.Count);

            audioSource.clip = songs[index];
            audioSource.Play();
        }
    }

    public void PlaySong(AudioClip song, float volume = 0.15f)
    {
        audioSource.clip = song;
        audioSource.volume = volume;
        audioSource.Play();
    }

}
