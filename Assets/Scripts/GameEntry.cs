using Controller.Window;
using Event;
using Manager;
using Unity.VisualScripting;
using UnityEngine;

public class GameEntry : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerManager _playerManager;
    private EventManager _eventManager;
    private FpsCounter _fpsCounter;
    private ServerManager _serverManager;
    private ControllerManager _controllerManager;
    private ItemManager _itemManager;
    private WindowManager _windowManager;
    
    void Start()
    {
        _playerManager = PlayerManager.Get();
        _eventManager = EventManager.Get();
        //_fpsCounter = new FpsCounter();
        
        _serverManager = ServerManager.Get();
        // _serverManager.StartHost();
        // _serverManager.StartClient();

        _controllerManager = ControllerManager.Get();
        _itemManager = ItemManager.Get();

        _windowManager = WindowManager.Get();
        _windowManager.OpenWindow("connect_window");

        transform.AddComponent<CoroutineHandler>();
        
        LogManager.Log("Game start");
    }

    private void Update()
    {
        _eventManager.TriggerEvent(Events.UPDATE);
    }
    
    private void FixedUpdate()
    {
        _eventManager.TriggerEvent(Events.FIXED_UPDATE);
    }

    private static GameEntry _instance;
    public static GameEntry Get()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindObjectOfType<GameEntry>();
        }
        return _instance;
    }
}