using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MalformedMap
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private BigCube _bigCube;

        [SerializeField]
        private float _distance;

        [SerializeField]
        private bool _enableWobble;

        [SerializeField]
        private float _wobbleDuration;

        [SerializeField]
        private float _maxWobble;

        private Vector3 _defaultPosition;
        private Vector3 _oldPosition;
        private Vector3 _targetPosition;
        private Quaternion _defaultRotation;
        private Quaternion _oldRotation;
        private Quaternion _targetRotation;
        private float elapsedTime;

        /// <summary>
        /// Is the camera wobbling.
        /// </summary>
        private bool Wobbling { get; set; }

        /// <summary>
        /// Initializes the object.
        /// </summary>
        private void Start()
        {
            transform.position = _bigCube.transform.position + new Vector3(0, 0, _distance);

            _defaultPosition = transform.position;
            _defaultRotation = transform.rotation;

            if (_enableWobble)
            {
                StartWobble();
            }
        }

        /// <summary>
        /// Updates the object once per frame.
        /// </summary>
        private void Update()
        {
            if (Wobbling)
            {
                Wobble();
            }
        }

        private void StartWobble()
        {
            Vector3 randomPos = _defaultPosition + new Vector3(
                Random.Range(-1 * _maxWobble, _maxWobble),
                Random.Range(-1 * _maxWobble, _maxWobble));

            _targetPosition = randomPos;
            _targetRotation = Quaternion.LookRotation(_bigCube.transform.position - _targetPosition);

            _oldPosition = transform.position;
            _oldRotation = transform.rotation;

            elapsedTime = 0;
            Wobbling = true;
        }

        private void EndWobble()
        {
            transform.position = _targetPosition;
            transform.rotation = _targetRotation;

            Wobbling = false;
        }

        private void ResetDefaultPosAndRot()
        {
            transform.position = _defaultPosition;
            transform.rotation = _defaultRotation;
        }

        /// <summary>
        /// Moves and rotates the camera.
        /// </summary>
        private void Wobble()
        {
            float ratio = elapsedTime / _wobbleDuration;
            transform.position = Vector3.Slerp(_oldPosition, _targetPosition, ratio);
            transform.rotation = Quaternion.Lerp(_oldRotation, _targetRotation, ratio);

            elapsedTime += Time.deltaTime;
            if (elapsedTime >= _wobbleDuration)
            {
                EndWobble();
                StartWobble();
            }
        }
    }
}
