using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FadeOnTrigger : MonoBehaviour
{
    [Tooltip("Target opacity (alpha) when player is inside the trigger area.")]
    [Range(0f, 1f)]
    public float targetOpacity = 0.5f;

    private float originalOpacity;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        originalOpacity = originalColor.a;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetOpacity(targetOpacity);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetOpacity(originalOpacity);
        }
    }

    void SetOpacity(float alpha)
    {
        Color newColor = spriteRenderer.color;
        newColor.a = alpha;
        spriteRenderer.color = newColor;
    }
}
