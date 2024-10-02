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

    // ī�� �̺�Ʈ ����
    public event UnityAction OnGuard;
    public event UnityAction OnHealing;
    public event UnityAction<string> OnAttack01;
    public event UnityAction<string> OnAttack02;
    public string monsterName;
    private void Start()
    {
        GameManager.Inst.RegisterCard(this);
        // ó�� ũ�� ����
        originalScale = transform.localScale;
        // ī�� �̹��� ��������
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
            // ī�尡 ��1�� UI�� ��ӵǾ����� Ȯ��
            CardEffect(monster);          
        }
        else
        {
            ReturnHand(); // ���� �ƴϸ� ����
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
        // ī���� ������ ���� ����
        switch (cardName)
        {
            case "�����":
            case "��μ���":
                if (monster != null && monster.name != "Player")
                {
                    Debug.Log($"{cardName} ��� - ������ ����");

                    // �̺�Ʈ Ʈ���� �� monsterName�� ����
                    if (cardName == "�����")
                    {
                        OnAttack01?.Invoke(monsterName);
                    }
                    else if (cardName == "��μ���")
                    {
                        OnAttack02?.Invoke(monsterName);
                    }

                    Destroy(gameObject);  // ���Ϳ��Ը� ����
                }
                else
                {
                    Debug.Log($"{cardName}�� �÷��̾�� ����� �� �����ϴ�. ī�尡 �ڵ�� ���ư��ϴ�.");
                    ReturnHand();  // �÷��̾�� ��� �� �ڵ�� ���ư�
                }
                break;
            case "����":
            case "ġ��":
                if (monster != null && monster.name == "Player")
                {
                    Debug.Log($"{cardName} ��� - �÷��̾�� ����");
                    // �÷��̾�� ���/�� ���� ����
                    // if������ ����
                    if (cardName == "����")
                    {
                        OnGuard?.Invoke();
                    }
                    else
                    {
                        OnHealing?.Invoke();
                    }
                    Destroy(gameObject);  // �÷��̾�Ը� ����
                }
                else
                {
                    Debug.Log($"{cardName}�� ���Ϳ��� ����� �� �����ϴ�. ī�尡 �ڵ�� ���ư��ϴ�.");
                    ReturnHand();  // ���Ϳ��� ��� �� �ڵ�� ���ư�
                }
                break;
        }        
    }
}
