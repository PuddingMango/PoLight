using UnityEngine;

public class FeedItem : MonoBehaviour
{
    [Header("사료 설정")]
    public bool isSpoiled = false; 
    public float recoveryAmount = 20f; 
    public float maxReductionAmount = 10f; 

    private Animator animator;
    private bool used = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (used || !collision.CompareTag("Player")) return;
        used = true;

        if (isSpoiled)
            animator.SetTrigger("PlayBad");
        else
            animator.SetTrigger("PlayGood");

        HungerSystem hunger = FindFirstObjectByType<HungerSystem>();
        if (hunger != null)
        {
            if (isSpoiled)
                hunger.EatSpoiledFeed(recoveryAmount, maxReductionAmount);
            else
                hunger.EatFeed(recoveryAmount);
        }

        Destroy(gameObject, 0.4f);
    }
}
