using Cinemachine;
using UnityEngine;
using GDG;

public class CameraController : MonoBehaviour
{
    CinemachineVirtualCamera cv;
    private void Start()
    {
        cv = GetComponentInChildren<CinemachineVirtualCamera>();
        EventManager.Instance.AddListener<GameOverEvent>(OnGameOver);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener<GameOverEvent>(OnGameOver);
    }

    private void OnGameOver(GameOverEvent e)
    {
        cv.enabled = false;
    }

}
