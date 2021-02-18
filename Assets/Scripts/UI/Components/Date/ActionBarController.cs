using TMPro;
using UnityEngine;

namespace UI.Components.Date
{
    public class ActionBarController : MonoBehaviour
    {
        public TMP_Text actionsText;
        public float fontSize;

        private bool _sizeComputed = false;

        private void ComputeFontSize()
        {
            actionsText.gameObject.SetActive(true);
            actionsText.enableAutoSizing = true;
            actionsText.ForceMeshUpdate();
            actionsText.enableAutoSizing = false;
            fontSize = actionsText.fontSize;
            _sizeComputed = true;
        }

        public void SetActions(int available, int maximum)
        {
            actionsText.text = $"Actions: {available}/{maximum}";
            if (!_sizeComputed)
            {
                ComputeFontSize();
            }
        }
    }
}