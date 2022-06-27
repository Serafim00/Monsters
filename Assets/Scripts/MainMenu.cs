using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;

    public System.Action StartGame;

    public void PlayBtn()
    {
        menuPanel.SetActive(false);
        StartGame?.Invoke();
    }

    public void SettingsBtn()
    {

    }

    public void ExitBtn()
    {
        Application.Quit();
    }

    public void RecordsBtn()
    {

    }

    public void CreditsBtn()
    {

    }

    public void OPenMenuBtn()
    {
        menuPanel.SetActive(true);
    }
}
