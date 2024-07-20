using System;
using System.Drawing;
using UnityEngine;

public class Scene : MonoBehaviour{
    public Scenery scenery;
    public Sprite backgroundImage;
    public Icons_Manager iconsManager;

    internal bool hasIcons()
    {
        return iconsManager != null;
    }
}
