using System.Collections;
using UnityEngine;
using UnityEditor;
using UVNF.Core.UI;
using UVNF.Extensions;
using CoroutineManager;

namespace UVNF.Core.Story.Utility
{
    public class FadeBottomCanvas : StoryElement
    {
        public override string ElementName => "FadeBottomCanvas";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Utility();

        public override StoryElementTypes Type => StoryElementTypes.Utility;
        
        public bool fadeUnfade;

        public float FadeOutTime = 1f;

        public bool WaitToFinish = false;

#if UNITY_EDITOR
        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
            fadeUnfade = GUILayout.Toggle(fadeUnfade, "Fade/Unfade Bottom Canvas");
            if (fadeUnfade)
                GUILayout.Label("Bottom Canvas will fade (invisible)");
            else
                GUILayout.Label("Bottom Canvas unfade (visible)");
            FadeOutTime = EditorGUILayout.Slider(FadeOutTime, 0, 10f);
            WaitToFinish = GUILayout.Toggle(WaitToFinish, "Wait To Finish Before Proceeding");
        }
#endif

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            if (WaitToFinish)
            {
                if(fadeUnfade)
                    return managerCallback.Canvas.FadeCanvasGroup(managerCallback.Canvas.BottomCanvasGroup, FadeOutTime);
                else
                    return managerCallback.Canvas.UnfadeCanvasGroup(managerCallback.Canvas.BottomCanvasGroup, FadeOutTime);
            }
            else
            {
                managerCallback.Canvas.StartFade(managerCallback.Canvas.BottomCanvasGroup, fadeUnfade, FadeOutTime);
            }
            return null;
        }
    }
}