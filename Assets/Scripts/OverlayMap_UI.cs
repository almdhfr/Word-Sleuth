using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayMap_UI : MonoBehaviour
{
    public CanvasGroup mapsCanvasGroup;
    private List<AreaIcon_UI> areaIcons;

    private void Awake()
    {
        areaIcons = new List<AreaIcon_UI>(mapsCanvasGroup.gameObject.GetComponentsInChildren<AreaIcon_UI>());
        foreach (var areaIcon in areaIcons)
        {
            areaIcon.AreaClicked += OnAreaIconClicked;
        }
    }
    private void OnAreaIconClicked(int areaIndex)
    {
        mapsCanvasGroup.alpha = 0f;
    }
    public void OnClicked()
    {
        if (mapsCanvasGroup.alpha == 1f)
        {
            mapsCanvasGroup.alpha = 0f;
            mapsCanvasGroup.interactable = false;
            mapsCanvasGroup.blocksRaycasts = false;
        }
        else
        {
            mapsCanvasGroup.alpha = 1f;
            mapsCanvasGroup.interactable = true;
            mapsCanvasGroup.blocksRaycasts = true;
        }
    }
}
