using UnityEngine;
using UnityEngine.UI;

public class HungerSystem : MonoBehaviour
{
    public Image[] hungerIcons;
    public Sprite normalIcon;
    public Sprite boneIcon;
    public Sprite spoiledIcon;
    public Sprite spoiledBoneIcon;


    public float maxHunger = 100f;
    public float currentHunger;
    public float hungerDecayRate = 20f;

    private bool[] isSpoiledSlot;

    void Start()
    {
        currentHunger = maxHunger;
        isSpoiledSlot = new bool[hungerIcons.Length];
    }

    void Update()
    {
        currentHunger -= hungerDecayRate * Time.deltaTime;
        currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);
        UpdateHungerUI();
    }

    void UpdateHungerUI()
    {
        int count = Mathf.CeilToInt((currentHunger / maxHunger) * hungerIcons.Length);

        for (int i = 0; i < hungerIcons.Length; i++)
        {
            if (i < count)
            {
                hungerIcons[i].sprite = isSpoiledSlot[i] ? spoiledIcon : normalIcon;
            }
            else
            {
            // ✅ 빈 칸인데 이전에 상한 고기였으면 → 상한 뼈다귀로!
                hungerIcons[i].sprite = isSpoiledSlot[i] ? spoiledBoneIcon : boneIcon;
            }
        }
    }


    public void EatFeed(float amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);

        int count = Mathf.CeilToInt((currentHunger / maxHunger) * hungerIcons.Length);
        for (int i = 0; i < count; i++)
            isSpoiledSlot[i] = false;
    }

    public void EatSpoiledFeed(float amount, float maxReduction)
    {
        maxHunger -= maxReduction;
        maxHunger = Mathf.Clamp(maxHunger, 20f, 100f);

        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);

        int count = Mathf.CeilToInt((currentHunger / maxHunger) * hungerIcons.Length);
        for (int i = 0; i < hungerIcons.Length; i++)
        {
            if (i < count && !isSpoiledSlot[i])
            {
                isSpoiledSlot[i] = true;
                break;
            }
        }
    }
}

