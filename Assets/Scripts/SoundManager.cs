using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Sprite soundOn; 
    public Sprite soundOff; 
    public AudioSource audioSource; 
    private AudioSource audioSourceBackground; 
    private bool soundControl;
    private Button soundButton; 
    private Image buttonImage; 

    void Start()
    {
        soundButton = GetComponent<Button>();
        buttonImage = soundButton.GetComponent<Image>();

        GameObject backgroundAudioObject = GameObject.FindGameObjectWithTag("AudioBackground");
        if (backgroundAudioObject != null)
        {
            audioSourceBackground = backgroundAudioObject.GetComponent<AudioSource>();
        }

        soundControl = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;

        UpdateButtonImage();
        UpdateAudioState();

        soundButton.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        soundControl = !soundControl;

        PlayerPrefs.SetInt("SoundEnabled", soundControl ? 1 : 0);
        PlayerPrefs.Save();

        UpdateButtonImage();
        UpdateAudioState();
    }

    void UpdateButtonImage()
    {
        buttonImage.sprite = soundControl ? soundOn : soundOff;
    }

    void UpdateAudioState()
    {
        if (audioSource != null)
        {
            audioSource.enabled = soundControl;
        }

        if (audioSourceBackground != null)
        {
            audioSourceBackground.enabled = soundControl;
        }
    }
}
