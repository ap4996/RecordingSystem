using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class DragSystem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rect;

    private Vector2 startingPosition;

    #region Zenject

    private SignalBus _signalBus;
    
    [Inject]
    private void InjectSignalBus(SignalBus signalBus)
    {
        _signalBus = signalBus;
        _signalBus.Subscribe<RecordInitialStateOfButtons>(SendInitialPosition);
        _signalBus.Subscribe<PlayRecording>(PlayRecording);
        _signalBus.Subscribe<ReplayCompleted>(ReplayCompleted);
    }
    private void OnDestroy()
    {
        _signalBus.TryUnsubscribe<RecordInitialStateOfButtons>(SendInitialPosition);
        _signalBus.TryUnsubscribe<PlayRecording>(PlayRecording);
        _signalBus.TryUnsubscribe<ReplayCompleted>(ReplayCompleted);
    }
    private void ReplayCompleted()
    {
        SetButtonPosition(startingPosition);
    }
    private void SendInitialPosition()
    {
        _signalBus.Fire(new RecordInputSignal
        {
            anchoredPosition = rect.anchoredPosition,
            buttonName = gameObject.name,
            inputType = InputType.StartingPosition
        });
    }
    private void PlayRecording(PlayRecording signal)
    {
        if (signal.record.buttonName != gameObject.name) return;

        if(signal.record.inputType == InputType.StartingPosition)
        {
            SetButtonPosition(signal.record.buttonAnchoredPosition);
        }
        if(signal.record.inputType == InputType.Dragging)
        {
            SetButtonPosition(signal.record.buttonAnchoredPosition);
        }
    }
    #endregion

    #region Monobehaviour

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        startingPosition = rect.anchoredPosition;
    }

    #endregion

    #region IDrag implementaion
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        _signalBus.Fire(new HideTooltipSignal());
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
    #endregion

    #region Button Manipulation

    private void SetButtonPosition(Vector2 pos)
    {
        rect.anchoredPosition = pos;
    }
    #endregion
}
