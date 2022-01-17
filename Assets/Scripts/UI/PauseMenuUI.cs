using UnityEngine;

public class PauseMenuUI : MenuUI
{
    public void SetPaused()
    {
        if (StaticData.isPaused)
            OpenMenu();
        else
            CloseMenu();
    }

    public void Resume()
    {
        StaticData.isPaused = false;
        StaticData.OnPauseChange?.Invoke();
    }

    private void CloseMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        OptionsGO.SetActive(false);
        MainMenuGO.SetActive(false);
    }

    private void OpenMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        MainMenuGO.SetActive(true);
    }

    public override void ExitGame()
    {
        //return to menu
    }

    private void OnEnable()
    {
        StaticData.OnPauseChange += SetPaused;
    }

    private void OnDisable()
    {
        StaticData.OnPauseChange -= SetPaused;

    }
}
