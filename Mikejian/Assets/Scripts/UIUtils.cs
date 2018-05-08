using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GUIZTestMode
{
    Less = 0,
    Greater,
    LEqual,
    GEqual,
    Equal,
    NotEqual,
    Always
}
public class UIUtils
{
    public static void SetEachZTestMode(GameObject window, GUIZTestMode mode)
    {
        Graphic[] graphs = window.GetComponentsInChildren<Graphic>(true);
        foreach (var g in graphs)
        {
            g.materialForRendering.SetInt("unity_GUIZTestMode", (int)mode);
        }
    }
}
