using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerSpawner spawner;
    [SerializeField] private GameObject demoCameraGO;
    [SerializeField] private GameObject hudGO;

    private GameObject localPlayerGO;

    private void Start()
    {
        localPlayerGO = spawner.SpawnPlayerOnServer();
        localPlayerGO.SetActive(false);
        StartRound();
    }

    public void StartRound()
    {
        hudGO.SetActive(true);
        localPlayerGO.SetActive(true);
        demoCameraGO.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
