using UnityEngine;

namespace scripts.UI
{
    class PopupRenderer : MonoBehaviour
    {
        [TextArea(3, 10)]
        public string text;
        private RectTransform rectTransform;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnMouseEnter()
        {
            UIController.Instance.popup.Show(text, new Vector2(transform.position.x + rectTransform.rect.width, transform.position.y));
        }

        public void OnMouseExit()
        {
            UIController.Instance.popup.Hide();
        }
    }
}
