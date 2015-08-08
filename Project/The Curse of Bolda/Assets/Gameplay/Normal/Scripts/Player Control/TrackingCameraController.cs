using UnityEngine;

namespace Gameplay.Normal.Scripts.Player_Control
{
    public class TrackingCameraController : MonoBehaviour
    {
        private Transform _transform;
        private Transform _objectToTrack;
        private Transform _bgTransform;
        private Rect _cameraMovementArea;
        private Vector2 _levelCenter;
        private Vector2 _bgMovementExtent;

        public GameObject ObjectToTrack;
        public GameObject LevelMap;
        public GameObject ParalaxBackground;

        private void Awake()
        {
            _transform = transform;
            _objectToTrack = ObjectToTrack.transform;
            _bgTransform = ParalaxBackground.transform;
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

            _levelCenter = new Vector2(
                LevelMap.transform.position.x + (mapBoundaries.width * 0.5f),
                LevelMap.transform.position.y + (mapBoundaries.height * 0.5f));

            SpriteRenderer bgSpriteRenderer = ParalaxBackground.GetComponent<SpriteRenderer>();
            _bgMovementExtent = new Vector2(
                bgSpriteRenderer.sprite.bounds.extents.x - cameraMargins.x,
                bgSpriteRenderer.sprite.bounds.extents.y - cameraMargins.y);
        }

        private void Update()
        {
            _transform.position = new Vector3(
                Mathf.Clamp(_objectToTrack.position.x, _cameraMovementArea.xMin, _cameraMovementArea.xMax),
                Mathf.Clamp(_objectToTrack.position.y, _cameraMovementArea.yMin, _cameraMovementArea.yMax),
                _transform.position.z);

            Vector2 offset = new Vector2(
                (_transform.position.x - _cameraMovementArea.center.x) / (_cameraMovementArea.width * 0.5f),
                (_transform.position.y - _cameraMovementArea.center.y) / (_cameraMovementArea.width * 0.5f));

            _bgTransform.position = new Vector3(
                _transform.position.x - (_bgMovementExtent.x * offset.x),
                _transform.position.y - (_bgMovementExtent.y * offset.y),  
                _bgTransform.position.z);
        }
    }
}