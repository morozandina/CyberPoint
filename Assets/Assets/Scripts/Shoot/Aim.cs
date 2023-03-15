using UnityEngine;

namespace Assets.Scripts.Shoot
{
    public class Aim : MonoBehaviour
    {
        private Quaternion rootRotation;
        private void Start() => rootRotation = transform.rotation;
        private void FixedUpdate() => transform.rotation = rootRotation;
    }
}
