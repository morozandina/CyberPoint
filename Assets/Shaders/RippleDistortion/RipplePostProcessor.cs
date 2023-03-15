using UnityEngine;

namespace Shaders.RippleDistortion
{
    [RequireComponent(typeof(Camera))]
    public class RipplePostProcessor : MonoBehaviour
    {
        public static RipplePostProcessor ins;

        private const float EXPECTED_DELTATIME_AT_60FPS = 1f / 60f;
        public const float LOWEST_AMOUNT_VALUE = 0.0001f;
        public Material RippleMaterial;
        public float MaxAmount = 50f;

        [Range(0, 1)]
        public float Friction = .9f;

        public float CurrentAmount;
        private bool _update;
        
        // Other
        private Camera _camera => GetComponent<Camera>();

        private void Awake()
        {
            if (ins != null)
            {
                Destroy(this);
                return;
            }
            ins = this;
        }
        private void Start()
        {
            CurrentAmount = RippleMaterial.GetFloat("_Amount");
            _update = CurrentAmount > LOWEST_AMOUNT_VALUE;
        }

        private void Update()
        {
            if (!_update)
                return;
        
            RippleMaterial.SetFloat("_Amount", CurrentAmount);
            var amountToReduce = CurrentAmount * (1 - Friction);
            CurrentAmount -= amountToReduce * (Time.deltaTime / EXPECTED_DELTATIME_AT_60FPS);
        
            if (!(CurrentAmount < LOWEST_AMOUNT_VALUE))
                return;
        
            _update = false;
            CurrentAmount = 0;
            RippleMaterial.SetFloat("_Amount", CurrentAmount);
        }
    
        private void OnApplicationQuit()
        {
            RippleMaterial.SetFloat("_Amount", 0);
            RippleMaterial.SetFloat("_CenterX", 0);
            RippleMaterial.SetFloat("_CenterY", 0);
        }
    
        public void Ripple(Vector3 pos)
        {
            CurrentAmount = MaxAmount;
            var position = _camera.WorldToScreenPoint(pos);
            RippleMaterial.SetFloat("_CenterX", position.x);
            RippleMaterial.SetFloat("_CenterY", position.y);
            _update = true;
        }
    }
}