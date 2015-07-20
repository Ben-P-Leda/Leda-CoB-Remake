using UnityEngine;

namespace Gameplay.Shared.Scripts
{
    public class JumpthroughController : MonoBehaviour
    {
        private JumpthroughContainer[] _platforms;
        private Transform _playerTransform;
        private Rigidbody2D _playerRigidBody2D;

        public GameObject Player;
        public GameObject TileMap;

        private void Awake()
        {
            Transform layer = TileMap.transform.FindChild("Jump-Through Platforms");

            _platforms = new JumpthroughContainer[layer.childCount];
            for (int i = 0; i < layer.childCount; i++)
            {
                Transform platformTransform = layer.GetChild(i);
                _platforms[i] = new JumpthroughContainer(platformTransform, platformTransform.gameObject);
            }

            _playerTransform = Player.transform;
            _playerRigidBody2D = Player.GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            for (int i = 0; i < _platforms.Length; i++)
            {
                if (_playerTransform.position.y - Jumpthru_Offset - _playerRigidBody2D.velocity.y > _platforms[i].Transform.position.y)
                {
                    _platforms[i].GameObject.layer = Below_Player_Sorting_Layer;
                }
                else
                {
                    _platforms[i].GameObject.layer = Above_Player_Sorting_Layer;
                }

                Diagnostics.DiagnosticsDisplay.SetDiagnostic(
                    _platforms[i].GameObject.name, 
                    string.Format("{0}: {1} | {2}", 
                        _platforms[i].GameObject.name, 
                        _platforms[i].GameObject.layer,
                        _platforms[i].Transform.position.y - (_playerTransform.position.y - Jumpthru_Offset - _playerRigidBody2D.velocity.y)));
            }
        }

        private const float Jumpthru_Offset = 0.175f;
        private const int Above_Player_Sorting_Layer = 9;
        private const int Below_Player_Sorting_Layer = 8;

        private class JumpthroughContainer
        {
            public Transform Transform { get; private set; }
            public GameObject GameObject { get; private set; }

            public JumpthroughContainer(Transform transform, GameObject gameObject)
            {
                Transform = transform;
                GameObject = gameObject;
            }
        }
    }


}