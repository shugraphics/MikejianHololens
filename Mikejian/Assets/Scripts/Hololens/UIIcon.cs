using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIIcon : Graphic,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler,IPointerUpHandler,IPointerDownHandler
{
    public delegate void OnClick();
    public event OnClick _OnClick;
    public delegate void OnEnter();
    public event OnEnter _OnEnter;
    public delegate void OnExit();
    public event OnEnter _OnExit;
    public delegate void OnDown();
    public event OnDown _OnDown;
    public delegate void OnUp();
    public event OnUp _OnUp;

    public Color FocusColor = new Color(1, 0, 0, 0.33f);
    public Color NormalColor = new Color(1, 1, 1, 0.33f);

    public void OnPointerClick(PointerEventData eventData)//鼠标点击
    {
        if(_OnClick!=null)
            _OnClick();
    }

    public void OnPointerEnter(PointerEventData eventData)//鼠标进入
    {
        color = FocusColor;
        if(_OnEnter!=null)
            _OnEnter();
    }

    public void OnPointerExit(PointerEventData eventData)//鼠标退出
    {
        color = NormalColor;
        if(_OnExit!=null)
            _OnExit();
    }

    public void OnPointerUp(PointerEventData eventData)//鼠标抬起
    {
        if(_OnUp!=null)
            _OnUp();
    }

    public void OnPointerDown(PointerEventData eventData)//鼠标按下
    {
        if(_OnDown!=null)
            _OnDown();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        color = NormalColor;
    }
}
