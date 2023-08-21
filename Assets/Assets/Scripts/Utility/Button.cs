using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Assets.Scripts.Utility
{
    public class Button : MonoBehaviour
    {
        public Color normalColor;
        public Color pressColor;
        public Graphic targetGraphic;
        [Space(5)]
        public UnityEvent onClick;

        private void OnMouseDown()
        {
            if (targetGraphic)
                targetGraphic.color = pressColor;
        }

        private void OnMouseUp()
        {
            if (targetGraphic)
                targetGraphic.color = normalColor;
            onClick?.Invoke();
        }

        private void OnValidate()
        {
            normalColor = Color.white;
            pressColor = Color.black;
            gameObject.TryGetComponent(out targetGraphic);
        }
    }
}
