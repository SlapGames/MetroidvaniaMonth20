using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialMarker:MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Message { get => message; set => message = value; }
    
    [SerializeField] private GameObject explanationScreen;
    [SerializeField] private TMP_Text explanationText;
    [SerializeField, TextArea(5,10)] private string message;


    public void OnPointerEnter(PointerEventData eventData)
    {
        explanationScreen.SetActive(true);
        explanationText.text = message;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        explanationScreen.SetActive(false);
    }
}
