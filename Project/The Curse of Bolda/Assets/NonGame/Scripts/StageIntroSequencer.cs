using UnityEngine;

using Shared.Scripts;

namespace NonGame.Scripts
{
    public class StageIntroSequencer : MonoBehaviour
    {
        private FadeTransitioner _fadeTransitioner;

        public string GameplaySceneName;

        private void Awake()
        {
            _fadeTransitioner = GetComponent<FadeTransitioner>();
        }

        private void Start()
        {
            _fadeTransitioner.FadeIn();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                _fadeTransitioner.TransitionCompletionHandler = HandleFadeOutComplete;
                _fadeTransitioner.FadeOut();
            }
        }

        private void HandleFadeOutComplete()
        {
            Application.LoadLevel(GameplaySceneName);
        }
    }
}