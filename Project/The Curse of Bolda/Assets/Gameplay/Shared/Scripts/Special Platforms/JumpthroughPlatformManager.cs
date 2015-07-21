using UnityEngine;

namespace Gameplay.Shared.Scripts.Special_Platforms
{
    public class JumpthroughPlatformManager : MonoBehaviour
    {
        private JumpthroughPlatformContainer[] _platforms;
        private Transform _playerTransform;

        protected JumpthroughPlatformContainer[] Platforms { get { return _platforms; } }

        public GameObject Player;
        public GameObject TileMap;
        public string MapLayerName;

        protected virtual void Awake()
        {
            Transform layer = TileMap.transform.FindChild(MapLayerName);

            _platforms = new JumpthroughPlatformContainer[layer.childCount];
            for (int i = 0; i < layer.childCount; i++)
            {
                Transform platformTransform = layer.GetChild(i);
                _platforms[i] = new JumpthroughPlatformContainer(platformTransform, platformTransform.gameObject);
            }

            _playerTransform = Player.transform;
        }

        private void Update()
        {
            for (int i = 0; i < _platforms.Length; i++)
            {
                if (_playerTransform.position.y - Jumpthru_Offset > _platforms[i].Transform.position.y)
                {
                    _platforms[i].GameObject.layer = Below_Player_Sorting_Layer;
                }
                else
                {
                    _platforms[i].GameObject.layer = Above_Player_Sorting_Layer;
                }
            }
        }

        private const float Jumpthru_Offset = 0.175f;
        private const int Above_Player_Sorting_Layer = 9;
        private const int Below_Player_Sorting_Layer = 8;
    }
}