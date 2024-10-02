using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Threading;
using UnityEngine.Events;

public class Cardcontroller : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public PRS originPRS;
    private Vector3 originalScale;
    Vector3 dragBeginPos;
    
    [SerializeField] new GameObject gameObject;
    private RawImage cardImage;

    // 카드 이벤트 생성
    public event UnityAction OnGuard;
    public event UnityAction OnHealing;
    public event UnityAction<string> OnAttack01;
    public event UnityAction<string> OnAttack02;
    public string monsterName;
    private void Start()
    {
        GameManager.Inst.RegisterCard(this);
        // 처음 크기 저장
        originalScale = transform.localScale;
        // 카드 이미지 가져오기
        cardImage = GetComponent<RawImage>();

        
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
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragBeginPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position += (Vector3)eventData.delta;
        //transform.Translate(eventData.delta);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hitInfo))
        {
            Monster monster = hitInfo.collider.gameObject.GetComponent<Monster>();
            // 카드가 적1의 UI에 드롭되었는지 확인
            CardEffect(monster);          
        }
        else
        {
            ReturnHand(); // 적이 아니면 리턴
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
    private void ReturnHand()
    {
        transform.position = dragBeginPos;
    }

    private void CardEffect(Monster monster)
    {
        string cardName = gameObject.name.Replace("(Clone)", "").Trim();
        monsterName = monster != null ? monster.name : "";
        // 카드의 종류에 따라 적용
        switch (cardName)
        {
            case "약공격":
            case "골부수기":
                if (monster != null && monster.name != "Player")
                {
                    Debug.Log($"{cardName} 사용 - 적에게 공격");

                    // 이벤트 트리거 시 monsterName을 전달
                    if (cardName == "약공격")
                    {
                        OnAttack01?.Invoke(monsterName);
                    }
                    else if (cardName == "골부수기")
                    {
                        OnAttack02?.Invoke(monsterName);
                    }

                    Destroy(gameObject);  // 몬스터에게만 삭제
                }
                else
                {
                    Debug.Log($"{cardName}는 플레이어에게 사용할 수 없습니다. 카드가 핸드로 돌아갑니다.");
                    ReturnHand();  // 플레이어에게 사용 시 핸드로 돌아감
                }
                break;
            case "막기":
            case "치유":
                if (monster != null && monster.name == "Player")
                {
                    Debug.Log($"{cardName} 사용 - 플레이어에게 적용");
                    // 플레이어에게 방어/힐 적용 로직
                    // if문으로 구분
                    if (cardName == "막기")
                    {
                        OnGuard?.Invoke();
                    }
                    else
                    {
                        OnHealing?.Invoke();
                    }
                    Destroy(gameObject);  // 플레이어에게만 삭제
                }
                else
                {
                    Debug.Log($"{cardName}는 몬스터에게 사용할 수 없습니다. 카드가 핸드로 돌아갑니다.");
                    ReturnHand();  // 몬스터에게 사용 시 핸드로 돌아감
                }
                break;
        }        
    }
}
