using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cardcontroller : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    private void Start()
    {
        // 처음 크기 저장
        originalScale = transform.localScale;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.Translate(eventData.delta);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * 1.5f;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }
}
