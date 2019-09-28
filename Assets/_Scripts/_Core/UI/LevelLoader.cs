using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Vadimskyi.Game
{
    public class LevelLoader : MonoBehaviour
    {
        public Image Background;
        public Image LoaderIcon;

        private Tweener _tweener;

        public void Start()
        {
            Background.gameObject.SetActive(true);
            _tweener = LoaderIcon.transform.DORotate(new Vector3(0, 0, -180), 0.2f).SetLoops(-1, LoopType.Incremental);
        }

        public void Stop()
        {
            Background.gameObject.SetActive(false);
            _tweener?.Kill();
            _tweener = null;
        }
    }
}
