using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;


public class GestureManager : Singleton<GestureManager>
{
    public GestureRecognizer ActiveRecognizer{get;private set;}
    public GestureRecognizer TappedRecognizer{get;private set;}
    public GestureRecognizer ManipulationRecognizer { get; private set; }
    public GestureRecognizer NavigationRecognizer { get; private set; }
    public GestureRecognizer ScaleChangeRecognizer { get; private set; }

    [HideInInspector]
    public bool IsNavigation = false;
    [HideInInspector]
    public bool IsManipulation = false;
    [HideInInspector]
    public bool IsScaleChange = false;
    public Vector3 NavigationRelativePosition{get;private set;}
    public Vector3 ManipulationRelativePosition{get;private set;}
    public Vector3 ManipulationStartPosition{get;private set;}
    public Vector3 ScaleChangeRelativePosition { get; private set; }
    public Vector3 ScaleChangeStartPosition { get; private set; }

    public delegate void DoubleClickHandler();
    public event DoubleClickHandler OnDoubleClick;
    public delegate void SingleClickHandler();
    public event SingleClickHandler OnSingleClick;

    private void OnEnable()
    {
        TappedRecognizer = new GestureRecognizer();
        TappedRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.DoubleTap);
        TappedRecognizer.TappedEvent += Recognizer_TappedEvent;

