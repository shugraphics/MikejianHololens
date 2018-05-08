using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// To track whether hand is detected or not
/// </summary>
public class HandsManager : Singleton<HandsManager>
{
    public bool HandDetected
    {
        get;
        private set;
    }

    private void Awake()
    {
        UnityEngine.XR.WSA.Input.InteractionManager.InteractionSourceDetected += Source_Detected;
        UnityEngine.XR.WSA.Input.InteractionManager.InteractionSourceLost += Source_Lost;
    }

    private void OnDestroy()
    {
        UnityEngine.XR.WSA.Input.InteractionManager.InteractionSourceDetected -= Source_Detected;
        UnityEngine.XR.WSA.Input.InteractionManager.InteractionSourceLost -= Source_Lost;
    }

    private void Source_Detected(UnityEngine.XR.WSA.Input.InteractionSourceDetectedEventArgs hand)
    {
        HandDetected = true;
    }

    private void Source_Lost(UnityEngine.XR.WSA.Input.InteractionSourceLostEventArgs hand)
    {
        HandDetected = false;
    }
}
