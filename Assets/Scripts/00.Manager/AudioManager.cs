using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 씬에 Manager가 없으면 에러를 발생시켜 문제를 알림
                Debug.LogError("AudioManager is not found in the scene.");
            }
            return _instance;
        }
    }

    private AudioSource musicAudioSource; // 배경 음악용 AudioSource
    public AudioClip musicClip; // 기본 배경 음악 클립

    private void Awake()
    {
        _instance = this;

        // 배경음 재생용 AudioSource 설정
        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.loop = true;
    }

    private void Start()
    {
        // 기본 배경 음악 시작
        ChangeBackGroundMusic(musicClip);
    }

    // 배경 음악을 다른 클립으로 교체하는 함수
    public void ChangeBackGroundMusic(AudioClip clip)
    {
        musicAudioSource.Stop();
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }
}