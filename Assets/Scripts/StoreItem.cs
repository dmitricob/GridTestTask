using UnityEngine;

namespace Assets.Scripts
{
    public class StoreItem
    {
        public int id;
        public string name;
        public string description;
        public Vector3 scale;
        public GameObject prefab;
        public Vector2 gridSize;
        public string imgPath;
        public Sprite image;

        public StoreItem(int id, string name, string description, Vector3 scale, GameObject prefab, Vector2 gridSize, string imgPath, Sprite image)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.scale = scale;
            this.prefab = prefab;
            this.gridSize = gridSize;
            this.imgPath = imgPath;
            this.image = image;
        }

        public static StoreItem CreteItem(int id, string name, string description, Vector3 scale, GameObject prefab, Vector2 gridSize, string imgPath, Sprite image)
        {
            return new StoreItem(id, name, description, scale, prefab, gridSize, imgPath, image);
        }
    }
}
