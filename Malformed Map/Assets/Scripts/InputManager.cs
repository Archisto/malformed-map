using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MalformedMap
{
    public class InputManager : MonoBehaviour
    {
        private const string HorizontalKey = "Horizontal";
        private const string VerticalKey = "Vertical";
        private const string ActionKey = "Action";

        [SerializeField]
        private BigCube _bigCube;

        [SerializeField]
        private CameraController _camera;

        [SerializeField]
        private bool _invertedBigCubeRotation;

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
            // Rotating the big cube
            Vector3 direction = new Vector3(Input.GetAxisRaw(HorizontalKey), Input.GetAxisRaw(VerticalKey));

            if (direction != Vector3.zero)
            {
                direction = (_invertedBigCubeRotation ? -1 * direction : direction);
                _bigCube.RotateInput(direction);
            }

            // Collecting the selected small cubes
            if (Input.GetButtonDown(ActionKey))
            {
                _bigCube.CollectSmallCubesInput();
            }
        }
    }
}
