using peasants;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PeasantModal : MonoBehaviour
    {
        public Button closeButton;

        public Text personNameText;
        public Text ageText;

        public void OnClickCloseButton()
        {
            gameObject.SetActive(false);
        }

        public void Show(Peasant p)
        {
            personNameText.text = p.personName;
            ageText.text = p.age.ToString();

            gameObject.SetActive(true);
        }
    }
}
