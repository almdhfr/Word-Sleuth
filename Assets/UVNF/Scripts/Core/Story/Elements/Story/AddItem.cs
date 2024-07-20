using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using UVNF.Core.UI;
using UVNF.Entities.Containers.Variables;
using UVNF.Extensions;
using System.Linq;

namespace UVNF.Core.Story.Dialogue
{
    public class AddItem : StoryElement
    {
        public override string ElementName => "AddItem";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Story();

        public override StoryElementTypes Type => StoryElementTypes.Story;

        public string itemName;

#if UNITY_EDITOR
        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
            itemName = EditorGUILayout.TextField("Item Name", itemName);
            // item = EditorGUILayout.ObjectField("Item", item, typeof(Item), false) as Item;
        }
#endif
        public override object GetValue(NodePort port)
        {
            if (port.IsConnected)
                return port.Connection.node;
            return null;
        }
        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            
            GameManager.Instance.AddNewItem(itemName);
            return null;
        }

        // public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        // {
        //     managerCallback.AdvanceStoryGraph(GetOutputPort("NextNode").Connection.node as StoryElement);
        //     return null;
        // }
    }
}