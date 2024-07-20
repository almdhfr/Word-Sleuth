using System.Collections;
using UnityEngine;
using UnityEditor;
using UVNF.Core;
using UVNF.Core.Story;
using System;
using System.Reflection;
using UVNF.Entities.Containers;
using UVNF.Entities.Containers.Variables;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

using UVNF.Core.Story.Scenery;
using UnityEngine.UI;
using UVNF.Core.Story.Other;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;


public class GameManager : Singleton<GameManager>
{
    public UVNFManager UVNFManager;

    [SerializeField] private Scene currentScene;

    [SerializeField] private InputActionAsset inputAction;
    public float changeSceneFadeTime = 1f;

    public List<Scene> scenes = new();
    public Button AreaGatewayObjectButton;
    public CanvasGroup InteractableUIOverlay;
    public CanvasGroup overlayMapCanvasGroup;
    public CanvasGroup LeaveAreaCanvasGroup;
    public CanvasGroup HealthPointsCanvasGroup;
    public TMP_Text HealthPointsText;
    private int healthPoints = 0;

    public Image mainBackground;
    public List<Item> items = new();
    public int maxItemCount = 5;
    public delegate void OnItemChange(bool add);
    public event OnItemChange OnItemChangeEvent;
    public StoryGraph trialPhaseStoryGraph_Start;
    public List<StoryGraph> TrialLoopStoryGraphs;
    public string lastTypedKeyword = string.Empty;

    public MatchTextByTyping matchTextByTyping;

    public List<string> variableStrings = new();

    public StoryGraph investigationFinishedStoryGraph;
    private void Start()
    {
        UVNFManager = GetComponent<UVNFManager>();
        UVNFManager.StoryFinished += OnStoryFinished;
        UVNFManager.StoryStarted += OnStoryStarted;

        if (currentScene)
        {
            UVNFManager.Canvas.ChangeBackground(currentScene.backgroundImage);

            foreach (var scene in scenes)
            {
                if (scene.hasIcons() && scene.iconsManager != null)
                {
                    if (scene.iconsManager.canvasGroup != null)
                    {
                        scene.iconsManager.canvasGroup.alpha = 0;
                        scene.iconsManager.gameObject.SetActive(false);
                    }

                }
            }
            if (currentScene.hasIcons() && currentScene.iconsManager != null)
            {
                currentScene.iconsManager.gameObject.SetActive(true);
                currentScene.iconsManager.canvasGroup.alpha = 1;
            }
        }
    }

    private void OnStoryStarted(string storyName)
    {
        if (storyName == "DetectiveStory")
        {
            InteractableUIOverlay.alpha = 0;
        }
        if (storyName == "TrialPhase_Start")
        {
            LeaveAreaCanvasGroup.alpha = 0f;
            overlayMapCanvasGroup.alpha = 0f;
            HealthPointsCanvasGroup.alpha = 1f;
            matchTextByTyping.ui_text.alpha = 1f;
        }
    }

    public void SetKeyword(string keyword)
    {
        matchTextByTyping.text = "";
        matchTextByTyping.m_Text = keyword;
    }
    private void CheckItems()
    {
        // Debug.Log("Checking items");
        if (items.Count >= maxItemCount)
        {
            UVNFManager.StartStory(investigationFinishedStoryGraph);
            // StartCoroutine(UVNFManager.Canvas.DisplayText("The evidence I gathered should be enough to bring light into this mystery. Let's go back to the library and talk to everyone."));
        }
    }
    public void AddItem(Item item)
    {
        items.Add(item);
        item.gameObject.transform.SetParent(gameObject.transform);
        OnItemChangeEvent?.Invoke(true);
        CheckItems();
    }

    // this is called from a story graph
    internal void AddNewItem(string itemName)
    {
        if (itemName == "_FadeIcons")
        {
            currentScene.iconsManager.gameObject.SetActive(false);
            return;
        }


        if (items.Exists(item => item.ItemName == itemName))
            return;

        GameObject newitem = new(itemName);
        Item itemComponent = newitem.AddComponent<Item>();
        itemComponent.ItemName = itemName;
        newitem.transform.parent = gameObject.transform;
        AddItem(itemComponent);
    }
    // public void RemoveItem(Item item)
    // {
    //     items.Remove(item);
    //     OnItemChangeEvent?.Invoke(false);
    // }

    private void OnStoryFinished(string storyName)
    {
        // Debug.Log("Story: " + storyName + " has finished");

        if (storyName == "DetectiveStory")
        {
            InteractableUIOverlay.alpha = 1;
            matchTextByTyping.ui_text.alpha = 0f;
            HealthPointsCanvasGroup.alpha = 0f;

            currentScene = scenes.Find(scene => scene.scenery == Scenery.Library);
            StartCoroutine(GoToScene((int)currentScene.scenery));
            UVNFManager.Canvas.BottomCanvasGroup.alpha = 1f;
        }
        if (storyName == "TrialPhase_Start")
        {
            QueueTrialLoop();
        }
        if (storyName == "TrialPhase_Viktoria")
        {
            if (variableStrings.Exists(variable => variable.ToLower() == "caseViktoriaClosed".ToLower()))
            {
                GameOver();
            }
            else
            {
                QueueTrialLoop();
            }
        }
    }

