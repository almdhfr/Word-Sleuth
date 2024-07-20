using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Icons_Manager : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
}
