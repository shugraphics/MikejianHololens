using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorStateCtr : Singleton<CursorStateCtr>
{

    public enum State
    {
        None,
        MoveCurrent
    }
    static public State m_state;
    static public float distance;
    static public Vector3 oldObjPos;
    static public Vector3 oldHitPos;
    static public Vector3 oldObjScale;
    //static public Vector3 oldCenter;
    static private Vector3 offset;
    //static private bool flagshink;

	void Start () {
        m_state = State.None;
        //flagshink = true;
        offset = new Vector3(0, 0, 0);
	}
	
	
	public void MoveNear () {                           //????
        //if (GeneralUIManager.animationBtn.activeSelf == true) GeneralUIManager.animationBtn.SetActive(false);
        if (m_state == State.MoveCurrent && (Interact.SelectedGameObject != GameObject.Find("测试5/MarkObj/Indicator0")) && (Interact.SelectedGameObject != GameObject.Find("测试5/MarkObj/Indicator1")))
        {
            if (distance > 1.2f && Interact.SelectedGameObject.transform.localScale.x > 20)
            {
                Interact.SelectedGameObject.transform.localScale = oldObjScale / 6;
                offset = Interact.SelectedGameObject.GetComponent<MeshRenderer>().bounds.center - oldHitPos;
                Interact.SelectedGameObject.transform.position = oldObjPos + (Camera.main.transform.position + Camera.main.transform.forward * distance / 6 - oldHitPos) - offset;
                distance = distance / 6;
                //flagshink = true;
            }
            //else
            //{
            //    Interact.SelectedGameObject.transform.position = oldObjPos + (Camera.main.transform.position + Camera.main.transform.forward * 1.2f - oldHitPos) - offset;
            //    distance = 1.2f;
            //    //if (distance < 2f) flagshink = false;
            //    //else flagshink = true;
            //}
        }
	}
}
