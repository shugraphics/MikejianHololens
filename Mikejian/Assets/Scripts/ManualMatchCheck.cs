using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualMatchCheck : Singleton<ManualMatchCheck> {

    private void Start()
    {
        UIUtils.SetEachZTestMode(gameObject, GUIZTestMode.Always);

        //Instance.gameObject.SetActive(false);//here use instance to make sure the instance to be valued
    }

    public void OnToggleValueChanged(/*Toggle calledToggle*/)
    {
        /*Toggle[] toggles = GetComponentsInChildren<Toggle>();
        foreach (Toggle t in toggles)//该循环确保每次只开启旋转、平移、缩放中的一个操作
        {
            if (t != calledToggle && calledToggle.isOn)
                t.isOn = false;
        }

        if (calledToggle.name.IndexOf("ManualMatch") != -1 && calledToggle.isOn)
            PositionIndicatorsManager.bManualMatch = true;
        else
            PositionIndicatorsManager.bManualMatch = false;*/

        if (gameObject.GetComponent<Toggle>().isOn)
            PositionIndicatorsManager.bManualMatch = true;
        else
        {
            PositionIndicatorsManager.bManualMatch = false;
        }
    }
}
