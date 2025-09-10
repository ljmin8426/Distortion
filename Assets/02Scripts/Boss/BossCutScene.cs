using UnityEngine;
using Unity.Cinemachine;

public class BossCutScene : MonoBehaviour
{
    private CinemachineCamera cinemachineCamera;

    private void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
        if (cinemachineCamera == null)
        {
            Debug.LogWarning("CinemachineCamera 컴포넌트를 찾을 수 없습니다");
            return;
        }
    }

    public void FindBoss()
    {
        GameObject bossObj = GameObject.FindWithTag("Boss");
        if (bossObj != null)
        {
            cinemachineCamera.LookAt = bossObj.transform;
            cinemachineCamera.Follow = bossObj.transform;
        }
        else
        {
            Debug.LogWarning("씬에서 Boss 태그 오브젝트를 찾을 수 없습니다.");
        }
    }
}
