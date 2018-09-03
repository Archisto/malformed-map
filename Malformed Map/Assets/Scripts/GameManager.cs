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

        public int collectedCubeCount;
        public int CollectedCubeCount
        {
            get { return collectedCubeCount; }
            set { collectedCubeCount = value; }
        }

        /// <summary>
        /// ...
        /// </summary>
        void Awake()
        {

        }
    }
}
