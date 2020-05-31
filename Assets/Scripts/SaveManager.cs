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
        public WorldObjectInfo[] objectArray;
        public WorldObjectInfo[] actorArray;
        public string[] groundArray;

        [Serializable]
        public struct WorldObjectInfo
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

        // Encode interactable objects
        WorldObject[] objects = Utils.Flatten2dArray(WorldController.Instance.ObjectMap);
        List<WorldInfo.WorldObjectInfo> objectList = new List<WorldInfo.WorldObjectInfo>();
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != null)
            {
                WorldInfo.WorldObjectInfo objectInfo;
                objectInfo.type = objects[i].GetType().ToString();
                objectInfo.locx = objects[i].GetLocationX();
                objectInfo.locy = objects[i].GetLocationY();
                objectInfo.info = objects[i].ObjectToString();
                objectList.Add(objectInfo);
            }
        }
        info.objectArray = objectList.ToArray();

        // Encode actor objects 
        info.actorArray = new WorldInfo.WorldObjectInfo[WorldController.Instance.ActorList.Length];
        for (int i = 0; i < WorldController.Instance.ActorList.Length; i++)
        {
            WorldInfo.WorldObjectInfo actorInfo;
            actorInfo.type = WorldController.Instance.ActorList[i].GetType().ToString();
            actorInfo.locx = WorldController.Instance.ActorList[i].GetLocationX();
            actorInfo.locy = WorldController.Instance.ActorList[i].GetLocationY();
            actorInfo.info = WorldController.Instance.ActorList[i].ObjectToString();
            info.actorArray[i] = actorInfo;
        }

        // Encode Tiles
        info.groundArray = Utils.Flatten2dArray(WorldController.Instance.GroundMap);
        info.width = WorldController.Instance.Width;
        info.height = WorldController.Instance.Height;

        return JsonUtility.ToJson(info);
    }

    public void WorldFromString(string info)
    {
        // Destroy old interactable objects
        foreach (WorldObject obj in Utils.Flatten2dArray(WorldController.Instance.ObjectMap))
        {
            if (obj != null)
            {
                Destroy(obj.gameObject);
            }
        }

        // Destroy old actor objects
        foreach (WorldObject obj in WorldController.Instance.ActorList)
        {
            if (obj != null)
            {
                Destroy(obj.gameObject);
            }
        }

        // Decode world info
        WorldInfo worldInfo = JsonUtility.FromJson<WorldInfo>(info);

        // Set interactable objects
        WorldController.Instance.ObjectMap = new InteractableObject[worldInfo.width, worldInfo.height];
        foreach (WorldInfo.WorldObjectInfo obj in worldInfo.objectArray)
        {
            Vector3 location = new Vector3(obj.locx, obj.locy, 0);
            GameObject prefab = WorldResources.GetGameObject(obj.type);
            InteractableObject interactableObject = Instantiate(prefab, location, Quaternion.identity).GetComponent<InteractableObject>();
            interactableObject.ObjectFromString(obj.info);
            WorldController.Instance.ObjectMap[(int)obj.locx, (int)obj.locy] = interactableObject;
        }

        // Set tiles
        WorldController.Instance.GroundMap = Utils.Reshape2dArray(worldInfo.groundArray, worldInfo.width, worldInfo.height);
        for (int i = 0; i < WorldController.Instance.Width; i++)
        {
            for (int j = 0; j < WorldController.Instance.Height; j++)
            {
                WorldController.Instance.WorldTilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GetTile(WorldController.Instance.GroundMap[i, j]));
            }
        }

        // Set actor objects
        WorldController.Instance.ActorList = new Actor[worldInfo.actorArray.Length];
        for (int i = 0; i < worldInfo.actorArray.Length; i++)
        {
            Vector3 location = new Vector3(worldInfo.actorArray[i].locx, worldInfo.actorArray[i].locy, 0);
            GameObject prefab = WorldResources.GetGameObject(worldInfo.actorArray[i].type);
            Actor actor = Instantiate(prefab, location, Quaternion.identity).GetComponent<Actor>();
            actor.ObjectFromString(worldInfo.actorArray[i].info);
            WorldController.Instance.ActorList[i] = actor;

            if (worldInfo.actorArray[i].type == "Player")
            {
                WorldController.Instance.WorldCamera.ToFollow = actor.gameObject;
            }
        }
    }
}
