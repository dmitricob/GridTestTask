#undef UNITY_EDITOR
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandler
{
    private int _cellsCount = 20;
    private float _fogRate = 5;

    private List<GameObject> _prefabs;
    private Material _fogMaterialPrefab;
    public Transform _staticParent; 
    public List<GameObject> Prefabs { get => _prefabs; private set => _prefabs = value; }

    public ObjectHandler(int cellsCount, float fogRate, Material fogMateriaslPrefab)
    {
        _cellsCount = cellsCount;
        _fogRate = fogRate;
        _cellsCount += (int)(_fogRate * 2); // calculate foged field size
        _fogMaterialPrefab = fogMateriaslPrefab;
        Prefabs = new List<GameObject>();
    }

    public void RandomSpawnPrefabs()
    {
        // top field
        RandomSpawnPrefabs(
            -0.5f * _cellsCount,
            0.5f * _cellsCount,
            _cellsCount,
            _fogRate,
            1,
            -1);
        // right field
        RandomSpawnPrefabs(
            0.5f * _cellsCount,
            0.5f * _cellsCount,
            _fogRate,
            _cellsCount,
            -1,
            -1);
        // left field
        RandomSpawnPrefabs(
            -0.5f * _cellsCount,
            0.5f * _cellsCount,
            _fogRate,
            _cellsCount,
            1,
            -1);
        // bot field
        RandomSpawnPrefabs(
            -0.5f * _cellsCount,
            -0.5f * _cellsCount,
            _cellsCount,
            _fogRate,
            1,
            1);

    }
    private void RandomSpawnPrefabs(float topLeftX, float topLeftY, float width, float height, int xFieldDir = 1, int yFieldDir = 1)
    {
        int n = 20;
        for (int i = 0; i < n; i++)
        {
            int randPrefId = Random.Range(0, Prefabs.Count - 1);
            float randomSpawnX = Random.Range(0, width);
            float randomSpawnY = Random.Range(0, height);
            // ToDo check collisions
            var current = GameObject.Instantiate(
                Prefabs[randPrefId],
                new Vector3(
                    topLeftX + randomSpawnX * xFieldDir,
                    0,
                    topLeftY + randomSpawnY * yFieldDir),
                Quaternion.identity);
            InitHandler(current,"NaturalObject");
        }
    }

#if UNITY_EDITOR
    public void LoadPrefabs(string prefabFolder)
    {
        string[] assetsPaths = AssetDatabase.GetAllAssetPaths();
        List<string> prefabsPath = new List<string>();

        foreach (string assetPath in assetsPaths)
        {
            if (assetPath.Contains(prefabFolder))
            {
                prefabsPath.Add(assetPath);//prefabsPaths.Add(assetPath);
            }
        }
        
        foreach (var path in prefabsPath)
        {
            object current = AssetDatabase.LoadMainAssetAtPath(path);
            if (current is GameObject)
                _prefabs.Add(current as GameObject);
        }
    }
#else
    public bool LoadPrefabs(string prefabFolder)
    {
        object[] loadedObjects = Resources.LoadAll("Prefabs/"+prefabFolder);
        //Resources.Load();
        foreach (var item in loadedObjects)
        {
            if (item is GameObject)
                Prefabs.Add(item as GameObject);
        }
        if (Prefabs.Count == 0)
            Debug.LogError(" Prefabs didnt load");
        return Prefabs.Count > 0;
    }
#endif

    void InitHandler(GameObject target,string name)
    {
        target.transform.parent = _staticParent;
        target.name = name;
        List<Material> materials = new List<Material>(
            target.GetComponent<Renderer>().sharedMaterials);
        materials.Add(_fogMaterialPrefab);
        target.GetComponent<Renderer>().sharedMaterials = materials.ToArray();
    }

}
