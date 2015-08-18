using UnityEngine;

using System.Collections.Generic;

using Gameplay.Shared.Scripts.Environment;

namespace Gameplay.Normal.Scripts.Player_Control
{
    public class TrackingCameraController : MonoBehaviour
    {
        private Transform _transform;
        private Transform _objectToTrack;
        private Transform _bgTransform;
        private Rect _cameraMovementArea;
        private Vector2 _bgMovementExtent;
        private BackgroundLayer[] _backgroundLayers;

        public GameObject ObjectToTrack;
        public GameObject LevelMap;
        public List<GameObject> BackgroundLayers;

        private void Awake()
        {
            _transform = transform;
            _objectToTrack = ObjectToTrack.transform;

            _backgroundLayers = new BackgroundLayer[BackgroundLayers.Count];
            for (int i = 0; i < BackgroundLayers.Count; i++) { _backgroundLayers[i] = BackgroundLayers[i].GetComponent<BackgroundLayer>(); }
        }

        private void Start()
        {
            Vector3 cameraMargins = (Camera.main.ViewportToWorldPoint(Vector3.one) - Camera.main.ViewportToWorldPoint(Vector3.zero)) * 0.5f;
            Rect mapBoundaries = LevelMap.GetComponent<TileEditor.TileMap>().Boundary;

            _cameraMovementArea = new Rect(
                Mathf.Min(LevelMap.transform.position.x + cameraMargins.x, cameraMargins.x),
                Mathf.Min(LevelMap.transform.position.y + cameraMargins.y, cameraMargins.y),
                Mathf.Max(mapBoundaries.width - (cameraMargins.x * 2.0f), 0),
                Mathf.Max(mapBoundaries.height - (cameraMargins.y * 2.0f), 0));

            for (int i = 0; i < _backgroundLayers.Length; i++) { _backgroundLayers[i].CalculateMovementExtent(cameraMargins); }
        }

        private void FixedUpdate()
        {
            _transform.position = new Vector3(
                Mathf.Clamp(_objectToTrack.position.x, _cameraMovementArea.xMin, _cameraMovementArea.xMax),
                Mathf.Clamp(_objectToTrack.position.y, _cameraMovementArea.yMin, _cameraMovementArea.yMax),
                _transform.position.z);

            Vector2 offset = new Vector2(
                (_transform.position.x - _cameraMovementArea.center.x) / (_cameraMovementArea.width * 0.5f),
                (_transform.position.y - _cameraMovementArea.center.y) / (_cameraMovementArea.height * 0.5f));

            for (int i = 0; i < _backgroundLayers.Length; i++) { _backgroundLayers[i].PositionRelativeToCamera(_transform.position, offset); }
        }
    }
}