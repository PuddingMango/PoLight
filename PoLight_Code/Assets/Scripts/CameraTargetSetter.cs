using UnityEngine;
using Unity.Cinemachine;

public class CameraTargetSetter : MonoBehaviour
{
    void Start()
    {
        // 플레이어 태그를 가진 오브젝트 탐색
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // 이 스크립트를 가진 가상 카메라 가져오기
            CinemachineCamera virtualCam = GetComponent<CinemachineCamera>();
            if (virtualCam != null)
            {
                virtualCam.Follow = player.transform;
            }
        }
    }
}

