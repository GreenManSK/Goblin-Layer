using Data;
using Objects.Golbin;
using TMPro;
using UnityEngine;

namespace UI.Components.Date
{
    public class TypeBarItemController : MonoBehaviour
    {
        public TMP_Text text;

        public void SetData(GoblinController data, float fontSize)
        {
            var type = GoblinTypesConfig.GetDefinition(data.type);
            text.text = data.type.ToString();
            text.color = type.color;
            text.fontSize = fontSize;
        }
    }
}