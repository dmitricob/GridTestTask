using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildedObject : MonoBehaviour
{
    public int id;
    public string objectName;
    public Vector2 SizeInGrid;

    internal void SetParams(int id, string name,Vector2 sizeInGrid)
    {
        this.id = id;
        this.objectName = name;
        this.SizeInGrid = sizeInGrid;
    }

    public void GetInfo()
    {
        Debug.Log($"id - {id} Name {objectName} Size {SizeInGrid}");
    }

    private void OnMouseDown()
    {
        this.GetInfo();
    }
}
