using System.Collections;
using Shaders.RippleDistortion;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class BlastWave : MonoBehaviour
    {
        [Header("Wave Control"), Space(10)]
        private const int pointsCount = 50;
        [SerializeField] private float maxRadius;
        [SerializeField] private float speed;
        [SerializeField] private float startWidth;

        [Header("Boom Control"), Space(10)]
        [SerializeField] private ParticleSystem[] booms;
        
        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();

            _lineRenderer.positionCount = pointsCount + 1;
            
            BOOM();
        }

        private IEnumerator Blast()
        {
            var currentRadius = 0f;
            
            while (currentRadius < maxRadius)
            {
                currentRadius += Time.deltaTime * speed;
                Draw(currentRadius);
                yield return null;
            }
        }

        private void Draw(float currentRadius)
        {
            var angleBetweenPoints = 360 / pointsCount;

            for (var i = 0; i <= pointsCount; i++)
            {
                var angle = i * angleBetweenPoints * Mathf.Deg2Rad;
                var direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
                var position = direction * currentRadius;
                
                _lineRenderer.SetPosition(i, position);
            }

            _lineRenderer.widthMultiplier = Mathf.Lerp(0f, startWidth, 1f - currentRadius / maxRadius);
        }

        public void BOOM()
        {
            StartCoroutine(Blast());
            foreach (var ps in booms)
            {
                ps.Play();
            }
            RipplePostProcessor.ins.Ripple(transform.position);
        }
    }
}
