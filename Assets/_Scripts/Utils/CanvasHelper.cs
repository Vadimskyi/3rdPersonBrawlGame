using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(RectTransform))]
    public class CanvasHelper : MonoSingleton<CanvasHelper>
    {
        private RectTransform _rectTransform;
        private Vector2 _uiOffset;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _uiOffset = new Vector2((float)_rectTransform.sizeDelta.x / 2f, (float)_rectTransform.sizeDelta.y / 2f);
        }

        public Vector3 WorldToCanvasPoint(Vector3 worldPoint)
        {
            Vector2 viewportPosition = Camera.main.WorldToViewportPoint(worldPoint);
            Vector2 proportionalPosition = new Vector2(viewportPosition.x * _rectTransform.sizeDelta.x, viewportPosition.y * _rectTransform.sizeDelta.y);
            
            return proportionalPosition - _uiOffset;
        }
    }
}
