using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustUIPanel : Singleton<AdjustUIPanel>
{

    //private enum MoveDirection
    //{
    //    Up,
    //    Down,
    //    Left,
    //    Right,
    //    Forward,
    //    Backward
    //}

    //private IEnumerator _forward;
    //private IEnumerator _back;
    //private IEnumerator _left;
    //private IEnumerator _right;
    //private IEnumerator _up;
    //private IEnumerator _down;

    private IEnumerator currentCoroutine;

    //private Vector3 rightDirection { get { return Camera.main.transform.right; } }
    //private Vector3 upDirection { get { return Vector3.up; } }
    //private Vector3 forwardDirection { get { return Vector3.Cross(rightDirection, upDirection); } }

    private void Start()
    {
        UIUtils.SetEachZTestMode(gameObject, GUIZTestMode.Always);
        //UIIcon[] UIIcons = transform.GetComponentsInChildren<UIIcon>();
        //for (MoveDirection direction = MoveDirection.Up; direction <= MoveDirection.Backward; ++direction)
        //{
        //    int index = (int)direction;
        //    MoveDirection _moveDirection = direction;

        //    UIIcons[index]._OnClick +=(()=> { MoveTargetObj(_moveDirection); });

        //}
        Instance.gameObject.SetActive(false);//here use instance to make sure the instance to be valued
    }

    //private void MoveTargetObj(MoveDirection _moveDirection)
    //{
    //    Interact.SelectedGameObject.transform.Translate(GetMoveDirection(_moveDirection) * 0.1f);
    //}

    //private Vector3 GetMoveDirection(MoveDirection direction)
    //{
    //    switch (direction)
    //    {
    //        case MoveDirection.Up:
    //            return upDirection;
    //        case MoveDirection.Down:
    //            return -upDirection;
    //        case MoveDirection.Left:
    //            return -rightDirection;
    //        case MoveDirection.Right:
    //            return rightDirection;
    //        case MoveDirection.Forward:
    //            return forwardDirection;
    //        case MoveDirection.Backward:
    //            return -forwardDirection;
    //        default:
    //            return new Vector3();
    //    }
    //}

    public void OnToggleValueChanged(Toggle calledToggle)
    {
        Debug.Log("Toggle name is " + calledToggle.name);

        Toggle[] toggles = GetComponentsInChildren<Toggle>();
        foreach (Toggle t in toggles)//该循环确保每次只开启旋转、平移、缩放中的一个操作
        {
            if (t != calledToggle && calledToggle.isOn)
                t.isOn = false;
        }

        if (!calledToggle.isOn)
            GestureManager.Instance.SwitchRecognizer(GestureManager.Instance.TappedRecognizer);//没有执行旋转平移缩放操作时，执行TappedRecognizer操作

        if (calledToggle.name.IndexOf("Manipulation") != -1 && calledToggle.isOn)
            GestureManager.Instance.SwitchRecognizer(GestureManager.Instance.ManipulationRecognizer);
        else if (calledToggle.name.IndexOf("Navigation") != -1 && calledToggle.isOn)
            GestureManager.Instance.SwitchRecognizer(GestureManager.Instance.NavigationRecognizer);
        else if (calledToggle.name.IndexOf("ScaleChange") != -1 && calledToggle.isOn)
            GestureManager.Instance.SwitchRecognizer(GestureManager.Instance.ScaleChangeRecognizer);
    }

}
