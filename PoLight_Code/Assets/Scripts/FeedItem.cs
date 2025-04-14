using UnityEngine;

public class FeedItem : MonoBehaviour
{
    public float recoveryAmount = 20f;

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
        animator.SetTrigger("Play");

        HungerSystem hunger = FindFirstObjectByType<HungerSystem>();
        if (hunger != null)
        {
            hunger.EatFeed(recoveryAmount);
        }

        Destroy(gameObject, 0.4f); 
    }
}
