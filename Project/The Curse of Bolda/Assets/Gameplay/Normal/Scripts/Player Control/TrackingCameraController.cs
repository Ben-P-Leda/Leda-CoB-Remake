using UnityEngine;

namespace Gameplay.Normal.Scripts.Player_Control
{
    public class TrackingCameraController : MonoBehaviour
    {
        private Transform _transform;
        private Transform _objectToTrack;
        private Rect _cameraMovementArea;

        public GameObject ObjectToTrack;
        public GameObject LevelMap;

        private void Awake()
        {
            _transform = transform;
            _objectToTrack = ObjectToTrack.transform;
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
        }

        private void Update()
        {
            _transform.position = new Vector3(
                Mathf.Clamp(_objectToTrack.position.x, _cameraMovementArea.xMin, _cameraMovementArea.xMax),
                Mathf.Clamp(_objectToTrack.position.y, _cameraMovementArea.yMin, _cameraMovementArea.yMax),
                _transform.position.z);
        }
    }
}