        NavigationRecognizer = new GestureRecognizer();
        NavigationRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.DoubleTap | GestureSettings.NavigationRailsX);
        NavigationRecognizer.NavigationStartedEvent += NavigationRecognizer_Start;
        NavigationRecognizer.NavigationUpdatedEvent += NavigationRecognizer_Update;
        NavigationRecognizer.NavigationCompletedEvent += NavigationRecognizer_Completed;
        NavigationRecognizer.NavigationCanceledEvent += NavigationRecognizer_Canceled;
        NavigationRecognizer.TappedEvent += Recognizer_TappedEvent;

        ManipulationRecognizer = new GestureRecognizer();
        ManipulationRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.DoubleTap | GestureSettings.ManipulationTranslate);
        ManipulationRecognizer.ManipulationStartedEvent += ManipulationRecognzer_Start;
        ManipulationRecognizer.ManipulationUpdatedEvent += ManipulationRecognzer_Update;
        ManipulationRecognizer.ManipulationCompletedEvent += ManipulationRecognzer_Completed;
        ManipulationRecognizer.ManipulationCanceledEvent += ManipulationRecognzer_Canceled;
        ManipulationRecognizer.TappedEvent += Recognizer_TappedEvent;

        ScaleChangeRecognizer = new GestureRecognizer();
        ScaleChangeRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.DoubleTap | GestureSettings.ManipulationTranslate);
        ScaleChangeRecognizer.ManipulationStartedEvent += ScaleChangeRecognzer_Start;
        ScaleChangeRecognizer.ManipulationUpdatedEvent += ScaleChangeRecognzer_Update;
        ScaleChangeRecognizer.ManipulationCompletedEvent += ScaleChangeRecognzer_Completed;
        ScaleChangeRecognizer.ManipulationCanceledEvent += ScaleChangeRecognzer_Canceled;
        ScaleChangeRecognizer.TappedEvent += Recognizer_TappedEvent;


        SwitchRecognizer(TappedRecognizer);
    }

    public void SwitchRecognizer(GestureRecognizer newRecognizer)
    {
        Debug.Log("To active is " + newRecognizer.ToString());
        if (newRecognizer == null)
            return;
        if (ActiveRecognizer != null)
        {
            if (ActiveRecognizer == newRecognizer)
                return;
            ActiveRecognizer.CancelGestures();
            ActiveRecognizer.StopCapturingGestures();
        }
        newRecognizer.StartCapturingGestures();
        ActiveRecognizer = newRecognizer;
    }

    private void Recognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {;
        if (tapCount == 1 && OnSingleClick != null)
            OnSingleClick();
        if (tapCount == 2 && OnDoubleClick != null)
            OnDoubleClick();
    }
    
    private void NavigationRecognizer_Start(InteractionSourceKind source, Vector3 RelativePosition, Ray headRay)
    {
        IsNavigation = true;
        NavigationRelativePosition = RelativePosition;
    }
    private void NavigationRecognizer_Update(InteractionSourceKind source, Vector3 RelativePosition, Ray headRay)
    {
        IsNavigation = true;
        NavigationRelativePosition = RelativePosition;
    }
    private void NavigationRecognizer_Completed(InteractionSourceKind source, Vector3 RelativePosition, Ray headRay)
    {
        IsNavigation = false;
    }
    private void NavigationRecognizer_Canceled(InteractionSourceKind source, Vector3 RelativePosition, Ray headRay)
    {
        IsNavigation = false;
    }

    private void ManipulationRecognzer_Start(InteractionSourceKind source, Vector3 RelativePosition, Ray headRay)
    {
        IsManipulation = true;
        ManipulationStartPosition = RelativePosition;
    }

    private void ManipulationRecognzer_Update(InteractionSourceKind source, Vector3 RelativePosition, Ray headRay)
    {
        IsManipulation = true;
        ManipulationRelativePosition = RelativePosition;
    }

    private void ManipulationRecognzer_Completed(InteractionSourceKind source, Vector3 RelativePosition, Ray headRay)
    {
        IsManipulation = false;
    }

    private void ManipulationRecognzer_Canceled(InteractionSourceKind source, Vector3 RelativePosition, Ray headRay)
    {
        IsManipulation = false;
    }

    private void ScaleChangeRecognzer_Start(InteractionSourceKind source, Vector3 RelativePosition, Ray headRay)
    {
        IsScaleChange = true;
        ScaleChangeStartPosition = RelativePosition;
    }

    private void ScaleChangeRecognzer_Update(InteractionSourceKind source, Vector3 RelativePosition, Ray headRay)
    {
        IsScaleChange = true;
        ScaleChangeRelativePosition = RelativePosition;
    }

    private void ScaleChangeRecognzer_Completed(InteractionSourceKind source, Vector3 RelativePosition, Ray headRay)
    {
        IsScaleChange = false;
    }

    private void ScaleChangeRecognzer_Canceled(InteractionSourceKind source, Vector3 RelativePosition, Ray headRay)
    {
        IsScaleChange = false;
    }

    private void OnDisable()
    {
        TappedRecognizer.TappedEvent -= Recognizer_TappedEvent;

        NavigationRecognizer.NavigationStartedEvent -= NavigationRecognizer_Start;
        NavigationRecognizer.NavigationUpdatedEvent -= NavigationRecognizer_Update;
        NavigationRecognizer.NavigationCompletedEvent -= NavigationRecognizer_Completed;
        NavigationRecognizer.NavigationCanceledEvent -= NavigationRecognizer_Canceled;
        NavigationRecognizer.TappedEvent -= Recognizer_TappedEvent;

        ManipulationRecognizer.ManipulationStartedEvent -= ManipulationRecognzer_Start;
        ManipulationRecognizer.ManipulationUpdatedEvent -= ManipulationRecognzer_Update;
        ManipulationRecognizer.ManipulationCompletedEvent -= ManipulationRecognzer_Completed;
        ManipulationRecognizer.ManipulationCanceledEvent -= ManipulationRecognzer_Canceled;
        ManipulationRecognizer.TappedEvent -= Recognizer_TappedEvent;

        ScaleChangeRecognizer.ManipulationStartedEvent -= ScaleChangeRecognzer_Start;
        ScaleChangeRecognizer.ManipulationUpdatedEvent -= ScaleChangeRecognzer_Update;
        ScaleChangeRecognizer.ManipulationCompletedEvent -= ScaleChangeRecognzer_Completed;
        ScaleChangeRecognizer.ManipulationCanceledEvent -= ScaleChangeRecognzer_Canceled;
        ScaleChangeRecognizer.TappedEvent -= Recognizer_TappedEvent;
    }
}
