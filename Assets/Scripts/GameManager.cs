using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Animations.Rigging;

public class GameManager : MonoBehaviour
{

    //전투 상황
    public enum State
    {
        start, playerTurn, enemyTurn, win, lose
    }

    // 플레이어 오브젝트
    [SerializeField] GameObject player;
    // 적 오브젝트 1, 2
    [SerializeField] GameObject enemy1;
    [SerializeField] GameObject enemy2;

    // 나중에 현재 몇턴째인지도 추가 가능

    //-------UI--------

    //플레이어 체력 ui 3개
    // 체력 텍스트 1,2
    public TextMeshProUGUI playerhptext1;
    public TextMeshProUGUI playerhptext2;
    // 체력바
    public Slider playerHpBarSlider;
    // 방어도 UI

    //적 체력 ui 1,2 
    // 체력바 1,2
    public Slider monsterHpBarSlider01;
    public Slider monsterHpBarSlider02;
    // 텍스트 1,2
    public TextMeshProUGUI monsterhptext1;
    public TextMeshProUGUI monsterhptext2;

    // 덱 숫자 UI
    [SerializeField] GameObject deckNum;
    private TextMeshProUGUI deckNumText;
    public static GameManager Inst { get; private set; }

    // 턴 종료 버튼
    [SerializeField] Button turnButton;
    // 코스트 숫자

    // 플레이어 턴 UI
    [SerializeField] GameObject playerturnUI;
    //--------------------------------

    // 플레이어 체력
    [SerializeField] public float playerHP;

    // 적들 체력
    [SerializeField] public float enemy01HP;
    [SerializeField] public float enemy02HP;

    // 방어도
    // 전체 코스트

    public State state;
    public bool playerLive;
    public bool isLive1; // 1적 생존 여부
    public bool isLive2; // 2적 생존 여부
    float playernowHP;
    float enemynowHP01;
    float enemynowHP02;

    private void Awake()
    {
        state = State.start; // 전투 시작 상태
        playernowHP = playerHP;
        enemynowHP01 = enemy01HP;
        enemynowHP02 = enemy02HP;
        playerLive = true;
        isLive1 = true;
        isLive2 = true;

        UIupdate();


        Inst = this;
        deckNumText = deckNum.GetComponent<TextMeshProUGUI>();

        turnButton.onClick.AddListener(ButtonClick);
    }

    private void Update()
    {
        if (state == State.start)
        {
            BattleStart();
        }

    }
    
    void BattleStart()
    {
        Debug.Log("전투 시작");
        // 턴넘기기
        state = State.playerTurn;
        PlayerAttackButton();
    }

    public void PlayerAttackButton()
    {
        if(state != State.playerTurn)
        {
            return;
        }
        StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerAttack()
    {
        yield return new WaitForSeconds(1f);

        if (playerturnUI == true)
            playerturnUI.SetActive(false);

        // 카드를 사용해 플레이어가 공격
        Debug.Log("플레이어 공격");
        enemynowHP01 -= 10;
        enemynowHP02 -= 10;
        UIupdate();

        if (enemynowHP01 <= 0)
        {
            enemynowHP01 = 0;
            isLive1 = false;
        }
        else if(enemynowHP02 <= 0)
        {
            enemynowHP02 = 0;
            isLive2 = false;
        }

        if (!isLive1 && !isLive2)
        {
            state = State.win;
            EndBattle();
        }
        else
        {
            // 대기중
            Debug.Log("플레이어 공격 완료. 턴을 넘기세요.");
        }
    }
    public void ButtonClick()
    {
        if (state == State.playerTurn)
        {
            Debug.Log("턴 넘김");
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
        yield return new WaitForSeconds(2f);
        // 적이 공격
        Debug.Log("적의 공격");
        playernowHP -= 5;
        // 적 공격이 끝나면 플레이어에게 턴 넘김
        UIupdate();

        if (playernowHP <= 0)
        {
            Debug.Log("플레이어 사망");
            playernowHP = 0;
            playerLive = false;
        }

        if (!isLive1 && !isLive2)
        {
            state = State.win;
            EndBattle();
        }
        else if (!playerLive)
        {
            Debug.Log("플레이어 사망");
            state = State.lose;
            EndBattle();
        }
        else
        {
            state = State.playerTurn;
            Debug.Log("적의 공격 완료. 플레이어의 턴입니다.");
            playerturnUI.SetActive(true);
            yield return new WaitForSeconds(1f);
            PlayerAttackButton();
        }        
    }

    // 덱 숫자 적용
    public void UpdateDeckNum(int num)
    {
        deckNumText.text = num.ToString();
    }

    // 체력 텍스트 업데이트
    private void UIupdate()
    {
        playerhptext1.text = ($"{playernowHP}/{playerHP}");
        playerhptext2.text = ($"{playernowHP}/{playerHP}");
        monsterhptext1.text = ($"{enemynowHP01}/{enemy01HP}");
        monsterhptext2.text = ($"{enemynowHP02}/{enemy02HP}");
        HPScrollbar();
    }
    // 체력바 업데이트
    public void HPScrollbar()
    {
        playerHpBarSlider.value = playernowHP / playerHP;
        monsterHpBarSlider01.value = enemynowHP01 / enemy01HP;
        monsterHpBarSlider02.value = enemynowHP02 / enemy02HP;
    }
}
