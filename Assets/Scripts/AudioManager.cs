using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Merge")]
    [SerializeField] private AudioClip[] _mergeSounds;
    [SerializeField] private float _mergeVolume = 1f;

    [Header("Victory")]
    [SerializeField] private AudioClip _victorySound;
    [SerializeField] private float _victoryVolume = 1f;

    [Header("Spawn")]
    [SerializeField] private AudioClip _spawnSound;
    [SerializeField] private float _spawnVolume = 0.7f;

    [SerializeField] private AudioSource _source;
    private int _lastMergeIndex = -1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _source.playOnAwake = false;
        _source.spatialBlend = 0f;
    }


    public void PlayMerge()
    {
        if (_mergeSounds == null || _mergeSounds.Length == 0) return;
        PlayRandom(_mergeSounds, _mergeVolume, ref _lastMergeIndex);
    }

    public void PlayVictory()
    {
        if (_victorySound != null)
            _source.PlayOneShot(_victorySound, _victoryVolume);
    }

    public void PlaySpawn()
    {
        if (_spawnSound != null)
            _source.PlayOneShot(_spawnSound, _spawnVolume);
    }


    private void PlayRandom(AudioClip[] clips, float volume, ref int lastIndex)
    {
        int index;
        if (clips.Length == 1)
        {
            index = 0;
        }
        else
        {
            do { index = Random.Range(0, clips.Length); }
            while (index == lastIndex);
        }

        lastIndex = index;
        _source.PlayOneShot(clips[index], volume);
    }
}