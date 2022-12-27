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
    private SignalBus _signalBus;
    
    [Inject]
    private void InjectSignalBus(SignalBus signalBus)
    {
        _signalBus = signalBus;
        _signalBus.Subscribe<RecordInitialStateOfButtons>(SendInitialColorForRecording);
        _signalBus.Subscribe<PlayRecording>(PlayRecording);
    }

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        SetInitialColor(colorData.GetRandomColorTemplate());
    }

    private void PlayRecording(PlayRecording signal)
    {
        if (signal.record.buttonName != gameObject.name) return;

        if (signal.record.inputType == InputType.StartingState)
        {
            SetInitialColor(colorData.GetColorTemplate(signal.record.colorName));
        }
        if (signal.record.inputType == InputType.RightClickForColorChange)
        {
            ChangeColor();
        }
    }

    private void SendInitialColorForRecording()
    {
        _signalBus.Fire(new RecordInputSignal
        {
            colorName = currentColor,
            buttonName = gameObject.name,
            inputType = InputType.StartingState
        });
    }

    private void SetInitialColor(ColorTemplate ct)
    {
        currentColor = ct.colorName;
        image.color = ct.currentColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            ChangeColor();
            _signalBus.Fire(new RecordInputSignal { 
                buttonName = gameObject.name,
                inputType = InputType.RightClickForColorChange
            });
        }
    }

    private void ChangeColor()
    {
        ColorTemplate ct = colorData.GetColorTemplate(currentColor);
        image.color = ct.nextColor;
        currentColor = ct.nextColorName;
    }
}