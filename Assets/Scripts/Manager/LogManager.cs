//#define LOG_MANAGER
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class LogManager
{
#if UNITY_EDITOR
	[OnOpenAsset(0)]
	static bool OnOpenAsset(int instanceID, int line)
	{
		var stackTrace = GetStackTrace();
		if (!stackTrace.Contains("[LogManager]"))
		{
			return false;
		}
		Match matches = Regex.Match(stackTrace, @"\(at(.+)\)", RegexOptions.IgnoreCase);
		matches = matches.NextMatch();
		if (matches.Success)
		{
			string pathLine = matches.Groups[1].Value;
			// Debug.Log(pathLine);
			pathLine = pathLine.Replace(" ", "");
			int splitIndex = pathLine.LastIndexOf(":", StringComparison.Ordinal);
			string path = pathLine[..splitIndex];
			line = Convert.ToInt32(pathLine[(splitIndex + 1)..]);
			string fullPath = Application.dataPath[..Application.dataPath.LastIndexOf("Assets", StringComparison.Ordinal)];
			fullPath = fullPath + path;
			string strPath = fullPath.Replace('/', '\\');
			UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(strPath, line);
			return true;
		}
		return false;
	}

	static string GetStackTrace()
	{
		//UnityEditor.ConsoleWindow
		var consoleWndType = typeof(EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
		//找到成员
		var fieldInfo = consoleWndType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
		var consoleWnd = fieldInfo.GetValue(null);
		if (consoleWnd == null)
			return "";
		//// 如果console窗口时焦点窗口的话，获取stacktrace
		if ((object)consoleWnd == EditorWindow.focusedWindow)
		{
			fieldInfo = consoleWndType.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
			return fieldInfo.GetValue(consoleWnd).ToString();
		}
		return "";
	}
#endif
	
    public static string GetValuesStr(params object[] values)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var value in values)
        {
            stringBuilder.Append(value);
            stringBuilder.Append("\t");
        }

        return stringBuilder.ToString();
    }

    public static string GetTraceStr(params object[] values)
    {
        StringBuilder stringBuilder = new StringBuilder(GetValuesStr(values));
        stringBuilder.Append("\nTrace stack info:");
        string trackStr = new System.Diagnostics.StackTrace().ToString();
        stringBuilder.Append(trackStr);
        return stringBuilder.ToString();
    }
    
    //[Conditional("LOG_MANAGER")]
    public static void Trace(params object[] values)
    {
        
        Debug.Log(GetTraceStr(values));
    }
    
    //[Conditional("LOG_MANAGER")]
    public static void TraceE(params object[] values)
    {
        
        Debug.LogError(GetTraceStr(values));
    }

    //[Conditional("LOG_MANAGER")]
    public static void Log(params object[] values)
    {
        Debug.Log(GetValuesStr(values) + "\n[LogManager]");
    }
    
    //[Conditional("LOG_MANAGER")]
    public static void LogWarning(params object[] values)
    {
        Debug.LogWarning(GetValuesStr(values) + "\n[LogManager]");
    }
    
    //[Conditional("LOG_MANAGER")]
    public static void LogError(params object[] values)
    {
        Debug.LogError(GetValuesStr(values) + "\n[LogManager]");
    }
}