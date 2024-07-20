using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UVNF.Entities.Containers;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class CharacterIcon_UI : MonoBehaviour
{
    // static public bool isEnabled = true;
    // public delegate void ClickAction();
    // public event ClickAction OnClick;
    public bool disableAfterFinish = false;
    public string itemDependency;

    private Button button;
    // private Image image; 
    private void Awake()
    {
        button = GetComponent<Button>();
        // image = GetComponent<Image>();
    }

    public StoryGraph storyGraph;

    // internal void SetVisibility(bool visible)
    // {
    //     image.enabled = visible;
    //     button.enabled = visible;
    // }
    public void OnClicked()
    {
        if (storyGraph != null)
        {
            GameManager.Instance.UVNFManager.Canvas.BottomCanvasGroup.alpha = 1;
            GameManager.Instance.AreaGatewayObjectButton.enabled = false;
            button.enabled = false;
            GameManager.Instance.UVNFManager.StoryFinished += OnStoryFinished;
            GameManager.Instance.UVNFManager.StartStory(storyGraph);
        }
        // OnClick?.Invoke();
    }

    public void OnStoryFinished(string storyName)
    {
        button.enabled = true;
        GameManager.Instance.AreaGatewayObjectButton.enabled = true;
        GameManager.Instance.UVNFManager.StoryFinished -= OnStoryFinished;

        if (disableAfterFinish && GameManager.Instance.HasItem(itemDependency))
        {
            gameObject.SetActive(false);
        }
    }
}
