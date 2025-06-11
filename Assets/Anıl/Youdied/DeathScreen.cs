using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
public class DeathScreen : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI deathText;
    public Button retryButton;
    public AudioSource audioSource;
    public AudioClip youDiedClip;
    public Image blackBackground;


    public float fadeInDuration = 1.5f;
    public float holdDuration = 1.5f;

    private void Awake()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        retryButton.gameObject.SetActive(false);
        retryButton.onClick.AddListener(RestartScene);
    }

    public void ShowDeathScreen()
    {
        gameObject.SetActive(true); // Enable if initially disabled
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        if (audioSource && youDiedClip)
            audioSource.PlayOneShot(youDiedClip);

        float t = 0;
while (t < fadeInDuration)
{
    t += Time.deltaTime;
    float alpha = Mathf.Lerp(0, 1, t / fadeInDuration);
    canvasGroup.alpha = alpha;

    if (blackBackground != null)
    {
        Color color = blackBackground.color;
        color.a = alpha * 0.6f; // keep it 60% dark
        blackBackground.color = color;
    }

    yield return null;
}


        yield return new WaitForSeconds(holdDuration);

        retryButton.gameObject.SetActive(true);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    
    

private void RestartScene()
{
    Time.timeScale = 1f;

    GameState.isPlayerDead = false; // ✅ Reset global flag

    Destroy(PlayerManager.instance?.gameObject);
    Destroy(SkillManager.instance?.gameObject);

    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}


}
