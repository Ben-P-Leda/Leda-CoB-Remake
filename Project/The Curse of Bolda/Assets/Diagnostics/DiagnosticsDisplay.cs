using UnityEngine;
using System.Collections.Generic;

namespace Diagnostics
{
    public class DiagnosticsDisplay : MonoBehaviour
    {
        private static DiagnosticsDisplay _instance;
        public static void SetDiagnostic(string key, string value) { _instance.SetDiagnosticString(key, value); }

        private Dictionary<string, string> _diagnostics;
        private GUIStyle _font;

        private void Awake()
        {
            _instance = this;
            _diagnostics = new Dictionary<string, string>();
        }

        private void Start()
        {
            _font = new GUIStyle();
            _font.fontStyle = FontStyle.Bold;
            _font.fontSize = 12;
            _font.normal.textColor = Color.black;
        }

        private void OnGUI()
        {
            Rect container = new Rect(0, 200, 1000, 15);
            foreach (KeyValuePair<string,string> kvp in _diagnostics)
            {
                GUI.Label(container, kvp.Value, _font);
                container.y += 15;
            }
        }


        private void SetDiagnosticString(string key, string value)
        {
            if (!_diagnostics.ContainsKey(key)) { _diagnostics.Add(key, ""); }
            _diagnostics[key] = value;
        }
    }
}