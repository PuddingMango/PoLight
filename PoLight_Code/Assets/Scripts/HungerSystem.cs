using UnityEngine;
using UnityEngine.UI;

public class HungerSystem : MonoBehaviour
{
    [Header("UI")]
    public Image[] hungerIcons;
    public Sprite normalIcon;   
    public Sprite boneIcon;     
    public Sprite spoiledIcon;  

    [Header("Hunger Settings")]
    public float maxHunger = 100f;
    public float currentHunger;
    public float hungerDecayRate = 20f; // 허기 감소 시간 

    private bool ateSpoiledFeed = false;

    void Start()
    {
        currentHunger = maxHunger;
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
                hungerIcons[i].sprite = ateSpoiledFeed ? spoiledIcon : normalIcon;
            else
                hungerIcons[i].sprite = boneIcon;
        }
    }

    public void EatFeed(float amount)
    {
        ateSpoiledFeed = false;
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);
    }

    public void EatSpoiledFeed(float amount, float maxReduction)
    {
        ateSpoiledFeed = true;
        maxHunger -= maxReduction;
        maxHunger = Mathf.Clamp(maxHunger, 20f, 100f);
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);
    }
}

