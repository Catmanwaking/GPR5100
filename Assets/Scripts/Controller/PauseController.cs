using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    private void Pause()
    {
        playerInput.SwitchCurrentActionMap("UI");
    }

    private void Unpause()
    {
        playerInput.SwitchCurrentActionMap("Player");
    }

    private void SetPaused() 
    {
        if (StaticData.isPaused)
            Pause();
        else
            Unpause();
    }

    private void OnPause()
    {
        StaticData.isPaused = !StaticData.isPaused;
        StaticData.OnPauseChange?.Invoke();
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
