using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Vadimskyi.Game
{
    public class GameLevelController : MonoBehaviour
    {
        public TMP_InputField NameField;
        public Button PlayButton;
        public string BattleSceneName;

        private LevelLoader _loader;

        private void Awake()
        {
            Application.targetFrameRate = 30;
            _loader = GetComponentInChildren<LevelLoader>(true);
        }

        public void StartLoading()
        {

        }

        public void EndLoading()
        {

        }

        public void LoadArena()
        {
            LoadLevel();
        }

        private async void LoadLevel()
        {
            _loader.Start();
            await SceneManager.LoadSceneAsync(BattleSceneName).AsAsyncOperationObservable();
            _loader.Stop();
        }
    }
}
