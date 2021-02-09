using Data;
using Entities.Types;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Components.Compendium
{
    public class TypeButtonController : MonoBehaviour
    {
        public delegate void SelectDelegate(GoblinType type);

        public event SelectDelegate OnSelect;
        
        public Button button;
        public TMP_Text title;
        public Image indicator;
        
        public GoblinType type;
        
        public void SetData(GoblinType type)
        {
            this.type = type;
            var definition = GoblinTypesConfig.GetDefinition(type);
            title.text = type.ToString();
            indicator.color = definition.color;
        }

        public void SetActive(bool active)
        {
            button.interactable = !active;
        }

        public void OnClick()
        {
            OnSelect?.Invoke(type);
        }
    }
}