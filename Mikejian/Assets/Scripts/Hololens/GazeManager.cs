using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeManager : Singleton<GazeManager>
{
    [Tooltip("The max gaze distance for calculationg a hit")]
    public float MaxGazeDistance = 10.0f;//

    [Tooltip("Select the layers raycast should target")]
    public LayerMask RaycastLayerMask = Physics.DefaultRaycastLayers;//RaycastLayerMask表示要要凝视的Mask

    public float lastHitDistance = 2.0f;//保存上一次射线与凝视物体碰撞的距离

    /// <summary>
    /// Whether the raycast hit something or not
    /// </summary>
    public bool Hit//用于判断是否检测到碰撞
    {
        get;
        private set;
    }

    public RaycastHit HitInfo//用于保存碰撞的信息
    {
        get;
        private set;
    }

    public GameObject HitObject//保存碰撞的物体
    {
        get;
        private set;
    }
    /// <summary>
    /// Hit Position
    /// </summary>
    public Vector3 HitPosition//碰撞点，就是gaze的坐标位置
    {
        get;
        private set;
    }

    /// <summary>
    /// Hit Normal
    /// </summary>
    public Vector3 HitNormal//碰撞点的法线，就是gaze的点的法向量
    {
        get;
        private set;
    }

    private Vector3 gazeOrigin;//保存相机的位置，即人的位置
    private Vector3 gazeDirection;//保存人的视线的方向

    private void Update()
    {
        gazeOrigin = Camera.main.transform.position;
        gazeDirection = Camera.main.transform.forward;

        //there is no stable

        UpdateRaycast();
    }

    private void UpdateRaycast()
    {
        RaycastHit hitInfo;
        Hit = Physics.Raycast(gazeOrigin, gazeDirection, out hitInfo, MaxGazeDistance, RaycastLayerMask);
        HitInfo = hitInfo;

        //if hit something then the Hitposition and HitNormal will according to the hit data 
        //else the Hitposition and HitNormal will according to the camera's rotation and MaxGazeDistance 
        if (Hit)
        {
            //minus to make sure that the cursor will always be out the model********
            HitPosition = hitInfo.point-gazeDirection*0.1f;
            HitNormal = hitInfo.normal;
            HitObject = hitInfo.collider.gameObject;
            lastHitDistance = HitInfo.distance;
        }
        else//如果没有与新的物体碰撞
        {
            HitPosition = gazeOrigin + (gazeDirection * 4);
            HitNormal = gazeDirection;
            HitObject = null;
        }
    }
}
