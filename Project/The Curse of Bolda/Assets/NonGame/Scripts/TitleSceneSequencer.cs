using UnityEngine;

using Shared.Scripts;

namespace NonGame.Scripts
{
    public class TitleSceneSequencer : MonoBehaviour
    {
        private FadeTransitioner _fadeTransitioner;

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
            CurrentGame.SetForNewGame();
            Application.LoadLevel("Level 1-1 Intro");
        }
    }
}