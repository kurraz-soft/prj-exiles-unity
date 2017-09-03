using System.Collections;
using scripts.UI;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text AlertText = null;

    public PeasantModal peasantModal;
    public Popup popup;

    private static UIController _instance;
    private IEnumerator _alertCoroutine;

    public static UIController Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<UIController>();
                if(!_instance) Debug.LogError("Can't find UIController object");
            }
            return _instance;
        }
    }

    public void ShowAlert(string text)
    {
        AlertText.text = text;
        AlertText.gameObject.SetActive(true);

        if(_alertCoroutine != null)
            StopCoroutine(_alertCoroutine);
        _alertCoroutine = AlertFadeOut(2.0f);
        StartCoroutine(_alertCoroutine);
    }

    private IEnumerator AlertFadeOut(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        AlertText.gameObject.SetActive(false);
    }
}
