using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Control
{
    [Serializable]
    public enum ButtonDirection
    {
        Left,
        Right
    }

    public abstract class ButtonsControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public static int Rotation;
        public ButtonDirection buttonDirection;
        private void Start() => Rotation = 0;
        public abstract void OnPointerDown(PointerEventData eventData);
        public abstract void OnPointerUp(PointerEventData eventData);
    }
}
