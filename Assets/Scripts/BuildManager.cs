using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager 
{
    private static BuildManager instance;
    private static object syncRoot = new System.Object();
    protected BuildManager()
    {
    }
    public static BuildManager GetInstance()
    {
        if (instance == null)
        {
            lock (syncRoot)
            {
                if (instance == null)
                    instance = new BuildManager();
            }
        }
        return instance;
    }



    private StoreItem selectedObjectToBuild;
    public GameObject buildedPrefab;
    private GameObject[,] fieldCells;

    public int cellsCount = 20;
    public int fieldSize = 20;
    private float cellSize = 1;
    public bool isSelectedObjectToBuild = false;

    public void SetParams(int cellCount, int fieldSize, GameObject buildedPrefab)
    {
        this.buildedPrefab = buildedPrefab;

        this.cellsCount = cellCount;
        this.fieldSize = fieldSize;
        cellSize = this.fieldSize / (cellCount*1f);

        fieldCells = new GameObject[this.cellsCount, this.cellsCount];
    }
    


    public void SetObjectToBuildByName(StoreItem storeItem)
    {
        if (storeItem == null)
            return;
        isSelectedObjectToBuild = true;
        selectedObjectToBuild = storeItem;
    }

    public GameObject TryToBuildSelectedObject(Vector3 position,Action<GameObject> InitiatinAction = null)
    {
        if (!isSelectedObjectToBuild)
        {
            Debug.Log("Unavailable plasce to build");
            return null;
        }
        
        position.x = Mathf.Floor(position.x);
        position.z = Mathf.Floor(position.z);

        Vector2 gridSize = selectedObjectToBuild.gridSize;

        if (!IsPostionAvailable(position, gridSize))
            return null;

        float xOffset = cellSize * 0.5f * (gridSize.x % 2 == 0 ? 2 : 1);
        float yOffset = cellSize * 0.5f * (gridSize.y % 2 == 0 ? 2 : 1);


        GameObject newObject = GameObject.Instantiate(buildedPrefab, position + new Vector3(xOffset, 0, yOffset), Quaternion.identity);

        if (InitiatinAction == null)
            DefaultInitialAction(newObject);
        else
            InitiatinAction(newObject);

        FillPosition(position,newObject, gridSize);

        isSelectedObjectToBuild = false;
        selectedObjectToBuild = null;

        // ToDo add particles emmising + buildEffect

        return newObject;
    }
   
    private void DefaultInitialAction(GameObject newBuilded)
    {
        // instantiate and add new model to object that will be on field
        GameObject newModel = GameObject.Instantiate(selectedObjectToBuild.prefab, newBuilded.transform);
        newModel.transform.localPosition = Vector3.zero;
        var collider = newModel.GetComponent<BoxCollider>();


        var newCollider = newBuilded.AddComponent<BoxCollider>();
        newCollider.size = collider.size;
        newCollider.center = collider.center;

        var buildedObject = newBuilded.GetComponent<BuildedObject>();
        buildedObject.SetParams(newBuilded.GetInstanceID(), selectedObjectToBuild.name,selectedObjectToBuild.gridSize);
        newBuilded.tag = "Builded";
    }

    // pos in grid dimmention
    public bool IsPostionAvailable(Vector3 pos, Vector2 size)
    {
        pos += new Vector3(cellsCount*0.5f, 0, cellsCount * 0.5f);


        int topLeftX = (int)(pos.x - Mathf.Ceil(size.x / 2) + 1);
        int topLeftY = (int)(pos.z - Mathf.Ceil(size.y / 2) + 1);
        
        int botRightX = (int)(pos.x + Mathf.Floor(size.x / 2));
        int botRightY = (int)(pos.z + Mathf.Floor(size.y / 2));

        if (topLeftX < 0
            || topLeftY < 0
            || botRightX >= cellsCount
            || botRightY >= cellsCount)
            return false;


            for (int i = topLeftX; i < botRightX + 1; i++)
        {
            for (int j = topLeftY; j < botRightY + 1; j++)
            {

                if (fieldCells[i, j] != null)
                    return false;
            }
        }

        return true;
    }    
    public void FillPosition(Vector3 pos,GameObject gm, Vector2 size)
    {
        pos += new Vector3(cellsCount * 0.5f, 0, cellsCount * 0.5f);


        int topLeftX = (int)(pos.x - Mathf.Ceil(size.x / 2) + 1);
        int topLeftY = (int)(pos.z - Mathf.Ceil(size.y / 2) + 1);

        int botRightX = (int)(pos.x +  Mathf.Floor(size.x / 2));
        int botRightY = (int)(pos.z +  Mathf.Floor(size.y / 2));

        for (int i = topLeftX; i < botRightX + 1; i++)
        {
            for (int j = topLeftY; j < botRightY + 1; j++)
            {
                fieldCells[i, j] = gm;

            }
        }
    }


}
