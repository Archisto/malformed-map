using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MalformedMap
{
    public class InputManager : MonoBehaviour
    {
        private const string HorizontalKey = "Horizontal";
        private const string VerticalKey = "Vertical";

        [SerializeField]
        private BigCube _bigCube;

        [SerializeField]
        private CameraController _camera;

        /// <summary>
        /// Initializes the object.
        /// </summary>
        private void Start()
        {

        }

        /// <summary>
        /// Updates the object once per frame.
        /// </summary>
        private void Update()
        {
            CheckInput();
        }

        private void CheckInput()
        {
            Vector3 direction = Vector3.zero;

            direction += new Vector3(Input.GetAxisRaw(HorizontalKey), 0, 0);
            direction += new Vector3(0, Input.GetAxisRaw(VerticalKey), 0);

            if (direction != Vector3.zero)
            {
                _bigCube.MovementInput(direction);
            }
        }
    }
}
