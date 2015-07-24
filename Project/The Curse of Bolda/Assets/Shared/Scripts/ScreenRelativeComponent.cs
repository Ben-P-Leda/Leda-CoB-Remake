using UnityEngine;

namespace Shared.Scripts
{
    public class ScreenRelativeComponent : MonoBehaviour
    {
        private float _scaling;

        protected float Scaling { get { return _scaling; } }

        protected virtual void Start()
        {
            _scaling = Screen.height / One_To_One_Screen_Height;
        }

        private const float One_To_One_Screen_Height = 675.0f;
    }
}