    private void GameOver()
    {
        UVNFManager.Canvas.FadeCanvasGroup(UVNFManager.Canvas.BackgroundCanvasGroup, 1);

        // reset
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

        // items = new();
        // variableStrings = new();
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void QueueTrialLoop()
    {
        // Debug.Log("Queueing trial loop");
        foreach (var story in TrialLoopStoryGraphs)
        {
            if (((StartElement)story.GetRootStory()[0]).StoryName == "TrialPhase_Luca")
                if (variableStrings.Exists(variable => variable.ToLower() == "caseLucaClosed".ToLower()))
                    continue;

            if (((StartElement)story.GetRootStory()[0]).StoryName == "TrialPhase_James")
                if (variableStrings.Exists(variable => variable.ToLower() == "caseJamesClosed".ToLower()))
                    continue;

            if (((StartElement)story.GetRootStory()[0]).StoryName == "TrialPhase_drEmily")
                if (variableStrings.Exists(variable => variable.ToLower() == "caseDrEmilyClosed".ToLower()))
                    continue;

            UVNFManager.StartStory(story);
        }
    }
    void OnDestroy()
    {
        if (UVNFManager != null && UVNFManager != null)
        {
            UVNFManager.StoryFinished -= OnStoryFinished;
        }
    }

    public bool HasItem(string itemName)
    {
        return items.Exists(item => item.ItemName == itemName);
    }

    public void SetActiveInput(bool active)
    {
        if (active)
        {
            inputAction.Enable();
        }
        else
        {
            inputAction.Disable();
        }
    }
    private IEnumerator GoToScene(int sceneIndex)
    {
        inputAction.Disable();
        yield return StartCoroutine(UVNFManager.Canvas.UnfadeCanvasGroup(UVNFManager.Canvas.LoadingCanvasGroup, changeSceneFadeTime));
        if (currentScene.hasIcons() && currentScene.iconsManager != null)
            currentScene.iconsManager.gameObject.SetActive(false);

        Scenery ts = (Scenery)sceneIndex;
        var nextScene = scenes.Find(scene => scene.scenery == ts);

        UVNFManager.Canvas.ChangeBackground(nextScene.backgroundImage);

        if (currentScene.hasIcons() && currentScene.iconsManager != null)
        {
            if (currentScene.iconsManager.canvasGroup != null)
                currentScene.iconsManager.canvasGroup.alpha = 0;
        }

        if (nextScene.hasIcons() && nextScene.iconsManager != null)
        {
            if (nextScene.iconsManager.canvasGroup != null)
                nextScene.iconsManager.canvasGroup.alpha = 1;
        }

        LeaveAreaCanvasGroup.alpha = ts == Scenery.EntranceHall ? 0 : 1;
        overlayMapCanvasGroup.alpha = ts == Scenery.EntranceHall ? 0 : 1;
        overlayMapCanvasGroup.interactable = ts != Scenery.EntranceHall;

        if (nextScene.hasIcons() && nextScene.iconsManager != null)
            nextScene.iconsManager.gameObject.SetActive(true);

        yield return StartCoroutine(UVNFManager.Canvas.FadeCanvasGroup(UVNFManager.Canvas.LoadingCanvasGroup, changeSceneFadeTime));
        inputAction.Enable();

        currentScene = nextScene;

        if (ts == Scenery.Library)
        {
            CheckTrialPhase();
        }
    }

    private void CheckTrialPhase()
    {
        if (items.Count >= maxItemCount)
        {
            UVNFManager.StartStory(trialPhaseStoryGraph_Start);

            overlayMapCanvasGroup.alpha = 0;
            LeaveAreaCanvasGroup.alpha = 0;
            InteractableUIOverlay.alpha = 1;
            HealthPointsCanvasGroup.alpha = 1;
            matchTextByTyping.ui_text.alpha = 1;
        }
    }

    public void GoToAreaByIndex(int sceneIndex)
    {
        StartCoroutine(GoToScene(sceneIndex));
    }

    internal bool IsLastTypedKeyword(string keyword)
    {
        return lastTypedKeyword == keyword;
    }

    internal void AddVariable(string variableName)
    {
        healthPoints++;
        HealthPointsText.text = "Health Points: " + healthPoints;
        variableStrings.Add(variableName);
    }
    internal void RemoveVariable(string variableName)
    {
        if (variableStrings.Exists(variable => variable.ToLower() == variableName.ToLower()))
        {
            healthPoints--;
            HealthPointsText.text = "Health Points: " + healthPoints;
            variableStrings.Remove(variableName);
        }
    }
}
