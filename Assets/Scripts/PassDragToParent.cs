using UnityEngine;
using UnityEngine.EventSystems;

public class PassDragToParent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Pass the drag event to the parent
        ExecuteEvents.ExecuteHierarchy<IBeginDragHandler>(transform.parent.gameObject, eventData, ExecuteEvents.beginDragHandler);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Pass the drag event to the parent
        ExecuteEvents.ExecuteHierarchy<IDragHandler>(transform.parent.gameObject, eventData, ExecuteEvents.dragHandler);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Pass the drag event to the parent
        ExecuteEvents.ExecuteHierarchy<IEndDragHandler>(transform.parent.gameObject, eventData, ExecuteEvents.endDragHandler);
    }

    public void OnScroll(PointerEventData eventData)
    {
        // Pass the scroll event to the parent
        ExecuteEvents.ExecuteHierarchy<IScrollHandler>(transform.parent.gameObject, eventData, ExecuteEvents.scrollHandler);
    }
}
