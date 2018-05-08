using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionIndicatorsManager : Singleton<PositionIndicatorsManager>
{
    public Transform MarkObjsRoot; //MarkObj
    public Transform OriginModel;//测试5
    public GameObject ModelMesh;//Groop001

    private Vector3[] oldMarkers = new Vector3[4];
    private Vector3[] hit3DPos = new Vector3[4];

    private Matrix4x4 originMatrix; //Matrix4x4矩阵变换
    private Matrix4x4 targetMatrix;
    private Matrix4x4 transMatrix;
    //transMatrix*originMatrix=targetMatrix

    private bool[] indicatorsDoneState = new bool[4];//indicatorsDoneState变量是单个小方块放置完毕的标志（放置完毕==true）
    private bool allIndicatorsSetDone//TODO:Whether this will work   只通过前两个小方块的indicatorsDoneState信息来判断allIndicatorsSetDone（即是否完成放置）
    {
        get
        {
            bool allDone = true;
            for (int index = 0; index != indicatorsDoneState.Length-2; ++index)
                allDone &= indicatorsDoneState[index];
            return allDone;
        }
    }
   
    void UpdateOldPos()//获取到游戏场景中所有小方块的位置信息（有三处调用）
    {
        for (int index = 0; index != MarkObjsRoot.childCount; ++index)
        {
            oldMarkers[index] = MarkObjsRoot.GetChild(index).position;
        }
    }
    void RestoreToOldPos()
    {
        for (int index = 0; index != MarkObjsRoot.childCount; ++index)
        {
            MarkObjsRoot.GetChild(index).position = oldMarkers[index];
        }
    }
    private void Start()
    {
        //ModelMesh.SetActive(false);
        QRCodeDetector.Instance.OnDetectedQRCode += EnableMapping;//扫完二维码，调用EnableMapping（参数是OnDetectedQRCode）函数
        InitMatrix(out originMatrix);

        UpdateOldPos();//初始化时调用UpdateOldPos函数，先获取到游戏场景中所有小方块的位置信息
        for (int index = 0; index != MarkObjsRoot.childCount; ++index)//MarkObjsRoot.childCount:检视面板中MarkObj对象的所有子物体数目
        {
            indicatorsDoneState[index] = false;//所有子物体（即小方块）的indicatorsDoneState赋值false
        }
    }

    int currentMarkIndex = -1;//???????
    private void EnableMapping(string QRResult)
    {
        if (int.TryParse(QRResult, out currentMarkIndex))
        {
            currentMarkIndex = Mathf.Clamp(currentMarkIndex, 1, MarkObjsRoot.childCount) - 1;//currentMarkIndex代表当前放置的是哪个小方块，Clamp函数限制value的值在min和max之间， 如果value小于min，返回min。 如果value大于max，返回max，否则返回value
            SpatialMapping.Instance.MappingEnabled = true;
            GestureManager.Instance.OnDoubleClick += SetMarkObjPosition;
        }
        else
            Debug.Log("Exception: error QRResult " + currentMarkIndex);
    }

    float SignedAngleBetween(Vector3 a3, Vector3 b3)
    {
        Vector2 a2; a2.x = a3.x; a2.y = a3.z;
        Vector2 b2; b2.x = b3.x; b2.y = b3.z;

        float angle = Vector2.Angle(a2, b2);
        float sign = Mathf.Sign(Vector3.Dot(Vector3.up, Vector3.Cross(a3, b3)));
        float signed_angle = angle * sign;
        return (signed_angle <= 0) ? 360 + signed_angle : signed_angle;
    }

    private void SetMarkObjPosition()//计算小方块新的位置
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 30, SpatialMapping.PhysicsRaycastMask))
        {
            MarkObjsRoot.GetChild(currentMarkIndex).position = hitInfo.point;
            hit3DPos[currentMarkIndex] = hitInfo.point;
            SpatialMapping.Instance.MappingEnabled = false;//关闭SpatialMapping，等待下一轮扫描

            indicatorsDoneState[currentMarkIndex] = true;//indicatorsDoneState
            if (allIndicatorsSetDone)//TODO:make this in indicatorsDoneState's Set
            {
                /*ModelMesh.SetActive(true);
                InitMatrix(out targetMatrix);
                transMatrix = targetMatrix * originMatrix.inverse;*/
                /*SetModalTransform();*/

                RestoreToOldPos();
                //计算缩放量
                //UpdateOldPos();
                float originDistance = Vector3.Distance(oldMarkers[1], oldMarkers[0]);
                float targetDistance = Vector3.Distance(hit3DPos[1], hit3DPos[0]);
                float ScaleTimes = targetDistance / originDistance;
                OriginModel.localScale = Vector3.Scale(OriginModel.localScale, new Vector3(ScaleTimes, ScaleTimes, ScaleTimes));

                //计算平移量
                UpdateOldPos();
                Vector3 offset = hit3DPos[0] - oldMarkers[0];
                OriginModel.transform.Translate(offset);

                //计算旋转量
                UpdateOldPos();
                Vector3 rotOrigin = oldMarkers[0];
                Vector3 rotVecOld3D = oldMarkers[1] - oldMarkers[0];
                Vector3 rotVecNew3D = hit3DPos[1] - hit3DPos[0];
                float angle = SignedAngleBetween(rotVecOld3D, rotVecNew3D);
                OriginModel.transform.RotateAround(rotOrigin, Vector3.up, angle);//以小方块0作为旋转点，绕y轴旋转角度（angle）
                //OriginModel.transform.Rotate(0, 100, 0);
            }
        }

        GestureManager.Instance.OnDoubleClick -= SetMarkObjPosition;
    }


    private void SetModalTransform()//没用
    {
        //Scale
        float originDistance = Vector3.Distance(originMatrix.GetColumn(0), originMatrix.GetColumn(1));
        float targetDistance = Vector3.Distance(targetMatrix.GetColumn(0), targetMatrix.GetColumn(1));
        float ScaleTimes = targetDistance / originDistance;
        OriginModel.localScale=Vector3.Scale(OriginModel.localScale, new Vector3(ScaleTimes, ScaleTimes, ScaleTimes));

        //Rotation
        Vector4 forwardDirection = transMatrix.GetColumn(2).normalized;
        Vector4 upDirection = transMatrix.GetColumn(1).normalized;
        OriginModel.rotation = Quaternion.LookRotation(forwardDirection, upDirection) * OriginModel.rotation;

        //Position
        Vector4 targetPosition = OriginModel.position;
        targetPosition[3] = 1;
        targetPosition = transMatrix * targetPosition;
        OriginModel.position = targetPosition;
    }

    private void InitMatrix(out Matrix4x4 matrixToSet, bool test = false)
    {
        if (MarkObjsRoot == null || MarkObjsRoot.childCount != 4)
            Debug.LogError("Wrong Mark Obj State");
        Vector4[] positions = new Vector4[4];
        for (int index = 0; index != MarkObjsRoot.childCount; ++index)
        {
            positions[index] = MarkObjsRoot.GetChild(index).position;
            positions[index][3] = 1;
        }
        matrixToSet = new Matrix4x4(positions[0], positions[1], positions[2], positions[3]);

        Debug.Log("Matrix to Set is: "+ matrixToSet);
    }

    private void OnDestroy()
    {
        QRCodeDetector.Instance.OnDetectedQRCode -= EnableMapping;
    }
}
