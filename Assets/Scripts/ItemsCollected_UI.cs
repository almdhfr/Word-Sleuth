using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemsCollectedUI : MonoBehaviour
{
    public TMP_Text itemsCollectedText; // Assign this in the inspector with your UI Text component

    private void Start()
    {
        itemsCollectedText.text = "Evidence: " + GameManager.Instance.items.Count + "/5";
    }
    private void OnEnable ()
    {
        GameManager.Instance.OnItemChangeEvent += OnItemChange;
    }
    private void OnDisable ()
    {
        GameManager.Instance.OnItemChangeEvent -= OnItemChange;
    }
    private void OnItemChange(bool add)
    {
        itemsCollectedText.text = "Evidence: " + GameManager.Instance.items.Count + "/5";
    }
}