using UnityEngine;

public class DontDestroyUI : MonoBehaviour
{
    private void Awake()
    {
        if (FindObjectsByType<DontDestroyUI>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}