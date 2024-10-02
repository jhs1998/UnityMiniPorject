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

    // �� ������Ʈ
    [SerializeField] Image enemy1; // prefap���� ����ȵ�
    [SerializeField] Image enemy2; // prefap���� ����ȵ�
    public void SetEnemies(Image e1, Image e2)
    {
        enemy1 = e1;
        enemy2 = e2;
    }

    private void Start()
    {
        // ó�� ũ�� ����
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
        // **ī�� �巡�װ� ������ �� ȣ��**
        //DropEnemy = CheckDropEnemy();

        if (!DropEnemy)
        {
            // **�� UI�� �ƴ� ���� ��ӵ� ��� ���� ��ġ�� �ǵ��ƿ�**
            ReturnHand();
        }
        else
        {
            // **�� UI�� ��ӵ� ��� ������ ����**
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
        // ī�尡 ��1�� UI�� ��ӵǾ����� Ȯ��
        if () // ��1 �浹 ����
        {
            Debug.Log("��1���� ��ӵ�");
            return true;
        }
        // ī�尡 ��2�� UI�� ��ӵǾ����� Ȯ��
        else if ()// ��2 �浹 ����
        {
            Debug.Log("��2���� ��ӵ�");
            return true;
        }

        return false; // ���� �ƴϸ� false
    }*/
    private void ReturnHand()
    {
        // ���� ��ġ�� ī�� �̵�
        transform.DOMove(originPRS.pos, 1f).SetEase(Ease.OutBounce);
        transform.DORotateQuaternion(originPRS.rot, 1f);
        transform.DOScale(originPRS.scale, 1f);
    }

    private void CardEffect()
    {
        // ī���� ������ ���� ����
        Debug.Log("������ ������");
        // GameManager�� ���� ������ �������� �ִ� ���� �߰� ����
    }
}
