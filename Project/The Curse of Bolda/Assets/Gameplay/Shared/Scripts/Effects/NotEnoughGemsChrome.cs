using UnityEngine;

namespace Gameplay.Shared.Scripts.Effects
{
    public class NotEnoughGemsChrome : MonoBehaviour
    {
        private Transform _transform;
        private Animator _animator;
        private GameObject _gameObject;
        private Transform _chromeObjectTransform;

        private void Awake()
        {
            _transform = transform;
            _animator = GetComponent<Animator>();
            _gameObject = gameObject;
            _chromeObjectTransform = _transform.FindChild("Not Enough Gems Chrome").transform;
        }

        private void Update()
        {
            if (!_animator.enabled)
            {
                _transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, _transform.position.z);
                _chromeObjectTransform.localPosition = Vector3.zero;
                _animator.enabled = true;

                if (Input.anyKeyDown) { _animator.enabled = true; }
            }
        }

        public void CompleteExitAnimation()
        {
            _animator.enabled = false;
            _gameObject.SetActive(false);

        }
    }
}
