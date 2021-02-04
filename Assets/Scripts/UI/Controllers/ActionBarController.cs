using TMPro;
using UnityEngine;

namespace UI.Controllers
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