using UnityEngine;

public class MenuUI : MonoBehaviour
{
    [SerializeField] protected GameObject MainMenuGO;
    [SerializeField] protected GameObject OptionsGO;

    public void ToggleOptionsMenu()
    {
        bool state = MainMenuGO.activeInHierarchy;
        MainMenuGO.SetActive(!state);
        OptionsGO.SetActive(state);
    }

    public virtual void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
