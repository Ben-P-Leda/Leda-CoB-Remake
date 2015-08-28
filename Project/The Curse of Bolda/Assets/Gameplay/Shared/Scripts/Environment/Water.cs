using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Environment
{
    public class Water : MonoBehaviour
    {
        private float[] _xPositions;
        private float[] _yPositions;
        private float[] _velocities;
        private float[] _accelerations;
        private GameObject[] _colliders;
        private GameObject[] _meshObjects;
        private Mesh[] _meshes;

        private float _left;
        private float _width;
        private float _top;
        private float _bottom;

        private int _edgeCount;
        private int _nodeCount { get { return _edgeCount + 1; } }

        private LineRenderer _topLine;
        private GameObject _jetpackBlocker;
        private GameObject _playerDeathTrigger;

        public GameObject WaterMesh;

        private void Awake()
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetMetricsFromTransform();

            _edgeCount = Mathf.RoundToInt(_width) * Edges_Per_Unity_Unit;

            InitializeContainerArrays();
            InitializeTopLine();
            InitializeMeshes();
            InitializeInteractionColliders();
        }

        private void GetMetricsFromTransform()
        {
            _left = transform.position.x - ((transform.localScale.x * 0.5f) * Transform_To_Mesh_Size_Modifier);
            _width = transform.localScale.x  * Transform_To_Mesh_Size_Modifier;
            _top = transform.position.y + ((transform.localScale.y * 0.5f) * Transform_To_Mesh_Size_Modifier);
            _bottom = transform.position.y - ((transform.localScale.y * 0.5f) * Transform_To_Mesh_Size_Modifier);
        }

        private void InitializeContainerArrays()
        {
            _xPositions = new float[_nodeCount];
            _yPositions = new float[_nodeCount];
            _velocities = new float[_nodeCount];
            _accelerations = new float[_nodeCount];

            for (int i = 0; i < _nodeCount; i++)
            {
                _yPositions[i] = _top;
                _xPositions[i] = _left + _width * i / _edgeCount;
                _accelerations[i] = 0;
                _velocities[i] = 0;
            }

            _colliders = new GameObject[_edgeCount];
            _meshObjects = new GameObject[_edgeCount];
            _meshes = new Mesh[_edgeCount];
        }

        private void InitializeTopLine()
        {
            _topLine = gameObject.GetComponent<LineRenderer>();
            _topLine.sortingLayerName = "Effects";
            _topLine.sortingOrder = 100;
            _topLine.SetVertexCount(_nodeCount);

            for (int i = 0; i < _nodeCount; i++) { _topLine.SetPosition(i, new Vector3(_xPositions[i], _yPositions[i], 0)); }
        }

        private void InitializeMeshes()
        {
            Vector2[] UVs = new Vector2[4];
            UVs[0] = new Vector2(0, 1);
            UVs[1] = new Vector2(1, 1);
            UVs[2] = new Vector2(0, 0);
            UVs[3] = new Vector2(1, 0);

            int[] tris = new int[6] { 0, 1, 3, 3, 2, 0 };

            for (int i = 0; i < _edgeCount; i++)
            {
                _meshes[i] = new Mesh();

                UpdateMesh(i);

                _meshes[i].uv = UVs;
                _meshes[i].triangles = tris;

                _meshObjects[i] = Instantiate(WaterMesh, Vector3.zero, Quaternion.identity) as GameObject;
                _meshObjects[i].GetComponent<MeshRenderer>().sortingLayerName = "Effects";
                _meshObjects[i].GetComponent<MeshFilter>().mesh = _meshes[i];
                _meshObjects[i].transform.parent = transform;

                _colliders[i] = new GameObject();
                _colliders[i].name = "Trigger";
                _colliders[i].tag = "Water Surface";
                _colliders[i].AddComponent<BoxCollider2D>();
                _colliders[i].transform.parent = _meshObjects[i].transform;
                _colliders[i].transform.position = new Vector3(_left + _width * (i + 0.5f) / _edgeCount, _top - 0.5f, 0);
                _colliders[i].transform.localScale = new Vector3(_width / _edgeCount, 1, 1);
                _colliders[i].GetComponent<BoxCollider2D>().isTrigger = true;
                _colliders[i].AddComponent<WaterDetector>();
            }
        }

        private void UpdateMeshes()
        {
            for (int i = 0; i < _edgeCount; i++) { UpdateMesh(i); }
        }

        private void UpdateMesh(int i)
        {
            Vector3[] Vertices = new Vector3[4];
            Vertices[0] = new Vector3(_xPositions[i], _yPositions[i], 0.0f);
            Vertices[1] = new Vector3(_xPositions[i + 1], _yPositions[i + 1], 0.0f);
            Vertices[2] = new Vector3(_xPositions[i], _bottom, 0.0f);
            Vertices[3] = new Vector3(_xPositions[i + 1], _bottom, 0.0f);

            _meshes[i].vertices = Vertices;
        }

        private void InitializeInteractionColliders()
        {
            _jetpackBlocker = new GameObject();
            _jetpackBlocker.name = "Jetpack Blocker";
            _jetpackBlocker.tag = "Wall Collider";
            _jetpackBlocker.AddComponent<BoxCollider2D>();
            _jetpackBlocker.transform.parent = transform;
            _jetpackBlocker.transform.localPosition = new Vector3(transform.parent.localPosition.x, transform.parent.localPosition.y + ((transform.parent.localScale.y * 0.5f) * Transform_To_Mesh_Size_Modifier), 0);
            _jetpackBlocker.transform.localScale = new Vector3(transform.parent.localScale.x * Transform_To_Mesh_Size_Modifier, 0.2f, 1);

            _playerDeathTrigger = new GameObject();
            _playerDeathTrigger.name = "Death Trigger";
            _playerDeathTrigger.tag = "Water Pool";
            _playerDeathTrigger.AddComponent<BoxCollider2D>();
            _playerDeathTrigger.GetComponent<BoxCollider2D>().isTrigger = true;
            _playerDeathTrigger.transform.parent = transform;
            _playerDeathTrigger.transform.localPosition = new Vector3(transform.parent.localPosition.x, transform.parent.localPosition.y, 0);
            _playerDeathTrigger.transform.localScale = new Vector3(transform.parent.localScale.x * Transform_To_Mesh_Size_Modifier, 0.2f, 1);
        }

        public void Splash(float xPosition, float velocity)
        {
            if (xPosition >= _xPositions[0] && xPosition <= _xPositions[_xPositions.Length - 1])
            {
                velocity = Mathf.Clamp(velocity, -0.45f, 0.0f);

                xPosition -= _xPositions[0];

                int index = Mathf.RoundToInt((_xPositions.Length - 1) * (xPosition / (_xPositions[_xPositions.Length - 1] - _xPositions[0])));

                _velocities[index] += velocity;
            }
        }

        private void Update()
        {
            if (CurrentGame.GameData.ActiveTool == ToolType.Jetpack) { _jetpackBlocker.layer = Constants.Affect_Player_Impact_Sorting_Layer; }
            else { _jetpackBlocker.layer = Constants.Ignore_Player_Impact_Sorting_Layer; }
        }

        //Called regularly by Unity
        private void FixedUpdate()
        {
            //Here we use the Euler method to handle all the physics of our springs:
            for (int i = 0; i < _xPositions.Length; i++)
            {
                float force = Spring_Constant * (_yPositions[i] - _top) + _velocities[i] * Damping_Modifier;
                _accelerations[i] = -force;
                _yPositions[i] += _velocities[i];
                _velocities[i] += _accelerations[i];
                _topLine.SetPosition(i, new Vector3(_xPositions[i], _yPositions[i], 0.0f));
            }

            //Now we store the difference in heights:
            float[] leftDeltas = new float[_xPositions.Length];
            float[] rightDeltas = new float[_xPositions.Length];

            //We make 8 small passes for fluidity:
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < _nodeCount; i++)
                {
                    //We check the heights of the nearby nodes, adjust velocities accordingly, record the height differences
                    if (i > 0)
                    {
                        leftDeltas[i] = Spread_Modifier * (_yPositions[i] - _yPositions[i - 1]);
                        _velocities[i - 1] += leftDeltas[i];
                    }
                    if (i < _nodeCount - 1)
                    {
                        rightDeltas[i] = Spread_Modifier * (_yPositions[i] - _yPositions[i + 1]);
                        _velocities[i + 1] += rightDeltas[i];
                    }
                }

                //Now we apply a difference in position
                for (int i = 0; i < _nodeCount; i++)
                {
                    if (i > 0) { _yPositions[i - 1] += leftDeltas[i]; }
                    if (i < _xPositions.Length - 1) { _yPositions[i + 1] += rightDeltas[i]; }
                }
            }
            //Finally we update the meshes to reflect this
            UpdateMeshes();
        }

        private const int Edges_Per_Unity_Unit = 5;
        private const float Transform_To_Mesh_Size_Modifier = 0.639f;
        private const float Spring_Constant = 0.02f;
        private const float Damping_Modifier = 0.05f;
        private const float Spread_Modifier = 0.05f;
    }

    public class WaterDetector : MonoBehaviour
    {
        private Water _water;

        private void Awake()
        {
            _water = transform.parent.parent.GetComponent<Water>();
        }

        private void OnTriggerEnter2D(Collider2D Hit)
        {
            Rigidbody2D rigidBody2D = Hit.GetComponent<Rigidbody2D>();

            if (rigidBody2D != null) { _water.Splash(transform.position.x, (rigidBody2D.velocity.y * rigidBody2D.mass) / Mass_Impact_Modifier); }
        }

        private const float Mass_Impact_Modifier = 80.0f;
    }
}
