using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    //전투 상황
    public enum State
    {
        start, playerTurn, enemyTurn, win
    }

    // 플레이어 오브젝트

    // 적 오브젝트 1, 2

    // 턴넘김 버튼

    // 나중에 현재 몇턴째인지도 추가 가능

    //-------UI--------

    //플레이어 체력 ui 3개
    // 체력 텍스트 1,2
    // 체력바

    // 방어도 UI

    //적 체력 ui 1,2 
    // 체력바 1,2
    // 텍스트 1,2

    // 덱 숫자 UI
    [SerializeField] GameObject deckNum;
    private TextMeshProUGUI deckNumText;
    public static GameManager Inst { get; private set; }
    // 코스트 숫자

    //-----------

    // 플레이어 체력
    // 적들 체력

    // 방어도
    // 전체 코스트

    // 카드 숫자



    public State state;
    public bool isLive1; // 1적 생존 여부
    public bool isLive2; // 2적 생존 여부

    private void Awake()
    {
        state = State.start; // 전투 시작 상태

        Inst = this;
        deckNumText = deckNum.GetComponent<TextMeshProUGUI>();
    }
    

    void BattleStart()
    {
        
        // 턴넘기기
        state = State.playerTurn;
    }

    public void PlayerAttackButton()
    {
        // 카드 사용으로 플레이어 행동

        if(state != State.playerTurn)
        {
            return;
        }
        StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerAttack()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("플레이어 공격");
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
        Debug.Log("전투 종료");
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);
        // 적이 공격

        // 적 공격이 끝나면 플레이어에게 턴 넘김

        state = State.playerTurn;
        
    }
    // 덱 숫자 적용
    public void UpdateDeckNum(int num)
    {
        deckNumText.text = num.ToString();
    }
}
