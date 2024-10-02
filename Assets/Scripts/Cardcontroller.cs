using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Cardcontroller : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public PRS originPRS;
    private Vector3 originalScale;
    private bool DropEnemy = false;

    // 적 오브젝트
    [SerializeField] Image enemy1; // prefap에서 적용안됨
    [SerializeField] Image enemy2; // prefap에서 적용안됨
    public void SetEnemies(Image e1, Image e2)
    {
        enemy1 = e1;
        enemy2 = e2;
    }

    private void Start()
    {
        // 처음 크기 저장
        originalScale = transform.localScale;
    }
    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {
            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        
        transform.Translate(eventData.delta);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // **카드 드래그가 끝났을 때 호출**
        //DropEnemy = CheckDropEnemy();

        if (!DropEnemy)
        {
            // **적 UI가 아닌 곳에 드롭된 경우 원래 위치로 되돌아옴**
            ReturnHand();
        }
        else
        {
            // **적 UI에 드롭된 경우 데미지 적용**
            CardEffect();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * 1.5f;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }
    /*
    private bool CheckDropEnemy()
    {
        // 카드가 적1의 UI에 드롭되었는지 확인
        if () // 적1 충돌 조건
        {
            Debug.Log("적1에게 드롭됨");
            return true;
        }
        // 카드가 적2의 UI에 드롭되었는지 확인
        else if ()// 적2 충돌 조건
        {
            Debug.Log("적2에게 드롭됨");
            return true;
        }

        return false; // 적이 아니면 false
    }*/
    private void ReturnHand()
    {
        // 원래 위치로 카드 이동
        transform.DOMove(originPRS.pos, 1f).SetEase(Ease.OutBounce);
        transform.DORotateQuaternion(originPRS.rot, 1f);
        transform.DOScale(originPRS.scale, 1f);
    }

    private void CardEffect()
    {
        // 카드의 종류에 따라 적용
        Debug.Log("적에게 데미지");
        // GameManager를 통해 적에게 데미지를 주는 로직 추가 가능
    }
}
