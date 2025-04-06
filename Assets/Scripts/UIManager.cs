using UnityEngine;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private CanvasGroup fadeCanvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log(Instance);
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeToBlack());
    }
    
    private System.Collections.IEnumerator FadeToBlack()
    {
        float duration = 1f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(0, 1, t / duration);
            yield return null;
        }

        fadeCanvas.alpha = 1;
    }
}
