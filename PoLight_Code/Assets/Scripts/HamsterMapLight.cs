using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HamsterMapLightSetup : MonoBehaviour
{
    public GameObject player; // Inspector에서 플레이어 연결

    void Start()
    {
        // 라이트 오브젝트 생성
        GameObject lightObj = new GameObject("PlayerLight");
        lightObj.transform.SetParent(player.transform);
        lightObj.transform.localPosition = Vector3.zero;

        // Light2D 컴포넌트 추가
        Light2D light2D = lightObj.AddComponent<Light2D>();
        light2D.lightType = Light2D.LightType.Point;
        light2D.intensity = 1.5f;
        light2D.pointLightOuterRadius = 3f;
        light2D.pointLightInnerRadius = 1.5f;
        light2D.color = Color.white;
    }
}
