using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [SerializeField] Toggle vSync;
    [Header("Controls")]
    public static KeyCode interactKey = KeyCode.E;
    public static KeyCode dropKey = KeyCode.G;
    public static KeyCode runKey = KeyCode.LeftShift;
    public static KeyCode crouchtKey = KeyCode.LeftControl;
    [Header("Localization")]
    [SerializeField] Button rusLanguage;
    [SerializeField] Button engLanguage;
    int currentLanguage = 0;
    void Start()
    {
        rusLanguage.onClick.AddListener(() => SetLanguage(0));
        engLanguage.onClick.AddListener(() => SetLanguage(1));
        LoadSettings();
    }
    void LoadSettings()
    {
        QualitySettings.vSyncCount = PlayerPrefs.GetInt("VSync");
        if (QualitySettings.vSyncCount == 1) vSync.isOn = true;
        else vSync.isOn = false;
    }

    public void SetLanguage(int count)
    {
        currentLanguage = count;
        PlayerPrefs.SetInt("language", currentLanguage);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetVSync(bool ToF)
    {
        if (ToF) QualitySettings.vSyncCount = 1;
        else QualitySettings.vSyncCount = 0;
        PlayerPrefs.SetInt("VSync", QualitySettings.vSyncCount);
    }

    public void ChangeKeyKode(KeyCode v)
    {
        StartCoroutine(Change(v));
    }
    IEnumerator Change(KeyCode i)
    {
        while (true)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    i = key;
                    yield break;
                }
            }
            yield return null;
        }
    }
}
