using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    public RectTransform buttonText;
    private Vector2 upPos;
    private Vector2 downPos;
    [SerializeField] private Vector3 offset = new Vector2(0, -4);
 
    private void Awake()
    {
        image = GetComponent<Image>();
        upPos = buttonText.localPosition;
        downPos = buttonText.localPosition + offset;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = UIHandler.Instance.buttonDownImage;
        buttonText.localPosition = downPos;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = UIHandler.Instance.buttonUpImage;
        buttonText.localPosition = upPos;
    }
}
