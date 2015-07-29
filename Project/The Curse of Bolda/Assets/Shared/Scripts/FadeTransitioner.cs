using UnityEngine;

namespace Shared.Scripts
{
    public class FadeTransitioner : MonoBehaviour
    {
        public delegate void TransitionCompletionCallback();

        private int _drawDepth;
        private float _alpha;
        private float _targetAlpha;
        private int _fadeDirection;
        private bool _transitionRunning;

        public Texture2D FadeTexture;
        public float TransitionSpeed;

        public TransitionCompletionCallback TransitionCompletionHandler { private get; set; }

        private void Awake()
        {
            _drawDepth = Draw_Depth;
            _transitionRunning = false;

            if (TransitionSpeed <= 0.0f) { TransitionSpeed = Default_Transition_Speed; }
        }

        public void FadeIn()
        {
            StartFade(-1);
        }

        public void FadeOut()
        {
            StartFade(1);
        }

        private void StartFade(int direction)
        {
            _alpha = (direction == 1 ? 0.0f : 1.0f);
            _targetAlpha = 1.0f - _alpha;
            _fadeDirection = direction;
            _transitionRunning = true;
        }

        private void Update()
        {
            if ((_transitionRunning) && (_alpha == _targetAlpha))
            {
                if (TransitionCompletionHandler != null) { TransitionCompletionHandler(); }
                TransitionCompletionHandler = null;
                _transitionRunning = false;
            }
        }

        private void OnGUI()
        {
            _alpha = Mathf.Clamp01(_alpha + (TransitionSpeed * _fadeDirection * Time.deltaTime));

            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, _alpha);
            GUI.depth = _drawDepth;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), FadeTexture);
        }

        private const int Draw_Depth = -1000;
        private const float Default_Transition_Speed = 1.2f;
    }
}