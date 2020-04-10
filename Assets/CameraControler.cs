using RTS_Cam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControler : MonoBehaviour
{
    public Button buttonUp;
    public Button buttonDown;

    public Button button45;
    public Button button90;
    public RTS_Camera camera;
    private float baseHeight;
    public float HeightSpeed;
    public float maxHeight;
    public float minHeight;
    float heightDir = 0;

    private bool isHeightChanging = false;
    private void Start()
    {
        Transform camT = Camera.main.transform;
        button45.onClick.AddListener(() => SetXRotation(camT, 45f));
        button90.onClick.AddListener(() => SetXRotation(camT, 90f));
        //buttonUp.onClick.AddListener(() => CameraChangeHeight(HeightSpeed));
        //buttonDown.onClick.AddListener(() => CameraChangeHeight(-HeightSpeed));
        baseHeight = camera.minHeight;
    }

    private void Update()
    {
        if (isHeightChanging
            && (camera.minHeight < maxHeight && heightDir > 0
            || camera.minHeight > minHeight && heightDir < 0))
        {
            CameraChangeHeight(heightDir);
        }
    }

    private void SetXRotation(Transform t, float angle)
    {
        t.localEulerAngles = new Vector3(angle, t.localEulerAngles.y, t.localEulerAngles.z);
    }

    private void CameraChangeHeight(float delta)
    {
        camera.minHeight += delta;
    }


    public void StartChangingHeight(int dir)
    {
        isHeightChanging = true;
        heightDir = dir * HeightSpeed;
    }

    public void StopChangeingHeight()
    {
        isHeightChanging = false; ;
        heightDir = 0;
    }


}
