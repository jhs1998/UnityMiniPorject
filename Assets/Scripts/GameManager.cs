using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    //���� ��Ȳ
    public enum State
    {
        start, playerTurn, enemyTurn, win
    }

    // �÷��̾� ������Ʈ

    // �� ������Ʈ 1, 2

    // �ϳѱ� ��ư

    // ���߿� ���� ����°������ �߰� ����

    //-------UI--------

    //�÷��̾� ü�� ui 3��
    // ü�� �ؽ�Ʈ 1,2
    // ü�¹�

    // �� UI

    //�� ü�� ui 1,2 
    // ü�¹� 1,2
    // �ؽ�Ʈ 1,2

    // �� ���� UI
    [SerializeField] GameObject deckNum;
    private TextMeshProUGUI deckNumText;
    public static GameManager Inst { get; private set; }
    // �ڽ�Ʈ ����

    //-----------

    // �÷��̾� ü��
    // ���� ü��

    // ��
    // ��ü �ڽ�Ʈ

    // ī�� ����



    public State state;
    public bool isLive1; // 1�� ���� ����
    public bool isLive2; // 2�� ���� ����

    private void Awake()
    {
        state = State.start; // ���� ���� ����

        Inst = this;
        deckNumText = deckNum.GetComponent<TextMeshProUGUI>();
    }
    

    void BattleStart()
    {
        
        // �ϳѱ��
        state = State.playerTurn;
    }

    public void PlayerAttackButton()
    {
        // ī�� ������� �÷��̾� �ൿ

        if(state != State.playerTurn)
        {
            return;
        }
        StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerAttack()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("�÷��̾� ����");
        if(!isLive1)
        {
            state = State.win;
            EndBattle();
        }
        else
        {
            state = State.enemyTurn;
            StartCoroutine(EnemyTurn());
        }
    }

    void EndBattle()
    {
        Debug.Log("���� ����");
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);
        // ���� ����

        // �� ������ ������ �÷��̾�� �� �ѱ�

        state = State.playerTurn;
        
    }
    // �� ���� ����
    public void UpdateDeckNum(int num)
    {
        deckNumText.text = num.ToString();
    }
}
