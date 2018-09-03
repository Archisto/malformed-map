using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MalformedMap
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField]
        private Text MisfortuneAccumulated;

        [SerializeField]
        private Text TreasureCollected;

        [SerializeField]
        private Text TerraformsCollected_Forest;

        [SerializeField]
        private Text TerraformsCollected_Water;

        /// <summary>
        /// Initializes the object.
        /// </summary>
        private void Start()
        {
            UpdateUI();
        }

        /// <summary>
        /// Updates the UI.
        /// </summary>
        public void UpdateUI()
        {
            UpdateMisfortuneCounter();

            TreasureCollected.text =
                GameManager.Instance.TreasureCollected.ToString();
            TerraformsCollected_Forest.text =
                GameManager.Instance.TerraformsCollected_Forest.ToString();
            TerraformsCollected_Water.text =
                GameManager.Instance.TerraformsCollected_Water.ToString();
        }

        private void UpdateMisfortuneCounter()
        {
            if (GameManager.Instance.GameOver)
            {
                MisfortuneAccumulated.color = Color.red;
            }

            MisfortuneAccumulated.text =
                GameManager.Instance.MisfortuneAccumulated.ToString()
                + " / " + GameManager.Instance.MaxMisfortune;
        }
    }
}
