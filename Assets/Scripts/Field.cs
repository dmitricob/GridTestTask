using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Field : MonoBehaviour
{
    public Material _gridMaterial;
    private Renderer _renderer;
    private Material[] _materials;
    public bool _isGridOn = false;
    public Action OnHit;
    public int _cellsCount;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _materials = _renderer.sharedMaterials;
        if (_isGridOn)
            _materials[1] = _gridMaterial;
        else
            _materials[1] = null;
        SetupMaterialsSettings(_cellsCount);
    }


    public void SetupMaterialsSettings(int cellsCount)
    {
        _cellsCount = cellsCount;
        _materials[0].mainTextureScale = new Vector2(cellsCount, cellsCount);
        _materials[1]?.SetFloat("_GridSize", cellsCount);
        _renderer.sharedMaterials = _materials;
    }

    public void TogleGrid()
    {
        _isGridOn = !_isGridOn;
        if (_isGridOn)
        {
            _materials[1] = _gridMaterial;
        }
        else{
            _materials[1] = null;
        }
        _renderer.sharedMaterials = _materials;
    }

    private void OnMouseDown()
    {
        if (OnHit == null)
            return;
        OnHit();
    }

}
