using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Assets.Scripts.Visual
{
    public class StringAnimationAndGet : MonoBehaviour
    {
        [Header("From what script: ")]
        [TextArea(30, 20)]
        public string references;

        [Header("Parameters")]
        [SerializeField] private float delay;
        [SerializeField] private TextMeshPro output;

        [Space(15)] [Header("Event in")]
        [SerializeField] private bool useEvent;
        [SerializeField] private float eventDelay;
        public UnityEvent eventAfter;
        
        private void Start()
        {
            if (references == null)
                return;

            StartCoroutine(TypeSentences());

            if (useEvent)
                StartCoroutine(ShowEvent());
        }

        private IEnumerator TypeSentences()
        {
            output.text = "";
            yield return new WaitForSeconds(delay);
            foreach (var letter in references.ToCharArray())
            {
                output.text += letter;
                yield return null;
            }
        }

        private IEnumerator ShowEvent()
        {
            yield return new WaitForSeconds(eventDelay);
            eventAfter?.Invoke();
        }
    }
}
