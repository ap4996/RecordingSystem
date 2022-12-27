using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class DragSystem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rect;

    private SignalBus _signalBus;
    
    [Inject]
    private void InjectSignalBus(SignalBus signalBus)
    {
        _signalBus = signalBus;
        _signalBus.Subscribe<RecordInitialStateOfButtons>(SendInitialPosition);
        _signalBus.Subscribe<PlayRecording>(PlayRecording);
    }

    private void SendInitialPosition()
    {
        _signalBus.Fire(new RecordInputSignal
        {
            anchoredPosition = rect.anchoredPosition,
            buttonName = gameObject.name,
            inputType = InputType.StartingState
        });
    }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void PlayRecording(PlayRecording signal)
    {
        if (signal.record.buttonName != gameObject.name) return;

        if(signal.record.inputType == InputType.StartingState)
        {
            SetButtonPosition(signal.record.buttonAnchoredPosition);
        }
        if(signal.record.inputType == InputType.Dragging)
        {
            SetButtonPosition(signal.record.buttonAnchoredPosition);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetButtonPosition(rect.anchoredPosition + eventData.delta);
        _signalBus.Fire(new RecordInputSignal
        {
            anchoredPosition = rect.anchoredPosition,
            buttonName = gameObject.name,
            inputType = InputType.Dragging
        });
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

    private void SetButtonPosition(Vector2 pos)
    {
        rect.anchoredPosition = pos;
    }
}
