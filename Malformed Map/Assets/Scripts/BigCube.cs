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
        private List<Material> _smallCubeTypes;

        [SerializeField]
        private List<Material> _smallCubeTypes_Rare;

        [SerializeField]
        private Side _visibleSide = Side.Front;

        [SerializeField]
        private Vector3 _cameraDirection = Vector3.forward;

        [SerializeField]
        private float _rotationDuration;

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

        private SmallCube[] _smallCubes;

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
            //_rotationDir = transform.rotation.eulerAngles;
            //_oldRotation = transform.rotation;
        }

        /// <summary>
        /// Initializes the small cubes.
        /// </summary>
        private void InitSmallCubes()
        {
            _smallCubes = transform.GetComponentsInChildren<SmallCube>();

            foreach (SmallCube sm in _smallCubes)
            {
                Material material = _smallCubeTypes[0];

                float random = Random.Range(0, 2.5f);
                if (random < 2f)
                {
                    material = _smallCubeTypes[(int) random];
                }
                else
                {
                    random = Random.Range(0, 2);
                    if (random == 2f)
                    {
                        random = 0f;
                    }

                    material = _smallCubeTypes_Rare[(int) random];
                }

                sm.SetMaterial(material);
            }

            UpdateSmallCubes();
        }

        /// <summary>
        /// Updates the small cubes.
        /// </summary>
        private void UpdateSmallCubes()
        {
            // TODO
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
        }

        /// <summary>
        /// Is the cube rotating.
        /// </summary>
        private bool Rotating { get; set; }

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
            //_oldRotation = transform.rotation;
            UpdateSmallCubes();
        }

        /// <summary>
        /// Rotates the cube.
        /// </summary>
        private void Rotate()
        {
            float ratio = elapsedTime / _rotationDuration;
            //transform.rotation = Quaternion.Lerp(_oldRotation, _targetRotation, ratio);

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
                //transform.rotation = _targetRotation;
                CleanUpRotation();
                UpdateVisibleSide();
                EndRotation();
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
        /// Updates the visible side.
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
        /// Sets the target rotation based on the given side.
        /// </summary>
        /// <param name="side">The target side</param>
        /// <returns>A Quaternion rotation</returns>
        private void SetNewTargetRotation(Side side)
        {
            Vector3 direction = Utils.SideToVector3(side);
            //_targetRotation = Quaternion.LookRotation(direction - transform.position);
        }

        /// <summary>
        /// Sets the target rotation based on the given direction
        /// and which way the cube is facing.
        /// </summary>
        /// <param name="direction">The rotation direction</param>
        /// <returns>A Quaternion rotation</returns>
        //private void SetNewTargetRotation(Vector3 direction)
        //{
        //    // TODO: Fix rotating in wrong directions and getting offset from straight angles

        //    Vector3 frontAxis = transform.worldToLocalMatrix * _cameraDirection;
        //    topAxis = transform.worldToLocalMatrix * Vector3.up;
        //    rightAxis = transform.worldToLocalMatrix * Vector3.right;

        //    if (direction.x != 0)
        //    {
        //        int sign = (direction.x > 0 ? 1 : -1);
        //        _rotationDir += Quaternion.AngleAxis(sign * 90, topAxis).eulerAngles;
        //        _targetRotation = Quaternion.Euler(_rotationDir);
        //        Debug.Log("Rotating " + (sign > 0 ? "Right" : "Left"));
        //        Debug.Log("AXIS: " + topAxis);
        //    }
        //    else if (direction.y != 0)
        //    {
        //        int sign = (direction.y > 0 ? 1 : -1);
        //        _rotationDir += Quaternion.AngleAxis(sign * 90, rightAxis).eulerAngles;
        //        _targetRotation = Quaternion.Euler(_rotationDir);
        //        Debug.Log("Rotating " + (sign > 0 ? "Up" : "Down"));
        //        Debug.Log("AXIS: " + rightAxis);
        //    }
        //}

        /// <summary>
        /// Rotates the cube based on the input direction.
        /// </summary>
        public void MovementInput(Vector3 direction)
        {
            if (Rotating)
            {
                return;
            }

            _inputDirection = direction;
            StartRotation();
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
