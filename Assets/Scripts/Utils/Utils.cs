using Controller;
using Interface;
using JetBrains.Annotations;
using Player;
using UnityEngine;

public static class Utils
{
    [CanBeNull]
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
                parent = parent.transform,
                localPosition = Vector3.zero
            }
        };
        return go;
    }

    public static bool IsNull(object obj)
    {
        if (obj == null) return true;
        if (obj is IMyDisposable disposable) return disposable.IsDisposed();
        return false;
    }
}