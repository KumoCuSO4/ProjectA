using System.Collections.Generic;
using Utils;

namespace Table
{
    public class WindowTable : Singleton<WindowTable>
    {
        private Dictionary<string, WindowData> data = new Dictionary<string, WindowData>();

        public WindowTable()
        {
            data["connect_window"] = new WindowData("ConnectWindow", 2);
        }

        public WindowData GetWindowData(string windowName)
        {
            return data[windowName];
        }
        
    }

    public class WindowData
    {
        public string name { private set; get; }
        public int layer { private set; get; }

        public WindowData(string name, int layer)
        {
            this.name = name;
            this.layer = layer;
        }
    }
}