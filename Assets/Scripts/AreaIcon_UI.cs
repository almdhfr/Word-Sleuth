using UnityEngine;

public class AreaIcon_UI : MonoBehaviour
{
    public int sceneryIndex;

    public delegate void AreaClickedEventHandler(int index);
    public event AreaClickedEventHandler AreaClicked;

    public void OnClicked()
    {
        GameManager.Instance.GoToAreaByIndex(sceneryIndex);
        AreaClicked?.Invoke(sceneryIndex);
    }
}
