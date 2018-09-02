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
        private float _moveDuration;

        [SerializeField]
        private float _maxWobble;

        [SerializeField]
        private BigCube.Side _side = BigCube.Side.Front;

        private Vector3 _targetPosition;
        private Vector3 _oldPosition;
        private Quaternion _targetRotation;
        private Quaternion _oldRotation;
        private bool moving;
        private float elapsedTime;

        /// <summary>
        /// Is the camera moving.
        /// </summary>
        private bool Moving
        {
            get
            {
                return moving;
            }
            set
            {
                moving = value;
                if (value == true)
                {
                    elapsedTime = 0;
                }
                else
                {
                    _oldPosition = transform.position;
                    _oldRotation = transform.rotation;
                }
            }
        }

        /// <summary>
        /// Initializes the object.
        /// </summary>
        private void Start()
        {
            SnapToBigCubeSide(_side);
        }

        /// <summary>
        /// Updates the object once per frame.
        /// </summary>
        private void Update()
        {
            if (Moving)
            {
                Move();
            }
        }

        /// <summary>
        /// Moves and rotates the camera.
        /// </summary>
        private void Move()
        {
            float ratio = elapsedTime / _moveDuration;
            transform.position = Vector3.Slerp(_oldPosition, _targetPosition, ratio);
            transform.rotation = Quaternion.Lerp(_oldRotation, _targetRotation, ratio);

            elapsedTime += Time.deltaTime;
            if (elapsedTime >= _moveDuration)
            {
                transform.position = _targetPosition;
                transform.rotation = _targetRotation;
                Moving = false;
            }
        }

        /// <summary>
        /// Snaps the camera to a side of the big cube and points it towards the big cube.
        /// </summary>
        public void SnapToBigCubeSide(BigCube.Side side)
        {
            transform.position = GetTargetPosition(side);
            transform.rotation = GetTargetRotation(transform.position);
            Moving = false;
        }

        /// <summary>
        /// Gets the target position.
        /// </summary>
        /// <param name="side">The target big cube side</param>
        /// <returns>A Vector3 position</returns>
        private Vector3 GetTargetPosition(BigCube.Side side)
        {
            Vector3 direction = Utils.SideToVector3(side);
            return _bigCube.transform.position + direction * _distance;
        }

        /// <summary>
        /// Gets the target rotation.
        /// </summary>
        /// <param name="targetPosition">The target position</param>
        /// <returns>A Quaternion rotation</returns>
        private Quaternion GetTargetRotation(Vector3 targetPosition)
        {
            return Quaternion.LookRotation(_bigCube.transform.position - targetPosition);
        }

        /// <summary>
        /// Moves the camera based on the input direction.
        /// </summary>
        public void MovementInput(Vector3 direction)
        {
            if (Moving)
            {
                return;
            }

            _side = Utils.GetNeighborSide(_side, direction);
            _targetPosition = GetTargetPosition(_side);
            _targetRotation = GetTargetRotation(_targetPosition);
            Moving = true;
        }
    }
}
