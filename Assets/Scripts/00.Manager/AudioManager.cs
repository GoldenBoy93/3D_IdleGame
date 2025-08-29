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
                // ���� Manager�� ������ ������ �߻����� ������ �˸�
                Debug.LogError("AudioManager is not found in the scene.");
            }
            return _instance;
        }
    }

    private AudioSource musicAudioSource; // ��� ���ǿ� AudioSource
    public AudioClip musicClip; // �⺻ ��� ���� Ŭ��

    private void Awake()
    {
        _instance = this;

        // ����� ����� AudioSource ����
        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.loop = true;
    }

    private void Start()
    {
        // �⺻ ��� ���� ����
        ChangeBackGroundMusic(musicClip);
    }

    // ��� ������ �ٸ� Ŭ������ ��ü�ϴ� �Լ�
    public void ChangeBackGroundMusic(AudioClip clip)
    {
        musicAudioSource.Stop();
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }
}