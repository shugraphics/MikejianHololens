using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QRCodeDetector : Singleton<QRCodeDetector>
{
    //TODO: low fps while the detector is running
    public GameObject QRCodeUI_Prefab;//QRCode预制体
    private GameObject QRCodeUI;
    private Text QRCodeText;

    private Color32[] data { get; set; }
    private WebCamTexture webCameraTexture;
    private BarcodeReader QRCodeReader;

    private IEnumerator scanQRCodeCoroutine;

    public delegate void DetectedQRCode(string CodeResult);
    public event DetectedQRCode OnDetectedQRCode;

    private void Start()
    {
        scanQRCodeCoroutine = ScanQRCode();
        StartCoroutine(InitWebCamera()); //开启初始化摄像头的协程InitWebCamera（）

        //GestureManager.Instance.OnDoubleClick += tryOpenQRCodeDetector;
    }

    public void tryOpenQRCodeDetector()//UI界面的调用从这开始
    {
        if (webCameraTexture == null)
            return;

        if (webCameraTexture.isPlaying)//如果相机在使用中，则stopQRCodeDetector
            stopQRCodeDetector();
        else
            openQRCodeDetector();
    }

    private void openQRCodeDetector()//开启相机进行
    {
        webCameraTexture.Play();
        QRCodeUI = Instantiate(QRCodeUI_Prefab);//实例化QRCodeUI_Prefab这个预制体
        InitQRCodeUIComponent();//??????
        CursorManager.Instance.HideCursor = true;//把光标和手setactive(false)
        StartCoroutine(scanQRCodeCoroutine);//开启协程，scanQRCodeCoroutine==ScanQRCode（），即开启扫描二维码的协程
    }

    private void stopQRCodeDetector()//和openQRCodeDetector()对应
    {
        webCameraTexture.Stop();
        Destroy(QRCodeUI);
        CursorManager.Instance.HideCursor = false;
        StopCoroutine(scanQRCodeCoroutine);//结束协程
    }

    private void InitQRCodeUIComponent()
    {
        if (QRCodeUI == null)
            return;

        UIUtils.SetEachZTestMode(QRCodeUI, GUIZTestMode.Always);
        QRCodeUI.GetComponent<Canvas>().worldCamera = Camera.main;
        QRCodeText = QRCodeUI.transform.Find("ScanResult").GetComponent<Text>();
    }

    private IEnumerator ScanQRCode()//扫描二维码的函数，将扫出来的结果存在_result变量中
    {
        QRCodeReader = new BarcodeReader();
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            data = webCameraTexture.GetPixels32();//相机捕捉到的纹理  
            Result _result = QRCodeReader.Decode(data, webCameraTexture.width, webCameraTexture.height);
            if (_result != null)
                DetectedQRHandle(_result);//调用DetectedQRHandle()函数
        }
    }

    private void DetectedQRHandle(Result _QRResult)//
    {
        QRCodeText.text = _QRResult.Text;
        stopQRCodeDetector();

        if (OnDetectedQRCode != null)
            OnDetectedQRCode(_QRResult.Text);
    }


    private IEnumerator InitWebCamera()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            if (devices.Length == 0)
                yield break;
            string devicename = devices[0].name;

            webCameraTexture = new WebCamTexture(devicename, Screen.width, Screen.height);
        }
    }
    private void OnDestroy()
    {
        //GestureManager.Instance.OnDoubleClick -= tryOpenQRCodeDetector;
    }
}
