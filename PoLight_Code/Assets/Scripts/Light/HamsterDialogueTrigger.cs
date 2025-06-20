using UnityEngine;

public class HamsterDialogueTrigger : MonoBehaviour
{
    private bool triggered = false;

    public void OnDialogueStart()
    {
        if (triggered) return;

        HamsterMapLight lightController = Object.FindFirstObjectByType<HamsterMapLight>();
        if (lightController != null)
        {
            lightController.FadeInGlobalLight();
            triggered = true;
        }
    }
}
