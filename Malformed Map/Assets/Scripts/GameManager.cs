using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MalformedMap
{
    public class GameManager : MonoBehaviour
    {
        #region Statics
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GameManager>();

                    if (instance == null)
                    {
                        Debug.LogError("A GameManager object could not be found in the scene.");
                    }
                }

                return instance;
            }
        }
        #endregion Statics

        [SerializeField]
        private List<SmallCubeType> _smallCubeTypes;

        [SerializeField]
        private List<SmallCubeType> _smallCubeTypes_Rare;

        [SerializeField]
        private int _maxMisfortune;

        private GameUI _UI;

        public int MaxMisfortune
        {
            get { return _maxMisfortune; }
        }

        public int totalActionsTaken;
        public int TotalActionsTaken
        {
            get { return totalActionsTaken; }
            set { totalActionsTaken = value; }
        }

        public int MisfortuneAccumulated { get; set; }
        public int TreasureCollected { get; set; }
        public int TerraformsCollected_Forest { get; set; }
        public int TerraformsCollected_Water { get; set; }

        public bool GameOver
        {
            get { return MisfortuneAccumulated >= MaxMisfortune; }
        }

        /// <summary>
        /// Initializes the game.
        /// </summary>
        void Awake()
        {
            _UI = FindObjectOfType<GameUI>();
            if (_UI == null)
            {
                Debug.LogError("GameUI object could not be found in the scene.");
            }
        }

        public void CollectCube(SmallCube sm)
        {
            switch (sm.Category)
            {
                case SmallCube.FormCategory.Malform:
                {
                    MisfortuneAccumulated++;
                    break;
                }
                case SmallCube.FormCategory.Treasure:
                {
                    TreasureCollected++;
                    break;
                }
                case SmallCube.FormCategory.Terraform:
                {
                    if (sm.Type == SmallCube.TerraformType.Forest)
                    {
                        TerraformsCollected_Forest++;
                    }
                    else if (sm.Type == SmallCube.TerraformType.Water)
                    {
                        TerraformsCollected_Water++;
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Gets a random small cube type.
        /// </summary>
        /// <returns>A small cube type</returns>
        public SmallCubeType GetRandomSmallCubeType()
        {
            SmallCubeType result = _smallCubeTypes[0];

            float random = Random.Range(0, 2.5f);
            if (random < 2f)
            {
                result = _smallCubeTypes[(int) random];
            }
            else
            {
                random = Random.Range(0, 2);
                if (random == 2f)
                {
                    random = 0f;
                }

                result = _smallCubeTypes_Rare[(int) random];
            }

            return result;
        }

        public void EndCollecting()
        {
            TotalActionsTaken++;
            _UI.UpdateUI();
        }
    }
}
