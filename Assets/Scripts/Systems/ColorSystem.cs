using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class ColorSystem : MonoBehaviour, IPointerClickHandler
{
    private Image image;

    [SerializeField]
    private ColorName currentColor;

    #region Zenject
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

    private void SendInitialColorForRecording()
    {
        Debug.Log($"Current Color {currentColor}");
        _signalBus.Fire(new RecordInputSignal
        {
            colorName = currentColor,
            buttonName = gameObject.name,
            inputType = InputType.StartingColor
        });
    }

    private void PlayRecording(PlayRecording signal)
    {
        if (signal.record.buttonName != gameObject.name) return;

        if (signal.record.inputType == InputType.StartingColor)
        {
            SetInitialColor(colorData.GetColorTemplate(signal.record.colorName));
        }
        if (signal.record.inputType == InputType.RightClickForColorChange)
        {
            ChangeColor();
        }
    }

    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        SetInitialColor(colorData.GetRandomColorTemplate());
    }
    private void OnDestroy()
    {
        _signalBus.TryUnsubscribe<RecordInitialStateOfButtons>(SendInitialColorForRecording);
        _signalBus.TryUnsubscribe<PlayRecording>(PlayRecording);
    }
    #endregion

    #region IPointer implementation
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ChangeColor();
            _signalBus.Fire(new RecordInputSignal
            {
                buttonName = gameObject.name,
                inputType = InputType.RightClickForColorChange
            });
        }
    }
    #endregion

    #region Button Manipulation
    private void SetInitialColor(ColorTemplate ct)
    {
        currentColor = ct.colorName;
        image.color = ct.currentColor;
    }

    private void ChangeColor()
    {
        ColorTemplate ct = colorData.GetColorTemplate(currentColor);
        image.color = ct.nextColor;
        currentColor = ct.nextColorName;
    }
    #endregion
}