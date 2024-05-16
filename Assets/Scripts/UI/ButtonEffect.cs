using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    private Vector2 upPos;
    private Vector2 downPos;

    [SerializeField] private Sprite buttonUpImage;
    [SerializeField] private Sprite buttonDownImage;
    [SerializeField] private Vector3 offset = new Vector2(0, -4);

    public RectTransform buttonText;
 
    private void Awake()
    {
        image = GetComponent<Image>();
        upPos = buttonText.localPosition;
        downPos = buttonText.localPosition + offset;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = buttonDownImage;
        buttonText.localPosition = downPos;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = buttonUpImage;
        buttonText.localPosition = upPos;
    }
}
