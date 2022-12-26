using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class DragSystem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rect;

    [Inject]
    private SignalBus _signalBus;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        _signalBus.Fire(new StartRecording());
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta;
        _signalBus.Fire(new RecordDragging
        {
            anchoredPosition = rect.anchoredPosition,
            buttonName = gameObject.name
        });
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _signalBus.Fire(new StopRecording());
    }
}
