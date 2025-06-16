using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<SoundManager>();

                if (instance == null)
                {
                    GameObject gameObject = new GameObject();
                    instance = gameObject.AddComponent<SoundManager>();
                    gameObject.name = "SoundManager";
                    DontDestroyOnLoad(gameObject);
                }
            }
            return instance;
        }
    }

    private void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [SerializeField]
    private AudioSource music;

    [SerializeField] 
    private AudioSource soundFXPrefab;

    public void PlayMusic(AudioClip clip, float volume)
    {
        if (music == null)
        {
            music = gameObject.AddComponent<AudioSource>();

            music.clip = clip;
            music.volume = volume;
            music.loop = true;

            music.Play();
        }
        else
        {
            music.clip = clip;
            music.volume = volume;

            music.Play();
        }
    }

    public void StopMusic()
    {
        if (music != null)
        {
            music.Stop();
        }
    }

    public AudioSource PlaySoundFXClip(AudioClip clip, float volume, Transform spawnTransform, bool loop = false)
    {
        AudioSource audioSource = Instantiate(soundFXPrefab, spawnTransform.position, Quaternion.identity);

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = loop;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        if (!loop)
        {
            Destroy(audioSource.gameObject, clipLength);
        }
        

        return audioSource;
    }

    public AudioSource PlayRandomSoundFXClip(AudioClip[] clip, float volume, Transform spawnTransform)
    {
        int rnd = Random.Range(0, clip.Length);
        
        AudioSource audioSource = Instantiate(soundFXPrefab, spawnTransform.position, Quaternion.identity);

        audioSource.clip = clip[rnd];
        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);

        return audioSource;
    }
}
