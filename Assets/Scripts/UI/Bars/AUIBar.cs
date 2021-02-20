using UnityEngine;
using UnityEngine.UI;

namespace UI.Bars
{
    public abstract class AUIBar : MonoBehaviour
    {
        public RectTransform bar;

        public GameObject icon;
        public Image barSprite;

        public void SetVisibility(bool visible)
        {
            var color = barSprite.color;
            color.a = visible ? 1 : 0;
            barSprite.color = color;
            icon.SetActive(visible);
        }

        protected void UpdateScale(float y)
        {
            var localScale = bar.localScale;
            bar.localScale = new Vector3(localScale.x, y, localScale.z);
        }
    }
}