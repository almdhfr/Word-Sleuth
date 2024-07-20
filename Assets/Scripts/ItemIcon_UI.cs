using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UVNF.Entities.Containers;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(UI_Hover))]
public class ItemIcon_UI : MonoBehaviour
{
    private Button button;

    // public bool collect = false;
    public bool disappear = false;
    // public Item item;

    public StoryGraph storyGraph;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnClicked()
    {
        StartStoryGraph();
    }

    private void StartStoryGraph()
    {
        button.enabled = false;
        GameManager.Instance.SetActiveInput(false);

        if (disappear)
            GameManager.Instance.UVNFManager.StoryFinished += OnStoryFinished;

        GameManager.Instance.UVNFManager.StartStory(storyGraph);
        GameManager.Instance.SetActiveInput(true);
        button.enabled = true;
    }
    private void OnStoryFinished(string storyName)
    {
        gameObject.SetActive(false);
        GameManager.Instance.UVNFManager.StoryFinished -= OnStoryFinished;
    }

}