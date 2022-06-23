using System;
using TMPro;
using UnityEngine;

namespace SmartCities.API.Gira
{
    public class Poi : MonoBehaviour
    {
        [SerializeField] private TMP_Text debugText;
        [SerializeField] private Canvas canvas;

        public void UpdateText(string t)
        {
            debugText.text = t;
        }

        private void OnMouseDown()
        {
            canvas.gameObject.SetActive(!canvas.gameObject.activeSelf);
        }
    }
}