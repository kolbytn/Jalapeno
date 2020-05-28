using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveManager : MonoBehaviour
{
    [Serializable]
    private struct WorldInfo
    {
        public int width;
        public int height;
        public ObjectInfo[] objectArray;
        public string[] groundArray;

        [Serializable]
        public struct ObjectInfo
        {
            public string type;
            public float locx;
            public float locy;
            public string info;
        }
    }

    public string WorldToString()
    {
        WorldInfo info;

        WorldObject[] objects = Utils.Flatten2dArray(WorldController.Instance.ObjectMap);
        List<WorldInfo.ObjectInfo> objectList = new List<WorldInfo.ObjectInfo>();
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != null)
            {
                WorldInfo.ObjectInfo objectInfo;
                objectInfo.type = objects[i].GetType().ToString();
                objectInfo.locx = objects[i].GetLocationX();
                objectInfo.locy = objects[i].GetLocationY();
                objectInfo.info = objects[i].ObjectToString();
                objectList.Add(objectInfo);
            }
        }

        info.objectArray = objectList.ToArray();
        info.groundArray = Utils.Flatten2dArray(WorldController.Instance.GroundMap);
        info.width = WorldController.Instance.Width;
        info.height = WorldController.Instance.Height;

        return JsonUtility.ToJson(info);
    }

    public void WorldFromString(string info)
    {
        foreach (WorldObject obj in Utils.Flatten2dArray(WorldController.Instance.ObjectMap))
        {
            if (obj != null)
            {
                Destroy(obj.gameObject);
            }
        }

        WorldInfo worldInfo = JsonUtility.FromJson<WorldInfo>(info);

        WorldController.Instance.ObjectMap = new InteractableObject[worldInfo.width, worldInfo.height];

        foreach (WorldInfo.ObjectInfo obj in worldInfo.objectArray)
        {
            Vector3 location = new Vector3(obj.locx, obj.locy, 0);
            GameObject prefab = WorldResources.GetGameObject(obj.type);
            InteractableObject interactableObject = Instantiate(prefab, location, Quaternion.identity).GetComponent<InteractableObject>();
            interactableObject.ObjectFromString(obj.info);
            WorldController.Instance.ObjectMap[(int)obj.locx, (int)obj.locy] = interactableObject;

            // if (obj.type == "PlayerController")
            // {
            //     WorldController.Instance.WorldCamera.ToFollow = worldObject.gameObject;
            // }
        }

        WorldController.Instance.GroundMap = Utils.Reshape2dArray(worldInfo.groundArray, worldInfo.width, worldInfo.height);

        for (int i = 0; i < WorldController.Instance.Width; i++)
        {
            for (int j = 0; j < WorldController.Instance.Height; j++)
            {
                WorldController.Instance.WorldTilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GetTile(WorldController.Instance.GroundMap[i, j]));
            }
        }
    }
}
