using TMPro;
using UnityEngine;

namespace UI.Components.Date
{
    public class ActionBarController : MonoBehaviour
    {
        public TMP_Text actionsText;

        public void SetActions(int available, int maximum)
        {
            actionsText.text = $"{available}/{maximum}";
        }
    }
}