using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class LastClickedUIElementTracker : MonoBehaviour
{
    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private InputActionReference clickActionReference; // Reference to the Input Action asset

    private GameObject lastClickedUIElement;

    private void OnEnable()
    {
        // Subscribe to the performed event of the click action
        clickActionReference.action.performed += OnClickPerformed;
        clickActionReference.action.Enable();
    }

    private void OnDisable()
    {
        // Unsubscribe from the click action event
        clickActionReference.action.performed -= OnClickPerformed;
        clickActionReference.action.Disable();
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
    
        // Create a PointerEventData object
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };
    
        // Perform the raycast
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);
    
        // Check the results
        foreach (var result in results)
        {
            if (result.gameObject != null)
            {
                // Update the last clicked UI element
                lastClickedUIElement = result.gameObject;
                Debug.Log($"Last clicked UI element: {lastClickedUIElement.name}");
                break; // Assuming you only care about the first (topmost) result
            }
        }
    }

    // Method to get the last clicked UI element from other scripts
    public GameObject GetLastClickedUIElement()
    {
        return lastClickedUIElement;
    }
}
