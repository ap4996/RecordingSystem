using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class ColorSystem : MonoBehaviour, IPointerClickHandler
{
    private Image image;

    [SerializeField]
    private ColorName currentColor;

    [Inject]
    private ColorData colorData;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        SetInitialColor();
    }

    private void SetInitialColor()
    {
        ColorTemplate ct = colorData.GetRandomColorTemplate();
        currentColor = ct.colorName;
        image.color = ct.currentColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            ChangeColor();
        }
    }

    private void ChangeColor()
    {
        ColorTemplate ct = colorData.GetColorTemplate(currentColor);
        image.color = ct.nextColor;
        currentColor = ct.nextColorName;
    }
}