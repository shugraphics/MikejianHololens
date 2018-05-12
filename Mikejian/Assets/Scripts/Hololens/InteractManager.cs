using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManager : Singleton<InteractManager>
{
    public GameObject FocusGameObject{get;private set;}

    private GameObject OldFocusGameObject=null;

    private void Start()
    {
        FocusGameObject = null;
        //GestureManager.Instance.OnSingleClick += SendResetMessage;
        GestureManager.Instance.OnDoubleClick += SendSelectMessage;//??当双击时才会调用SendSelectMessage()函数，发送“onselect”信息
        GestureManager.Instance.OnSingleClick += CursorStateCtr.Instance.MoveNear;//??

        // by_xqzhu
        GestureManager.Instance.OnDoubleClick += PositionIndicatorsManager.Instance.ClickIndicator;
    }
    private void Update()
    {
        OldFocusGameObject = FocusGameObject;
        GazeManager _gazeManager=GazeManager.Instance;

        if (_gazeManager == null)
            return;

        FocusGameObject = null;
        if (_gazeManager.Hit)
        {
            RaycastHit _hitInfo = _gazeManager.HitInfo;
            if (_hitInfo.collider != null)
                FocusGameObject = _hitInfo.collider.gameObject;
        }

        if (FocusGameObject != OldFocusGameObject)
        {
            SendFocusStateMessage(FocusGameObject, true);
            SendFocusStateMessage(OldFocusGameObject, false);
        }
    }

    private void SendFocusStateMessage(GameObject GameobjectToBeHandled,bool Focus)
    {
        if (GameobjectToBeHandled != null)
        {
            if (GameobjectToBeHandled.GetComponent<Interact>()!= null)
            {
                if (Focus)
                    GameobjectToBeHandled.SendMessage("GazeEnter");
                else
                    GameobjectToBeHandled.SendMessage("GazeExit");
            }
        }
    }

    private void SendSelectMessage()
    {
        if (FocusGameObject == null)
            return;
        FocusGameObject.SendMessage("OnSelect");
    }

    //private void SendResetMessage()
    //{
    //    if (FocusGameObject == null)
    //        return;
    //    FocusGameObject.SendMessage("ResetPosition");
    //}

    private void OnDestroy()
    {
        //GestureManager.Instance.OnSingleClick -= SendResetMessage;
        GestureManager.Instance.OnDoubleClick -= SendSelectMessage;
        GestureManager.Instance.OnSingleClick -= CursorStateCtr.Instance.MoveNear;

        // by_xqzhu
        GestureManager.Instance.OnDoubleClick -= PositionIndicatorsManager.Instance.ClickIndicator;
    }
}
