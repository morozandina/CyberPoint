using System;
using System.Collections.Generic;
using Assets.Scripts.Class;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.Camera
{
    public class CameraControl : MonoBehaviour
    {
        public Ease animationEase;
        public void ChangePosition(CameraWhatElement to)
        {
            to.cameraWatchObjects.beforeEvent?.Invoke();
            var sequence = DOTween.Sequence();
            sequence
                .Append(transform.DOMove(to.cameraWatchObjects.cameraToObjectPosition, 1.2f))
                .Insert(0, transform.DORotate(to.cameraWatchObjects.cameraToObjectRotation, sequence.Duration()))
                .SetEase(animationEase)
                .SetUpdate(true)
                .OnComplete(() => to.cameraWatchObjects.completeEvent?.Invoke());
        }
    }
}
