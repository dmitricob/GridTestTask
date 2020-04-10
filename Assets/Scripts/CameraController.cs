using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject gm;

    public float _flySpeed;
    public float _rotateSpeed;
    public float _rotateRate = 0.02f;
    public Vector3 _lookingAt = new Vector3(0,0,0);
    private bool _rotateEnable;
    private int _rotateDir;

    private bool _moveIsStarted;
    private Vector3 _beginMovePosition;
    private Vector3 _newMovePosition;
    private Vector3 dragOrigin;

    private void Awake()
    {
        //TryToSetLookAt();
        transform.LookAt(_lookingAt);
    }

    private void TryToSetLookAt()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit))
            _lookingAt = raycastHit.point;

        
    }

    Vector3 lastDragPosition;

    void Update()
    {
        UpdateDrag1();
        UpdateDrag();
    }

    void UpdateDrag()
    {
        if (Input.GetMouseButtonDown(2))
            lastDragPosition = Input.mousePosition;
        if (Input.GetMouseButton(2))
        {
            var delta = lastDragPosition - Input.mousePosition;
            transform.Translate(delta * Time.deltaTime * 0.25f);
            lastDragPosition = Input.mousePosition;
        }
    }

    private void UpdateDrag1()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(gm, Camera.main.ScreenToViewportPoint(Input.mousePosition), transform.rotation);
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * _flySpeed, 0, pos.y * _flySpeed);
        move *= -1;
        transform.Translate(move, Space.World);
        dragOrigin = Input.mousePosition;
        _lookingAt += move;
        transform.LookAt(_lookingAt);
    }

    //private void Update()
    //{
    //    UpdateDrag();
    //    //Debug.DrawRay(transform.position - new Vector3(0,0,0.2f),transform.forward*1000,Color.red);

    //    //if (Input.GetMouseButtonDown(0))
    //    //{
    //    //    Instantiate(gm, Camera.main.ScreenToViewportPoint(Input.mousePosition),transform.rotation);
    //    //    return;
    //    //}
    //}

    public void RotateOnY(float angle)
    {
        transform.RotateAround(_lookingAt, Vector3.up, _rotateSpeed * _rotateRate * _rotateDir);
        //transform.RotateAround(_lookingAt, Vector3.up, 30 * Time.deltaTime * _rotateDir);
    }

    public void RotateAround()
    {
        RotateOnY(_rotateSpeed);
    }
    public void StartRotate(bool rotateRight)
    {
        TryToSetLookAt();
        if (rotateRight)
            _rotateDir = -1;
        else
            _rotateDir = 1;
        _rotateEnable = true;
        InvokeRepeating("RotateAround",0,_rotateRate);
    }
    public void StopRotate()
    {
        _rotateEnable = false;
        CancelInvoke("RotateAround");
    }

    public void Move()
    {
        _newMovePosition = Input.GetTouch(0).position;
        if (_moveIsStarted)
        {
            Vector3 dif = _newMovePosition - _beginMovePosition;
            transform.Translate(_newMovePosition);
        }
        //transform.Translate(dir);
    }
    public void StartMove()
    {
        _moveIsStarted = true;
        _beginMovePosition = Input.GetTouch(0).position;
        Debug.Log("move start");
    }
    public void StopMove()
    {
        _moveIsStarted = false;
        Debug.Log("move start");
    }
}
