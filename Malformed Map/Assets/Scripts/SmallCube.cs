using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MalformedMap
{
    public class SmallCube : MonoBehaviour
    {
        public enum Category
        {
            Terraform = 0,
            Malform = 1,
            Treasure = 2,
            Challenge = 3
        }

        [SerializeField]
        private BigCube.Side[] _sides = new BigCube.Side[3];

        private Renderer _renderer;

        public bool Selected { get; set; }

        /// <summary>
        /// Initializes the object.
        /// </summary>
        private void Start()
        {
            _renderer = GetComponent<Renderer>();
        }

        /// <summary>
        /// Updates the object once per frame.
        /// </summary>
        private void Update()
        {

        }

        public void SetMaterial(Material material)
        {
            if (_renderer != null)
            {
                _renderer.material = material;
            }
        }
    }
}
