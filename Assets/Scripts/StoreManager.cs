using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public List<GameObject> _prefabs;
    public List<StoreItem> _storeItems;
    public Button _storeItemButton;
    //public XMLData _xmlData;  ToDo
    public bool isStoreActive = false;
    public GameObject storeWindow;


    public void SetPrefabs(List<GameObject> prefabs)
    {
        _prefabs = prefabs;
    }

    public void InitStorItems()
    {
        _storeItems = new List<StoreItem>();

        UnityEngine.Object[] images = Resources.LoadAll("Images", typeof(Sprite));
        Vector3 scale = new Vector3(1, 1, 1);
        Vector2 gridSize = new Vector2(1, 1);
        int id = 0;
        foreach (var prefab in _prefabs)
        {
            string name = prefab.name;
            var currentImg = FindInByName<Sprite>(images, name);

            if (name.Contains("rock"))
                gridSize = new Vector2(2, 2);
            else
                gridSize = new Vector2(3, 3);

            _storeItems.Add(StoreItem.CreteItem(
                id++,
                name,
                $"This is {name}",
                scale,
                prefab,
                gridSize,
                "",
                currentImg));
        }
    }

    private T FindInByName<T>(UnityEngine.Object[] array, string name) where T : class
    {
        foreach (var item in array)
        {
            if (item is T)
            {
                if (item.name == name)
                {
                    return item as T;
                }
            }
        }
        return default(T);
    }

    public void AddStoreItem(StoreItem storeItem)
    {
        Button newButton = Instantiate(_storeItemButton, transform);
        newButton.GetComponent<Image>().sprite = storeItem.image;
        (newButton.GetComponentInChildren(typeof(Text)) as Text).text = storeItem.name;
        newButton.onClick.AddListener(delegate { ButtonClicked(storeItem); });
        //btn.GetComponent<Button>().onClick.AddListener(delegate { btnClicked(param); });
    }

    public void Populate()
    {
        foreach (var item in _storeItems)
        {
            AddStoreItem(item);
        }
    }

    public void ButtonClicked(StoreItem storeitem)
    {
        BuildManager.GetInstance().SetObjectToBuildByName(storeitem);
        CloseStore();
    }

    public void OpenStore()
    {
        if (!isStoreActive)
        {
            isStoreActive = true;
            storeWindow.active = true;
        }
    }

    public void CloseStore()
    {
        if (isStoreActive)
        {
            isStoreActive = false;
            storeWindow.active = false;
        }
    }
}
