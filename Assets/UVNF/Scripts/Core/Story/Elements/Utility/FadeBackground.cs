using System.Collections;
using UnityEngine;
using UnityEditor;
using UVNF.Core.UI;
using UVNF.Extensions;
using CoroutineManager;

namespace UVNF.Core.Story.Utility
{
    public class FadeBackground : StoryElement
    {
        public override string ElementName => "FadeBackground";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Utility();

        public override StoryElementTypes Type => StoryElementTypes.Utility;
        
        public bool fadeUnfade;

        public float FadeOutTime = 1f;

        public bool WaitToFinish = false;

#if UNITY_EDITOR
        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
            fadeUnfade = GUILayout.Toggle(fadeUnfade, "Fade Background");
            if (fadeUnfade)
                GUILayout.Label("Canvas Group will fade");
            else
                GUILayout.Label("Canvas Group will unfade.");
            FadeOutTime = EditorGUILayout.Slider(FadeOutTime, 0, 10f);
            WaitToFinish = GUILayout.Toggle(WaitToFinish, "Wait To Finish Before Proceeding");
        }
#endif

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            if (WaitToFinish)
            {
                // var t = TaskManager.CreateTask(managerCallback.Canvas.FadeCanvasGroup(managerCallback.Canvas.BottomCanvasGroup, FadeOutTime));
                // t.Start();
                // return managerCallback.Canvas.StartFade(managerCallback.Canvas.BackgroundCanvasGroup, fadeUnfade, FadeOutTime);
                if(fadeUnfade)
                    return managerCallback.Canvas.FadeCanvasGroup(managerCallback.Canvas.BackgroundCanvasGroup, FadeOutTime);
                else
                    return managerCallback.Canvas.UnfadeCanvasGroup(managerCallback.Canvas.BackgroundCanvasGroup, FadeOutTime);
            }
            else
            {
                // var t = TaskManager.CreateTask(managerCallback.Canvas.UnfadeCanvasGroup(managerCallback.Canvas.BottomCanvasGroup, FadeOutTime));
                // t.Start();
                managerCallback.Canvas.StartFade(managerCallback.Canvas.BackgroundCanvasGroup, fadeUnfade, FadeOutTime);
            }
            return null;
        }
    }
}