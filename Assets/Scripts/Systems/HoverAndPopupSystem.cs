using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class HoverAndPopupSystem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject tooltipGO;
    public TextMeshProUGUI tooltipText;
    public Button button;

    private bool showTooltip;

    [SerializeField]
    private ButtonShape buttonShape;

    #region Zenject

    [Inject]
    private TextData textData;

    private SignalBus _signalBus;

    [Inject]
    private void InjectSignalBus(SignalBus signalBus)
    {
        _signalBus = signalBus;
        _signalBus.Subscribe<PlayRecording>(PlayRecording);
        _signalBus.Subscribe<HideTooltipSignal>(HideTooltip);
    }

    private void PlayRecording(PlayRecording signal)
    {
        if (signal.record.buttonName != gameObject.name) return;

        if(signal.record.inputType == InputType.ShowTooltip)
        {
            ShowTooltip();
        }
        if(signal.record.inputType == InputType.HideTooltip)
        {
            showTooltip = true;
            HideTooltip();
        }
        if(signal.record.inputType == InputType.PopupOpened)
        {
            PopupOpenClick();
        }
    }
    private void HideTooltip()
    {
        if (showTooltip)
        {
            showTooltip = false;
            tooltipGO.SetActive(false);
            _signalBus.Fire(new RecordInputSignal
            {
                buttonName = gameObject.name,
                inputType = InputType.HideTooltip
            });
        }
    }
    #endregion

    #region IPointer implementation
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!showTooltip)
        {
            showTooltip = true;
            WaitAndShowTooltip();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && !eventData.dragging)
        {
            PopupOpenClick();
        }
    }
    #endregion

    #region Object Manipulators

    private async void WaitAndShowTooltip()
    {
        await UniTask.Delay(500);
        if (showTooltip)
        {
            ShowTooltip();
        }
    }

    private void ShowTooltip()
    {
        tooltipText.text = textData.GetTooltipText(buttonShape);
        tooltipGO.SetActive(true);
        _signalBus.Fire(new RecordInputSignal
        {
            buttonName = gameObject.name,
            inputType = InputType.ShowTooltip
        });
    }

    private void PopupOpenClick()
    {
        _signalBus.Fire(new RecordInputSignal
        {
            inputType = InputType.PopupOpened,
            buttonName = gameObject.name
        });
        _signalBus.Fire(new OpenPopup
        {
            popupContent = textData.GetPopupText(buttonShape)
        });
    }

    #endregion
}
