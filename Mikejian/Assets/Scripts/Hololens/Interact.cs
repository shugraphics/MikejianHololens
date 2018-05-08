using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle the gaze message
/// </summary>
public class Interact : MonoBehaviour
{
    private static Interact[] InteractibleObject;//实例化一个该类的对象数组，用于存放Hierarchy面板中的所有挂载Interact脚本的模型的,表示所有可交互的物体
    private static GameObject _Parent;

    private static GameObject _selectedGameObject;
    public static GameObject SelectedGameObject//属性，使得外部类访问Interact类时不会直接访问_selectedGameObject变量，而是通过SelectedGameObject该属性访问私有变量
                                               //简单说一下属性和字段的区别：字段就是成员变量，而属性确实提供给外部访问内部成员变量的接口。
                                               //之所以会有属性的出现，就是为了避免外部对类的成员的直接访问，通俗的说就是OOP中的封装思想。
    {
        get {return _selectedGameObject;}
        private set{_selectedGameObject = value;}
    }
    public Vector3 SelectedPosition;//用于保存凝视所选中物体的原有位置信息
    public Quaternion SelectedRotation;//用于保存凝视所选中物体的原有旋转信息
    public Vector3 SelectedScale;//用于保存凝视所选中物体的原有尺寸信息


    private void InitInteractibleObject()
    {
        if (InteractibleObject != null)//当InteractibleObject数组不为空时，返回
            return;
        //Find all brother
        Transform _parent = transform.parent;
        InteractibleObject = _parent.GetComponentsInChildren<Interact>();
    }

    private void Start()//调用InteractibleObject()函数
    {
        if (_Parent == null)
            _Parent = transform.parent.gameObject;//找到检视面板中的“Group001”对象

        InitInteractibleObject();//调用函数
    }

    //只要选中物体不取消选中，就能reset到选中时物体所处的位置。
    void ResetPosition()
    {
        //CursorStateCtr.m_state = CursorStateCtr.State.None;
        _selectedGameObject.transform.position = SelectedPosition;
        _selectedGameObject.transform.rotation = SelectedRotation;
        _selectedGameObject.transform.localScale = SelectedScale;
        
        //CancelProcess(_selectedGameObject);
    }


    void OnSelect()//       用于处理InteractManager的“OnSelect”回调
    {
        bool _cancelSelected = SelectedGameObject == gameObject;//判断新一次选中的物体（gameObject）与上一次选中的物体(SelectedGameObject)是否相同，如果相同则表示“取消选中”的操作

        if (_cancelSelected)//如果取消选中
        {
            Color materialColor = _selectedGameObject.GetComponent<MeshRenderer>().material.color;
            materialColor.a = 0.8f;
            _selectedGameObject.GetComponent<MeshRenderer>().material.color = materialColor;//上面三行用于使物体被取消选中后透明度发生变化

            CancelProcess(_selectedGameObject);
            CursorStateCtr.m_state = CursorStateCtr.State.None;
        }
        else
            SelectProcess();//**??
    }


    private void SelectProcess()
    {
        GazeManager _gazeManager = GazeManager.Instance;
        if (_gazeManager != null && _gazeManager.Hit)
        {
            CursorStateCtr.m_state = CursorStateCtr.State.MoveCurrent;//双击选中物体的同时，更改m_state状态为MoveCurrent，这样为下一次的单击该物体的事件做准备
            RaycastHit _hitInfo = _gazeManager.HitInfo;
            if (_hitInfo.collider != null)
            {
                CursorStateCtr.distance = _hitInfo.distance;
                CursorStateCtr.oldHitPos = _hitInfo.point;
                CursorStateCtr.oldObjPos = gameObject.transform.position;
                CursorStateCtr.oldObjScale = gameObject.transform.localScale;
                //CursorStateCtr.oldCenter = gameObject.GetComponent<MeshRenderer>().bounds.center;
            }
        }
                
        SelectedGameObject = gameObject;//SelectedGameObject每次双击选中不同模型时，变量存储新的选中模型
        AdjustUIPanel.Instance.gameObject.SetActive(true); //??

        for (int index = 0; index != InteractibleObject.Length; ++index)
        {
            if(InteractibleObject[index].gameObject!=gameObject)
                CancelProcess(InteractibleObject[index].gameObject);//???
        }
        //the GUI of Panel actually don't need switch recognizer,so this can use for rotation
        Color materialColor = _selectedGameObject.GetComponent<MeshRenderer>().material.color;
        //if (materialColor.a > 0.6)
        //    materialColor.a = 0.5f;
        //else
        //    materialColor.a = 1f;
        materialColor.a = 0.5f;
        _selectedGameObject.GetComponent<MeshRenderer>().material.color = materialColor;

        SelectedPosition = _selectedGameObject.transform.position;
        SelectedRotation = _selectedGameObject.transform.rotation;
        SelectedScale = _selectedGameObject.transform.localScale;

        //CursorStateCtr.Instance.MoveNear();
    }

    private void CancelProcess(GameObject toBeCanceled)
    {
        if (toBeCanceled == null)
            return;
        Debug.Log("Enter Cancel Process  "+toBeCanceled.name);
        if (toBeCanceled == _selectedGameObject)
        {
            ResetPosition();

            _selectedGameObject = null;
            AdjustUIPanel.Instance.gameObject.SetActive(false);
        }

        /*Color materialColor = toBeCanceled.GetComponent<MeshRenderer>().material.color;
        materialColor.a = 0.5f;
        toBeCanceled.GetComponent<MeshRenderer>().material.color = materialColor;*/
    }
}
