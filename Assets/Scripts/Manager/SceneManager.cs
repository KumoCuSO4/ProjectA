using System.Collections.Generic;
using Controller;
using Controller.Scene;
using JetBrains.Annotations;
using UnityEngine;

namespace Manager
{
    public class SceneManager : Singleton<SceneManager>
    {
        private BaseScene _curScene = null;
        private Const.SceneID _curSceneID = Const.SceneID.NONE;

        public void EnterScene(Const.SceneID sceneID)
        {
            if (_curSceneID == Const.SceneID.NONE || _curSceneID != sceneID)
            {
                ExitCurScene();
                string sceneName = "scene_name";
                GameObject _sceneRoot = new GameObject(sceneName);
                _curScene = new BaseScene(_sceneRoot);
                _curSceneID = sceneID;
            }
        }
        
        public void ExitCurScene()
        {
            if (_curScene == null) return;
            _curScene.Exit();
            _curScene = null;
        }

        public Const.SceneID GetCurSceneID()
        {
            return _curSceneID;
        }

        [CanBeNull]
        public BaseScene GetCurScene()
        {
            return _curScene;
        }
    }
}