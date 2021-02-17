using System;
using Constants;
using UnityEngine;
using UnityEngine.Events;

namespace Objects.Enviroment
{
    public enum TrapdoorState
    {
        Open,
        Close
    }

    public class TrapdoorController : MonoBehaviour
    {
        public UnityEvent triggers;
        
        public TrapdoorState state = TrapdoorState.Close;
        public SpriteRenderer spriteRenderer;
        public Collider2D collider;

        public Sprite openSprite;
        public Sprite closeSprite;

        private void Start()
        {
            SetState(state);
        }

        public void SetState(TrapdoorState state)
        {
            this.state = state;
            collider.enabled = state == TrapdoorState.Open;
            spriteRenderer.sprite = state == TrapdoorState.Close ? closeSprite : openSprite;
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                triggers?.Invoke();
            }
        }
    }
}