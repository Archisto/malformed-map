using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MalformedMap
{
    public class BigCube : MonoBehaviour
    {
        public enum Side
        {
            Front = 0,
            Back = 1,
            Left = 2,
            Right = 3,
            Top = 4,
            Bottom = 5,
            None = 6
        }

        [SerializeField]
        private Side _visibleSide = Side.Front;

        [SerializeField]
        private Vector3 _cameraDirection = Vector3.forward;

        [SerializeField]
        private float _rotationDuration;

        [SerializeField]
        private float _collectionDuration;

        [SerializeField]
        private LayerMask _visibleSideMask;

        [SerializeField]
        private Transform _rotationMarkerFront;

        [SerializeField]
        private Transform _rotationMarkerBack;

        [SerializeField]
        private Transform _rotationMarkerLeft;

        [SerializeField]
        private Transform _rotationMarkerRight;

        [SerializeField]
        private Transform _rotationMarkerTop;

        [SerializeField]
        private Transform _rotationMarkerBottom;

        [SerializeField]
        private Vector3 _smallCubeGridOrigin;

        [SerializeField]
        private float _smallCubeSize;

        private SmallCube[] _smallCubes;
        private SmallCube[] _selectedSmallCubes;

        private Vector3 _inputDirection;
        private float elapsedTime;
        //private Vector3 _rotationDir;
        //private Quaternion _targetRotation;
        //private Quaternion _oldRotation;

        /// <summary>
        /// Initializes the object.
        /// </summary>
        private void Start()
        {
            InitSmallCubes();
        }

        /// <summary>
        /// Initializes the small cubes.
        /// </summary>
        private void InitSmallCubes()
        {
            _smallCubes = transform.GetComponentsInChildren<SmallCube>();

            SetSmallCubeTypes();
            SetSmallCubeCoords();
            SelectVisibleSmallCubes();
        }

        /// <summary>
        /// Sets the small cubes' types at random.
        /// </summary>
        private void SetSmallCubeTypes()
        {
            foreach (SmallCube sm in _smallCubes)
            {
                sm.SetType(GameManager.Instance.GetRandomSmallCubeType());
            }
        }

        /// <summary>
        /// Sets the small cubes' coordinates inside the big cube.
        /// </summary>
        private void SetSmallCubeCoords()
        {
            // TODO: Fix

            foreach (SmallCube sm in _smallCubes)
            {
                int x = 0;
                int y = 0;
                int z = 0;

                for (int i = 0; i < 4; i++)
                {
                    float coord = _smallCubeGridOrigin.x + i * _smallCubeSize;

                    if (sm.transform.localPosition.x == coord)
                    {
                        x = i;
                    }
                    if (sm.transform.localPosition.y == coord)
                    {
                        y = i;
                    }
                    if (sm.transform.localPosition.z == coord)
                    {
                        z = i;
                    }
                }

                sm.Coordinates = new Vector3(x, y, z);
            }
        }

        /// <summary>
        /// Determines which small cubes are visible on camera and thus selected.
        /// </summary>
        private void SelectVisibleSmallCubes()
        {
            foreach (SmallCube sm in _smallCubes)
            {
                Ray ray = new Ray(sm.transform.position, Vector3.forward);
                bool raycastHit = Physics.Raycast(ray, _smallCubeSize, _visibleSideMask);
                sm.Selected = raycastHit;
            }
        }

        /// <summary>
        /// Updates the object once per frame.
        /// </summary>
        private void Update()
        {
            if (Rotating)
            {
                Rotate();
            }
            else if (Collecting)
            {
                MoveAndGenerateSmallCubes();
            }
        }

        /// <summary>
        /// Is the cube rotating.
        /// </summary>
        private bool Rotating { get; set; }

        /// <summary>
        /// Are the selected small cubes being collected.
        /// </summary>
        private bool Collecting { get; set; }

        /// <summary>
        /// Starts rotation.
        /// </summary>
        private void StartRotation()
        {
            Rotating = true;
            elapsedTime = 0;
        }

        /// <summary>
        /// Ends rotation.
        /// </summary>
        private void EndRotation()
        {
            Rotating = false;
            SelectVisibleSmallCubes();
        }

        /// <summary>
        /// Rotates the cube.
        /// </summary>
        private void Rotate()
        {
            if (_inputDirection.x != 0)
            {
                int sign = (_inputDirection.x > 0 ? 1 : -1);
                transform.Rotate(Vector3.up, sign * 90 * (Time.deltaTime / _rotationDuration), Space.World);
            }
            else if (_inputDirection.y != 0)
            {
                int sign = (_inputDirection.y > 0 ? 1 : -1);
                transform.Rotate(Vector3.right, sign * 90 * (Time.deltaTime / _rotationDuration), Space.World);
            }

            elapsedTime += Time.deltaTime;
            if (elapsedTime >= _rotationDuration)
            {
                CleanUpRotation();
                UpdateVisibleSide();
                EndRotation();
            }
        }

        /// <summary>
        /// Ends collecting small cubes.
        /// </summary>
        private void EndCollecting()
        {
            Collecting = false;
            SetSmallCubeCoords();
            SelectVisibleSmallCubes();
            GameManager.Instance.EndCollecting();
        }

        /// <summary>
        /// After collecting selected small cubes,
        /// moves the remaining ones and generates new ones.
        /// </summary>
        private void MoveAndGenerateSmallCubes()
        {
            float ratio = elapsedTime / _collectionDuration;
            bool done = ratio >= 1f;
            elapsedTime += Time.deltaTime;

            foreach (SmallCube sm in _smallCubes)
            {
                if (sm.Collected)
                {
                    // Transparency
                    sm.SetTransparency(ratio);

                    if (done)
                    {
                        sm.Regenerate();
                    }
                }

                // Position
                sm.transform.position = Vector3.Lerp(
                    sm.OldPosition,
                    sm.OldPosition + new Vector3(0, 0, _smallCubeSize),
                    ratio);

                if (done)
                {
                    sm.transform.position = sm.OldPosition + new Vector3(0, 0, _smallCubeSize);
                }
            }

            if (done)
            {
                EndCollecting();
            }
        }

        /// <summary>
        /// Restores the cube's rotation to straight angles.
        /// </summary>
        private void CleanUpRotation()
        {
            Vector3 rotation = transform.rotation.eulerAngles;

            if (rotation.x % 90 >= 45)
            {
                rotation.x = (int) (rotation.x / 90 + 1) * 90;
            }
            else
            {
                rotation.x = (int) (rotation.x / 90) * 90;
            }

            if (rotation.y % 90 >= 45)
            {
                rotation.y = (int) (rotation.y / 90 + 1) * 90;
            }
            else
            {
                rotation.y = (int) (rotation.y / 90) * 90;
            }

            if (rotation.z % 90 >= 45)
            {
                rotation.z = (int) (rotation.z / 90 + 1) * 90;
            }
            else
            {
                rotation.z = (int) (rotation.z / 90) * 90;
            }

            transform.rotation = Quaternion.Euler(rotation);
        }

        /// <summary>
        /// Updates which side is visible.
        /// </summary>
        private void UpdateVisibleSide()
        {
            _visibleSide = Side.None;

            // The array is in the same order as the Side enum
            Vector3[] directions = new Vector3[6];
            directions[0] = transform.forward;
            directions[1] = transform.forward * -1;
            directions[3] = transform.right * -1;
            directions[2] = transform.right;
            directions[4] = transform.up;
            directions[5] = transform.up * -1;

            for (int i = 0; i < directions.Length; i++)
            {
                Ray ray = new Ray(transform.position, directions[i]);
                bool raycastHit = Physics.Raycast(ray, 5, _visibleSideMask);
                if (raycastHit)
                {
                    // The index is cast as Side and
                    // the result is the visible side
                    _visibleSide = (Side) i;
                    Debug.Log("Visible side: " + _visibleSide);
                    break;
                }
            }

            if (_visibleSide == Side.None)
            {
                Debug.LogError("Couldn't find the visible side.");
            }
        }

        /// <summary>
        /// Rotates the cube based on the input direction.
        /// </summary>
        public void RotateInput(Vector3 direction)
        {
            if (Rotating || Collecting)
            {
                return;
            }

            _inputDirection = direction;
            StartRotation();
        }

        public void CollectSmallCubesInput()
        {
            if (Rotating || Collecting)
            {
                return;
            }

            Collecting = true;
            elapsedTime = 0;

            foreach (SmallCube sm in _smallCubes)
            {
                if (sm.Selected)
                {
                    sm.Collect();
                    Vector3 newPosition = sm.transform.position + new Vector3(0, 0, -4 * _smallCubeSize);
                    sm.transform.position = newPosition;
                    sm.SetType(GameManager.Instance.GetRandomSmallCubeType());
                }

                sm.OldPosition = sm.transform.position;
            }
        }

        private void OnDrawGizmos()
        {
            // Line towards camera
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + _cameraDirection * 10);

            // Line on the front face of the cube
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5);
        }
    }
}
