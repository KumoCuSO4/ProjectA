
using System.Text;
using UnityEngine;

public static class LogManager
{

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

    public static void Trace(params object[] values)
    {
        
        Debug.Log(GetTraceStr(values));
    }
    
    public static void TraceE(params object[] values)
    {
        
        Debug.LogError(GetTraceStr(values));
    }

    public static void Log(params object[] values)
    {
        Debug.Log(GetValuesStr(values));
    }
    
    public static void LogWarning(params object[] values)
    {
        Debug.LogWarning(GetValuesStr(values));
    }
    
    public static void LogError(params object[] values)
    {
        Debug.LogError(GetValuesStr(values));
    }
}