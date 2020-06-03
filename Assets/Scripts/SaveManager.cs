using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveManager : MonoBehaviour {

    [Serializable]
    private struct WorldInfo {

        public int Width;
        public int Height;
        public EntityInfo[] ObjectArray;
        public EntityInfo[] ActorArray;
        public string[] GroundArray;

        [Serializable]
        public struct EntityInfo {

            public string Type;
            public float Locx;
            public float Locy;
            public string Info;
        }
    }

    public string WorldToString() {
        WorldInfo info;

        // Encode interactable objects
        WorldObject[] objects = Utils.Flatten2dArray(WorldController.Instance.ObjectMap);
        List<WorldInfo.EntityInfo> objectList = new List<WorldInfo.EntityInfo>();
        for (int i = 0; i < objects.Length; i++) {

            if (objects[i] != null) {

                WorldInfo.EntityInfo objectInfo;
                objectInfo.Type = objects[i].GetType().ToString();
                objectInfo.Locx = objects[i].GetLocationX();
                objectInfo.Locy = objects[i].GetLocationY();
                objectInfo.Info = objects[i].ObjectToString();
                objectList.Add(objectInfo);
            }
        }
        info.ObjectArray = objectList.ToArray();

        // Encode actor objects 
        info.ActorArray = new WorldInfo.EntityInfo[WorldController.Instance.ActorList.Length];
        for (int i = 0; i < WorldController.Instance.ActorList.Length; i++) {

            WorldInfo.EntityInfo actorInfo;
            actorInfo.Type = WorldController.Instance.ActorList[i].GetType().ToString();
            actorInfo.Locx = WorldController.Instance.ActorList[i].GetLocationX();
            actorInfo.Locy = WorldController.Instance.ActorList[i].GetLocationY();
            actorInfo.Info = WorldController.Instance.ActorList[i].ObjectToString();
            info.ActorArray[i] = actorInfo;
        }

        // Encode Tiles
        info.GroundArray = Utils.Flatten2dArray(WorldController.Instance.GroundMap);
        info.Width = WorldController.Instance.Width;
        info.Height = WorldController.Instance.Height;

        return JsonUtility.ToJson(info);
    }

    public void WorldFromString(string info) {

        // Destroy old interactable objects
        foreach (WorldObject obj in Utils.Flatten2dArray(WorldController.Instance.ObjectMap)) {
            if (obj != null)
            {
                Destroy(obj.gameObject);
            }
        }

        // Destroy old actor objects
        foreach (Actor obj in WorldController.Instance.ActorList) {
            if (obj != null) {
                Destroy(obj.gameObject);
            }
        }

        // Decode world info
        WorldInfo worldInfo = JsonUtility.FromJson<WorldInfo>(info);

        // Set interactable objects
        WorldController.Instance.ObjectMap = new WorldObject[worldInfo.Width, worldInfo.Height];
        foreach (WorldInfo.EntityInfo obj in worldInfo.ObjectArray) {

            Vector3 location = new Vector3(obj.Locx, obj.Locy, 0);
            GameObject prefab = WorldResources.GetGameObject(obj.Type);
            WorldObject interactableObject = Instantiate(prefab, location, Quaternion.identity).GetComponent<WorldObject>();
            interactableObject.ObjectFromString(obj.Info);
            WorldController.Instance.ObjectMap[(int)obj.Locx, (int)obj.Locy] = interactableObject;
        }

        // Set tiles
        WorldController.Instance.GroundMap = Utils.Reshape2dArray(worldInfo.GroundArray, worldInfo.Width, worldInfo.Height);
        for (int i = 0; i < WorldController.Instance.Width; i++) {
            for (int j = 0; j < WorldController.Instance.Height; j++) {
                WorldController.Instance.WorldTilemap.SetTile(new Vector3Int(i, j, 0), WorldResources.GetTile(WorldController.Instance.GroundMap[i, j]));
            }
        }

        // Set actor objects
        WorldController.Instance.ActorList = new Actor[worldInfo.ActorArray.Length];
        for (int i = 0; i < worldInfo.ActorArray.Length; i++) {

            Vector3 location = new Vector3(worldInfo.ActorArray[i].Locx, worldInfo.ActorArray[i].Locy, 0);
            GameObject prefab = WorldResources.GetGameObject(worldInfo.ActorArray[i].Type);
            Actor actor = Instantiate(prefab, location, Quaternion.identity).GetComponent<Actor>();
            actor.ObjectFromString(worldInfo.ActorArray[i].Info);
            WorldController.Instance.ActorList[i] = actor;

            if (worldInfo.ActorArray[i].Type == "Player") {
                WorldController.Instance.WorldCamera.ToFollow = actor.gameObject;
            }
        }
    }
}
