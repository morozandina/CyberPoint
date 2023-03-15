using System;
using System.Collections.Generic;
using Assets.Scripts.Class;
using UnityEngine;

namespace Assets.Scripts.Camera
{
    [Serializable]
    public class CameraWatchObject
    {
        public WhereCameraWatch WhereCameraWatch;
        public Vector3 CameraToObjectPosition;
        public Vector3 CameraToObjectRotation;
    }
    
    public class CameraControl : MonoBehaviour
    {
        public List<CameraWatchObject> CameraWatchObjects;
    }
}
