using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Visual
{
    public class StringAnimationAndGet : MonoBehaviour
    {
        [Header("From what script: ")]
        [TextArea(30, 20)]
        public string references;
        [Header("Parameters")]
        [SerializeField] private TextMeshPro output;
        [SerializeField] private GameObject menu;

        private void Start()
        {
            if (references == null)
                return;

            StartCoroutine(TypeSentences());
        }

        private IEnumerator TypeSentences()
        {
            output.text = "";
        
            foreach (var letter in references.ToCharArray())
            {
                output.text += letter;
                yield return null;
            }

            menu.SetActive(true);
        }
    }
}
