using System;
using System.Collections.Generic;
using System.Linq;
using Controller.Window;
using Table;
using UnityEngine;
using Object = UnityEngine.Object;

public class WindowManager : Singleton<WindowManager>
{
    private Dictionary<string, BaseWindow> windows = new Dictionary<string, BaseWindow>();
    private Transform windowRoot;

    public WindowManager()
    {
        windowRoot = GameObject.Find("UI").transform;
    }
    
    public BaseWindow GetWindow(string windowName)
    {
        if (!windows.ContainsKey(windowName))
        {
            return null;
        }
        BaseWindow window = windows[windowName];
        if (window.IsDisposed())
        {
            windows.Remove(windowName);
            return null;
        }
        return window;
    }
    
    public BaseWindow OpenWindow(string windowName, params object[] windowParams)
    {
        BaseWindow window = GetWindow(windowName);
        if (window != null)
        {
            CloseWindow(window);
        }
        
        WindowData windowData = WindowTable.Get().GetWindowData(windowName);
        string className = windowData.name;
        Type type = Type.GetType("Controller.Window." + className);
        if (type == null)
        {
            LogManager.LogError("窗口类不存在", className);
            return null;
        }
        
        GameObject windowPrefab = Resources.Load<GameObject>("Prefabs/Window/" + windowName);
        GameObject obj = Object.Instantiate(windowPrefab, windowRoot);
        obj.transform.localPosition = Vector3.zero;
        object[] constructorArgs = { obj, windowName };
        
        constructorArgs = constructorArgs.Concat(windowParams).ToArray();
        window = Activator.CreateInstance(type, constructorArgs) as BaseWindow;
        windows[windowName] = window;
        window.InitWindow();
        return window;
    }
    
    public void CloseWindow(string windowName)
    {
        BaseWindow window = GetWindow(windowName);
        CloseWindow(window);
    }
    
    public void CloseWindow(BaseWindow window)
    {
        if (window == null)
        {
            return;
        }
        window.WillClose();
        window.Dispose();
        windows.Remove(window.GetWindowName());
    }
}