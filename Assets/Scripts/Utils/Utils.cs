using Controller;
using UnityEngine;

public static class Utils
{
    public static PlayerController GetLocalPlayer()
    {
        return PlayerManager.Get().GetLocalPlayer();
    }

    public static GameObject AddChild(GameObject parent, GameObject obj)
    {
        GameObject instance = Object.Instantiate(obj, parent.transform);
        return instance;
    }

    public static GameObject AddChild(GameObject parent, string name)
    {
        GameObject go = new GameObject(name)
        {
            transform =
            {
                parent = parent.transform
            }
        };
        return go;
    }
}