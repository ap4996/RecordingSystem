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
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta;
        Debug.Log($"object position {rect.anchoredPosition} time {Time.timeAsDouble}");
        _signalBus.Fire(new RecordDragging
        {
            anchoredPosition = rect.anchoredPosition,
            buttonName = gameObject.name
        });
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }
}
