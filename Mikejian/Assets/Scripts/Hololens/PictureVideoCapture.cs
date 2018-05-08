using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.WSA.WebCam;


public class PictureVideoCapture : Singleton<PictureVideoCapture>
{
    private PhotoCapture m_PhotoCapture;
    private VideoCapture m_VideoCapture;

    private string pictureFolderPath;

    private string currentImagePath;
    private string currentVideoPath;

    private void Start()
    {
#if NETFX_CORE
        getPicturesFolderAsync();
#endif
    }

    public void StartRecordingVideo()
    {
        VideoCapture.CreateAsync(true, OnVideoCaptureCreated);
    }

    void OnVideoCaptureCreated(VideoCapture videoCapture)
    {
        if (videoCapture != null)
        {
            m_VideoCapture = videoCapture;

            Resolution cameraResolution = VideoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
            float cameraFramerate = VideoCapture.GetSupportedFrameRatesForResolution(cameraResolution).OrderByDescending((fps) => fps).First();

            CameraParameters cameraParameters = new CameraParameters();
            cameraParameters.hologramOpacity = 1.0f;
            cameraParameters.frameRate = cameraFramerate;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

            m_VideoCapture.StartVideoModeAsync(cameraParameters,
                                                VideoCapture.AudioState.None,
                                                OnStartedVideoCaptureMode);
        }
        else
        {
            Debug.LogError("Failed to create VideoCapture Instance!");
        }
    }

    void OnStartedVideoCaptureMode(VideoCapture.VideoCaptureResult result)
    {
        if (result.success)
        {
            string filename = string.Format("MyVideo_{0}.mp4", Time.time);
            currentVideoPath = System.IO.Path.Combine(Application.persistentDataPath, filename);

            m_VideoCapture.StartRecordingAsync(currentVideoPath, OnStartedRecordingVideo);
        }
    }

    void OnStartedRecordingVideo(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Started Recording Video!");
    }

    public void StopRecordingVideo()
    {
        m_VideoCapture.StopRecordingAsync(OnStoppedRecordingVideo);
    }

    void OnStoppedRecordingVideo(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Stopped Recording Video!");
        //if (m_VideoCapture.IsRecording)
            m_VideoCapture.StopVideoModeAsync(OnStoppedVideoCaptureMode);
    }

    void OnStoppedVideoCaptureMode(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("currentVideoPath is " + currentVideoPath);
#if NETFX_CORE
        System.IO.File.Move(currentVideoPath, System.IO.Path.Combine(pictureFolderPath, "Camera Roll", System.IO.Path.GetFileName(currentVideoPath)));
#endif
        m_VideoCapture.Dispose();
        m_VideoCapture = null;
    }

    public void TakePhoto()
    {
        PhotoCapture.CreateAsync(true, OnPhotoCaptureCreated);
    }

    private void OnPhotoCaptureCreated(PhotoCapture captureObject)
    {
        m_PhotoCapture = captureObject;

        Resolution resolution = PhotoCapture.SupportedResolutions.OrderByDescending(res => res.width * res.height).First();

        CameraParameters c = new CameraParameters(WebCamMode.PhotoMode);
        c.hologramOpacity = 1.0f;
        c.cameraResolutionWidth = resolution.width;
        c.cameraResolutionHeight = resolution.height;
        c.pixelFormat = CapturePixelFormat.BGRA32;

        m_PhotoCapture.StartPhotoModeAsync(c, OnPhotoModeStarted);
    }

    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        string file = string.Format(@"Image_{0:yyyy-MM-dd_hh-mm-ss-tt}.jpg", DateTime.Now);
        currentImagePath = System.IO.Path.Combine(Application.persistentDataPath, file);

        m_PhotoCapture.TakePhotoAsync(currentImagePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
    }

    public void StopCamera()
    {
        m_PhotoCapture.StopPhotoModeAsync(OnPhotoModeStopped);
    }

    private void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
#if NETFX_CORE
            if(pictureFolderPath != null)
                System.IO.File.Move(currentImagePath, System.IO.Path.Combine(pictureFolderPath, "Camera Roll", System.IO.Path.GetFileName(currentImagePath)));
#endif
            StopCamera();
        }
        else
        {
            Debug.LogError(string.Format("Failed to save photo to disk ({0})", result.hResult));
        }
    }

    private void OnPhotoModeStopped(PhotoCapture.PhotoCaptureResult result)
    {
        m_PhotoCapture.Dispose();
        m_PhotoCapture = null;
    }

#if NETFX_CORE
    private async void getPicturesFolderAsync() {
        Windows.Storage.StorageLibrary picturesStorage = await Windows.Storage.StorageLibrary.GetLibraryAsync(Windows.Storage.KnownLibraryId.Pictures);
        pictureFolderPath = picturesStorage.SaveFolder.Path;
    }
#endif

}

