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
    public class SetKeyword : StoryElement
    {
        public override string ElementName => "SetKeyword";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Story();

        public override StoryElementTypes Type => StoryElementTypes.Story;

        public string keyword = string.Empty;

#if UNITY_EDITOR
        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
            keyword = EditorGUILayout.TextField("SetKeyword", keyword);
        }
#endif

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            GameManager.Instance.SetKeyword(keyword);
            managerCallback.AdvanceStoryGraph(GetOutputPort("NextNode").Connection.node as StoryElement);       
            yield return null;
        }
    }
}