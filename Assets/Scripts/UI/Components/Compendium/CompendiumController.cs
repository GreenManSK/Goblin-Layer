using System.Collections.Generic;
using System.Linq;
using Constants;
using Data;
using Entities;
using Entities.Types;
using TMPro;
using UnityEngine;

namespace UI.Components.Compendium
{
    public class CompendiumController : MonoBehaviour
    {
        public GameObject typeButtonPrefab;
        public GameObject typeButtonsContainer;

        public TMP_Text title;
        public TMP_Text description;
        public TMP_Text likes;

        public HairSectionController hairSection;
        public CostumeSectionController costumeSection;
        public GlassesSectionController glassesSection;
        public AccessorySectionController accessorySection;

        private List<TypeButtonController> _typeButtons = new List<TypeButtonController>();

        private void Start()
        {
            PrepareTypeButtons();
        }

        private void OnDestroy()
        {
            _typeButtons.ForEach(b => b.OnSelect -= SelectType);
        }

        private void PrepareTypeButtons()
        {
            foreach (Transform child in typeButtonsContainer.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var type in Game.UnlockedTypes)
            {
                var go = Instantiate(typeButtonPrefab, typeButtonsContainer.transform);
                var button = go.GetComponent<TypeButtonController>();
                button.SetData(type);
                button.OnSelect += SelectType;
                _typeButtons.Add(button);
            }

            SelectType(Game.UnlockedTypes[0]);
        }

        private void SelectType(GoblinType type)
        {
            _typeButtons.ForEach(b => b.SetActive(b.type == type));
            var definition = GoblinTypesConfig.GetDefinition(type);

            title.text = type.ToString();
            description.text = definition.description ?? "Description placeholder";
            likes.text = $"<b>Likes:</b> {GetSeductionList(definition, true)}\n" +
                         $"<b>Dislikes:</b> {GetSeductionList(definition, false)}" +
                         (definition.envy > 0 ? "\nIs envious of other girls" : "");

            hairSection.SetItems(definition.hairs);
            
            var hasCostumes = definition.costumes.Length > 0;
            costumeSection.gameObject.SetActive(hasCostumes);
            costumeSection.SetItems(definition.costumes);

            var hasGlasses = definition.glasseses.Length > 0 && definition.glasseses.First() != Glasses.None;
            glassesSection.gameObject.SetActive(hasGlasses);
            glassesSection.SetItems(hasGlasses ? definition.glasseses : new Glasses[0]);

            var hasAccessory = definition.accessories.Count > 0;
            accessorySection.gameObject.SetActive(hasAccessory);
            accessorySection.SetItems(definition.accessories.Keys);
        }

        private string GetSeductionList(TypeDefinition definition, bool positive)
        {
            var seduction = new Dictionary<SeductionType, float>
            {
                {SeductionType.Compliment, definition.compliment},
                {SeductionType.Flirt, definition.flirt},
                {SeductionType.Insult, definition.insult},
                {SeductionType.Ask, definition.ask},
                {SeductionType.Attack, definition.attack},
                {SeductionType.AttackPlayer, definition.attackPlayer},
            };
            var texts = seduction
                .Where(item => ((positive ? 1 : -1) * item.Value) > 0)
                .Select(item => item.Key.Name()).ToList();
            return texts.Count > 0 ? string.Join(", ", texts) : "None";
        }
    }
}