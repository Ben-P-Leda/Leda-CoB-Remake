using UnityEngine;

using System.Collections.Generic;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Activators
{
    public class RestartPointManager : MonoBehaviour
    {
        private Transform[] _restartPoints;
        private Transform _playerTransform;

        public GameObject Player;

        private void Awake()
        {
            _restartPoints = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform restartPoint = transform.GetChild(i);
                restartPoint.gameObject.GetComponent<RestartPoint>().ActivationHandler = HandleRestartPointActivation;
                _restartPoints[i] = restartPoint;
            }

            _playerTransform = Player.transform;
        }

        private void HandleRestartPointActivation(string name)
        {
            for (int i=0; i<_restartPoints.Length; i++)
            {
                if (_restartPoints[i].name == name) { CurrentGame.GameData.RestartPoint = _playerTransform.position; }
                else { ((RestartPoint)_restartPoints[i].GetComponent<RestartPoint>()).Reset(); }
            }
        }
    }
}
