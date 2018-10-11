using UnityEngine;

namespace MalbersAnimations
{
    public class MalbersEditor
    {
        public static GUIStyle StyleGray
        {
            get
            {
                return Style(new Color(0.5f, 0.5f, 0.5f, 0.3f));
            }
        }

        public static GUIStyle StyleBlue
        {
            get
            {
                return Style(new Color(0, 0.5f, 1f, 0.3f));
            }
        }
        public static GUIStyle StyleGreen
        {
            get
            {
                return Style(new Color(0f, 1f, 0.5f, 0.3f));
            }
        }

        public static GUIStyle Style(Color color)
        {
            GUIStyle currentStyle = new GUIStyle(GUI.skin.box);
            currentStyle.border = new RectOffset(-1, -1, -1, -1);

            Color[] pix = new Color[1];
            pix[0] = color;
            Texture2D bg = new Texture2D(1, 1);
            bg.SetPixels(pix);
            bg.Apply();


            currentStyle.normal.background = bg;
            return currentStyle;
        }
    }
}