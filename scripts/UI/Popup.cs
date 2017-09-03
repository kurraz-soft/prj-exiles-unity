using UnityEngine;
using UnityEngine.UI;

namespace scripts.UI
{
    public class Popup : MonoBehaviour
    {
        public Text textElement;

        public void Show(string text, Vector2 pos)
        {
            textElement.text = text;
            transform.position = pos;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
