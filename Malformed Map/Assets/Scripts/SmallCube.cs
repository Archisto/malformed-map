using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MalformedMap
{
    public class SmallCube : MonoBehaviour
    {
        public enum FormCategory
        {
            Malform = 0,
            Treasure = 1,
            Terraform = 2,
            Challenge = 3
        }

        public enum TerraformType
        {
            None = 0,
            Forest = 1,
            Water = 2
        }

        public Vector3 _coordinates;
        public FormCategory _category;

        private Renderer _renderer;

        public Vector3 Coordinates
        {
            get { return _coordinates; }
            set { _coordinates = value; }
        }

        public FormCategory Category
        {
            get { return _category; }
            set { _category = value; }
        }

        public TerraformType Type { get; set; }

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

        public void SetType(SmallCubeType type)
        {
            if (_renderer != null)
            {
                Category = type.Category;
                Type = type.TerraformType;
                _renderer.material = type.Material;
            }
        }

        public void Collect()
        {
            Collected = true;
            SetTransparency(0);
            GameManager.Instance.CollectCube(this);
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
        }
    }
}
