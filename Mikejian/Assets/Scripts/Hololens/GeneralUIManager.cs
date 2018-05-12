using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralUIManager : Singleton<GeneralUIManager>
{
    // by_xqzhu
    public GameObject ModelMesh;//测试5

    public GameObject infoBtn;
    //public GameObject Btn1;
    //public GameObject Btn2;
    //public GameObject Btn3;
    //public GameObject Btn4;
    //public GameObject Btn5;
    //public GameObject Btn6;
    //public GameObject Btn7;
    //public GameObject Btn8;
    //public GameObject Btn9;
    //public GameObject Btn10;
    private GameObject animationBtn;
    private GameObject anime;

    private void Start()
    {

       /* bool flag1 = true; //flag1==true表示动画在播放
        bool flag2 = true;
        bool flag3 = true;
        bool flag4 = true;
        bool flag5 = true;
        */
        /*Animation animation = GameObject.Find("测试5").GetComponent<Animation>();//animation是动画变量
        Text Btn5Txt = GameObject.Find("UIGeneral/HDPE重力排水管/Text").transform.GetComponent<Text>();
        Text Btn6Txt = GameObject.Find("UIGeneral/成排水管/Text").transform.GetComponent<Text>();
        Text Btn7Txt = GameObject.Find("UIGeneral/成排桥架/Text").transform.GetComponent<Text>();
        Text Btn8Txt = GameObject.Find("UIGeneral/风管/Text").transform.GetComponent<Text>();
        Text Btn9Txt = GameObject.Find("UIGeneral/小口径支管/Text").transform.GetComponent<Text>();
        Text animationInfo = GameObject.Find("AnimationInfo/InfoDisplay/Text").transform.GetComponent<Text>();animationInfo变量存储的是每段动画所要现实的对应信息//以上6句获取btn的Text
        
        GameObject anime = GameObject.Find("AnimationInfo");//??
        GameObject animationBtn = GameObject.Find("AnimationInfo/InfoDisplay");//??animationBtn变量存储的是InfoDisplay UI显示框
        animationBtn.SetActive(false);//??
        Vector3 boxsize = anime.GetComponent<BoxCollider>().size;//??获取AnimationInfo对象的BoxCollider.size
        */
        GestureManager.Instance.OnDoubleClick += SwitchGameObjectActive;//？？？？？？？？？？？

        UIIcon[] UIIcons = GetComponentsInChildren<UIIcon>();
        foreach (var icon in UIIcons)
        {
            icon._OnClick += (()=> { gameObject.SetActive(false); });//
        }


        transform.Find("TakePhote").GetComponent<UIIcon>()._OnClick +=(()=> 
        {
            Debug.Log("Click the take Photo");
            PictureVideoCapture.Instance.TakePhoto();
        });

        transform.Find("StartVideo").GetComponent<UIIcon>()._OnClick += (() =>
        {
            Debug.Log("Start recording Video");
            PictureVideoCapture.Instance.StartRecordingVideo();
        });

        transform.Find("StopVideo").GetComponent<UIIcon>()._OnClick += (() =>
        {
            Debug.Log("Stop recording Video");
            PictureVideoCapture.Instance.StopRecordingVideo();
        });

        transform.Find("QRCode").GetComponent<UIIcon>()._OnClick += (() => 
        {
            // by_xqzhu
            ModelMesh.SetActive(false);

            QRCodeDetector.Instance.tryOpenQRCodeDetector();//开启二维码扫描
        });

        transform.Find("显示隐藏").GetComponent<UIIcon>()._OnClick += (() =>//这里显示；隐藏的是Hierarchy面板中的ObjectsInfo物体
        {
            if (animationBtn.activeSelf == true) animationBtn.SetActive(false);
            else infoBtn.SetActive(!infoBtn.activeSelf);
        });
    }

    private void SwitchGameObjectActive()//切换UI界面显示或者隐藏
    {

        if (!GazeManager.Instance.Hit)//     对空处双击才会执行
        {

            gameObject.SetActive(!gameObject.activeSelf);
        }
          
    }

    private void OnDestroy()
    {
        GestureManager.Instance.OnDoubleClick -= SwitchGameObjectActive;
    }
}
