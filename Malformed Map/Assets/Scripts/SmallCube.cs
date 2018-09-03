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

        public Vector3 _coordinates;

        private Renderer _renderer;

        public Vector3 Coordinates
        {
            get
            {
                return _coordinates;
            }
            set
            {
                _coordinates = value;
            }
        }

        public bool Selected { get; set; }

        public bool Collected { get; set; }

        public Vector3 OldPosition { get; set; }

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

        public void Collect()
        {
            Collected = true;
            SetTransparency(0);
        }

        public void Regenerate()
        {
            Collected = false;
            SetTransparency(1);
        }

        public void SetTransparency(float ratio)
        {
            Color newColor = _renderer.material.color;
            newColor.a = ratio;
            _renderer.material.color = newColor;
        }

        private void OnDrawGizmos()
        {
            if (Selected)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(transform.position, new Vector3(1.1f, 1.1f, 1.1f));
            }

            //if (Collected)
            //{
            //    Gizmos.color = Color.red;
            //    Gizmos.DrawWireCube(transform.position, new Vector3(1.15f, 1.15f, 1.15f));
            //}
        }
    }
}
