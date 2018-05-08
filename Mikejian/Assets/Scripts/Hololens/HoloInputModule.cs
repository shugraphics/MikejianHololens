using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoloInputModule : HoloLensInputModule
{
    public static HoloInputModule _instance;
    public static HoloInputModule Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<HoloInputModule>();
            return _instance;
        }
    }
    public GameObject CurrentRaycastGameObject
    {
        get
        {
            return GetCurrentFocusedGameObject();
        }
    }
}
