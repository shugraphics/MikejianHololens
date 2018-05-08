using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureAction : MonoBehaviour
{
    [Tooltip("How fast the gameobject will rotate by the value of navigation")]
    public float RotationSensitivity = 2f;

    [Tooltip("How fast the gameobject will scale by the value of manipulation")]
    public float ScaleSensitivity = 2f;

    [Range(0, 1)]
    [Tooltip("How fast the gameobject will move by the value of manipulation")]
    public float ManipulationSensitivity = 0.05f;

    [HideInInspector]
    public bool IsNavigating = false;

    [HideInInspector]
    public bool IsManipulating = false;

    [HideInInspector]
    public bool IsScaleChange = false;

    private void Update()
    {
        if (GestureManager.Instance == null || Interact.SelectedGameObject==null)
            return;

        PerformRotation();
        PerformMove();
        PerformScale();
    }

    private void PerformRotation()
    {
        GameObject _selected = Interact.SelectedGameObject;
        IsNavigating = GestureManager.Instance.IsNavigation;
        if (GestureManager.Instance.IsNavigation)
        {
            float rotationFactor = GestureManager.Instance.NavigationRelativePosition.x * RotationSensitivity;
            Interact.SelectedGameObject.transform.Rotate(0, -1 * rotationFactor, 0);
        }
    }

    private void PerformMove()
    {
        GameObject _selected = Interact.SelectedGameObject;
        IsManipulating = GestureManager.Instance.IsManipulation;
        if (GestureManager.Instance.IsManipulation)
        {
            Vector3 deltaVector = GestureManager.Instance.ManipulationRelativePosition - GestureManager.Instance.ManipulationStartPosition;
            Interact.SelectedGameObject.transform.position += deltaVector * ManipulationSensitivity;
        }
    }

    private void PerformScale()
    {
        GameObject _selected = Interact.SelectedGameObject;
        IsScaleChange = GestureManager.Instance.IsScaleChange;
        if (GestureManager.Instance.IsScaleChange)
        {
            Vector3 originpos = Interact.SelectedGameObject.GetComponent<MeshRenderer>().bounds.center;
            float scaleFactor = GestureManager.Instance.ScaleChangeRelativePosition.x * ScaleSensitivity;
            Interact.SelectedGameObject.transform.localScale = Interact.SelectedGameObject.transform.localScale - new Vector3(1, 1, 1) * scaleFactor;
            Vector3 ScaleOffset = Interact.SelectedGameObject.GetComponent<MeshRenderer>().bounds.center - originpos;
            Interact.SelectedGameObject.transform.position = Interact.SelectedGameObject.transform.position - ScaleOffset;
        }
    }

}

