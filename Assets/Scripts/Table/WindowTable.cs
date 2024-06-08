using System.Collections.Generic;

namespace Table
{
    public class WindowTable : Singleton<WindowTable>
    {
        private Dictionary<string, WindowData> data = new Dictionary<string, WindowData>();

        public WindowTable()
        {
            data["connect_window"] = new WindowData("ConnectWindow", 2);
            data["main_window"] = new WindowData("MainWindow", 2);
            data["place_window"] = new WindowData("PlaceWindow", 2);
        }

        public WindowData GetWindowData(string windowName)
        {
            return data[windowName];
        }
        
    }

    public record WindowData(string name, int layer);
}