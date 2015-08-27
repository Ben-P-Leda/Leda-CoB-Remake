using UnityEngine;

using System.Collections.Generic;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Special_Platforms
{
    public class JumpthroughPlatformManager : MonoBehaviour
    {
        private JumpthroughPlatformContainer[] _platforms;
        private Transform _playerTransform;
        private Rigidbody2D _playerRigidBody;

        protected JumpthroughPlatformContainer[] Platforms { get { return _platforms; } }

        public GameObject Player;
        public GameObject TileMap;
        public string MapLayerName;
        public List<GameObject> TileMapIndependentPlatforms;

        protected virtual void Awake()
        {
            Transform layer = TileMap.transform.FindChild(MapLayerName);

            _platforms = new JumpthroughPlatformContainer[layer.childCount + TileMapIndependentPlatforms.Count];

            for (int i = 0; i < layer.childCount; i++)
            {
                Transform platformTransform = layer.GetChild(i);
                _platforms[i] = new JumpthroughPlatformContainer(platformTransform, platformTransform.gameObject);
            }

            for (int i = 0; i < TileMapIndependentPlatforms.Count; i++ )
            {
                Transform platformTransform = TileMapIndependentPlatforms[i].transform;
                _platforms[layer.childCount + i] = new JumpthroughPlatformContainer(platformTransform, platformTransform.gameObject);
            }

            _playerTransform = Player.transform;
            _playerRigidBody = Player.GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (_playerRigidBody.velocity.y <= 0.0f)
            {
                for (int i = 0; i < _platforms.Length; i++)
                {
                    if (ShouldApplyPhysicsToPlayer(_platforms[i].Transform.position))
                    {
                        _platforms[i].GameObject.layer = Constants.Affect_Player_Impact_Sorting_Layer;
                    }
                    else
                    {
                        _platforms[i].GameObject.layer = Constants.Ignore_Player_Impact_Sorting_Layer;
                    }
                }
            }
        }

        private bool ShouldApplyPhysicsToPlayer(Vector3 platformPosition)
        {
            if (CurrentGame.GameData.ActiveTool == ToolType.Jetpack) { return true; }

            if (_playerTransform.position.y - Jumpthru_Offset <= platformPosition.y) { return false; }
            if (_playerTransform.position.x + Horizontal_Margin < platformPosition.x) { return false; }
            if (_playerTransform.position.x - Horizontal_Margin > platformPosition.x) { return false; }

            return true;
        }

        private const float Jumpthru_Offset = 0.365f;
        private const float Horizontal_Margin = 0.35f;
    }
}