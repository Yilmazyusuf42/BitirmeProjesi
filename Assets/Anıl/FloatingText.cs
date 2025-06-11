using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float moveSpeed = 50f;
    public float duration = 1f;

    private RectTransform rectTransform;
    private float timer;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Show(string message)
    {
        text.text = message;
        timer = duration;
    }

    private void Update()
    {
        if (timer <= 0f)
        {
            Destroy(gameObject);
            return;
        }

        timer -= Time.deltaTime;
        rectTransform.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime;
    }
}
