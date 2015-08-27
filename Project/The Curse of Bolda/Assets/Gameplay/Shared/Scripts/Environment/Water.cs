using UnityEngine;

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

        private int _edgeCount;
        private int _nodeCount { get { return _edgeCount + 1; } }

        private LineRenderer _topLine;

        public float Left;
        public float Width;
        public float Top;
        public float Bottom;

        public GameObject WaterMesh;

        private void Awake()
        {
            _edgeCount = Mathf.RoundToInt(Width) * Edges_Per_Unity_Unit;

            InitializeContainerArrays();
            InitializeTopLine();
            InitializeMeshes();
        }

        private void InitializeContainerArrays()
        {
            _xPositions = new float[_nodeCount];
            _yPositions = new float[_nodeCount];
            _velocities = new float[_nodeCount];
            _accelerations = new float[_nodeCount];

            for (int i = 0; i < _nodeCount; i++)
            {
                _yPositions[i] = Top;
                _xPositions[i] = Left + Width * i / _edgeCount;
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
                _colliders[i].tag = "Water Pool";
                _colliders[i].AddComponent<BoxCollider2D>();
                _colliders[i].transform.parent = transform;
                _colliders[i].transform.position = new Vector3(Left + Width * (i + 0.5f) / _edgeCount, Top - 0.5f, 0);
                _colliders[i].transform.localScale = new Vector3(Width / _edgeCount, 1, 1);
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
            Vertices[2] = new Vector3(_xPositions[i], Bottom, 0.0f);
            Vertices[3] = new Vector3(_xPositions[i + 1], Bottom, 0.0f);

            _meshes[i].vertices = Vertices;
        }

        public void Splash(float xPosition, float velocity)
        {
            if (xPosition >= _xPositions[0] && xPosition <= _xPositions[_xPositions.Length - 1])
            {
                xPosition -= _xPositions[0];

                int index = Mathf.RoundToInt((_xPositions.Length - 1) * (xPosition / (_xPositions[_xPositions.Length - 1] - _xPositions[0])));

                _velocities[index] += velocity;
            }
        }

        //Called regularly by Unity
        private void FixedUpdate()
        {
            //Here we use the Euler method to handle all the physics of our springs:
            for (int i = 0; i < _xPositions.Length; i++)
            {
                float force = Spring_Constant * (_yPositions[i] - Top) + _velocities[i] * Damping_Modifier;
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
        private const float Spring_Constant = 0.02f;
        private const float Damping_Modifier = 0.02f;
        private const float Spread_Modifier = 0.05f;
    }

    public class WaterDetector : MonoBehaviour
    {
        private Water _water;

        private void Awake()
        {
            _water = transform.parent.GetComponent<Water>();
        }

        private void OnTriggerEnter2D(Collider2D Hit)
        {
            Rigidbody2D rigidBody2D = Hit.GetComponent<Rigidbody2D>();

            if (rigidBody2D != null) { _water.Splash(transform.position.x, (rigidBody2D.velocity.y * rigidBody2D.mass) / Mass_Impact_Modifier); }
        }

        private const float Mass_Impact_Modifier = 80.0f;
    }
}
