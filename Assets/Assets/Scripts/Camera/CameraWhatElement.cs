using System;
using Assets.Scripts.Class;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Camera
{
    [Serializable]
    public class CameraWatchObject
    {
        public WhereCameraWatch whereCameraWatch;
        public Vector3 cameraToObjectPosition;
        public Vector3 cameraToObjectRotation;
        [Space(15)]
        public UnityEvent completeEvent;
        [Space(15)]
        public UnityEvent beforeEvent;

    }
    
    public class CameraWhatElement : MonoBehaviour
    {
        public CameraWatchObject cameraWatchObjects;
    }
}
