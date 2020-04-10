using pools;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject _mainField;
    public int _cellsCount = 20;
    private int _fieldSize = 20;//!!!
    public float _fogStartFrom = 15;
    public float _fogRate = 5;

    public Material _fogMateriaslPrefab;

    public ObjectHandler _objectHandler;

    public StoreManager _storeManager;

    public Transform cameraTransform;

    public GameObject buildedPrefab;

    public Transform _static;


    private void Awake()
    {

        _objectHandler = new ObjectHandler(
            _cellsCount,
            _fogRate,
            _fogMateriaslPrefab);

        //_storeManager = new StoreManager();

        //_gameObjectPool = new GameObjectPool(_gameObjectPrefab,_gameObjectCount,GameObjectHandler,true);
    }

    void Start()
    {
        SetupSettings();
        //_gameObjectPool.prePopulate((int)_gameObjectCount);
        //_gameObjectPool.Spawn(new Vector3(0, 0, 0), Quaternion.identity);
        //_gameObjectPool.Unspawn(myGameObject);
        _objectHandler._staticParent = _static;
        _objectHandler.LoadPrefabs("NaturalObjPrefabs");
        _objectHandler.RandomSpawnPrefabs();

        BuildManager.GetInstance().SetParams(_cellsCount,_cellsCount, buildedPrefab);
        //BuildManager.GetInstance().SetPrefabs(_objectHandler.Prefabs);

        _storeManager.SetPrefabs(_objectHandler.Prefabs);
        _storeManager.InitStorItems();

        _storeManager.Populate();

    }

    void Update()
    {
        //objectHandler.LoadPrefabs("NaturalObjPrefabs");


        //RandomSpawnPrefabs();
    }

    void SetupSettings()
    {
        _mainField.GetComponent<Field>().SetupMaterialsSettings(_cellsCount);
        _mainField.GetComponent<Field>().OnHit = OnBuild;
        _fogMateriaslPrefab.SetFloat("_GridSize", _fogStartFrom);
        _fogMateriaslPrefab.SetFloat("_FogRate", _fogRate);

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnBuild()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z += 5;

        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);//new Ray(cameraTransform.position, pos - cameraTransform.position);
        RaycastHit raycasHit;
        Physics.Raycast(cameraRay,out raycasHit);
        var point = raycasHit.point;
        BuildManager.GetInstance().TryToBuildSelectedObject(point);
    }

}
