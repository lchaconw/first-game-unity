using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

public class MenuGame : MonoBehaviour
{
    public AudioMixer audioMixer;
    private int _currentCharacter;
    public GameObject characterHolder;

    private void Awake()
    {
        LoadCharacter();
    }

    private void LoadCharacter()
    {
        _currentCharacter = PlayerPrefs.GetInt("CharacterSelected");
        SelectCharacter(_currentCharacter);
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void ButtonCloseGame()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        Application.Quit();
    }

    public void setVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    private void SelectCharacter(int index)
    {
        for (int i = 0; i < characterHolder.transform.childCount; i++)
        {
            characterHolder.transform.GetChild(i).gameObject.SetActive(i == index);
        }
    }

    public void ChangeCharacter(int change)
    {
        _currentCharacter += change;
        if (_currentCharacter < 0)
            _currentCharacter = 0;
        else if (_currentCharacter >= characterHolder.transform.childCount)
            _currentCharacter = _currentCharacter - 1;

        SelectCharacter(_currentCharacter);
    }

    public void ConfirmCharacter()
    {
        PlayerPrefs.SetInt("CharacterSelected", _currentCharacter);
    }
}
