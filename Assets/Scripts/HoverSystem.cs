using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class HoverSystem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltipGO;
    public TextMeshProUGUI tooltipText;

    private bool showTooltip;

    [SerializeField]
    private ButtonShape buttonShape;

    [Inject]
    private TextData textData;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"Enter");
        if(!showTooltip)
        {
            Debug.Log($"Starting Timer");
            showTooltip = true;
            WaitAndShowTooltip();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"Exit");
        showTooltip = false;
        tooltipGO.SetActive(false);
    }
    private async void WaitAndShowTooltip()
    {
        await UniTask.Delay(500);
        if (showTooltip)
        {
            tooltipText.text = textData.GetTooltipText(buttonShape);
            tooltipGO.SetActive(true);
        }
    }
}
