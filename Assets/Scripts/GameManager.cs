using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Animations.Rigging;

public class GameManager : MonoBehaviour
{

    //���� ��Ȳ
    public enum State
    {
        start, playerTurn, enemyTurn, win, lose
    }

    // �÷��̾� ������Ʈ
    [SerializeField] GameObject player;
    // �� ������Ʈ 1, 2
    [SerializeField] GameObject enemy1;
    [SerializeField] GameObject enemy2;

    // ���߿� ���� ����°������ �߰� ����

    //-------UI--------

    //�÷��̾� ü�� ui 3��
    // ü�� �ؽ�Ʈ 1,2
    public TextMeshProUGUI playerhptext1;
    public TextMeshProUGUI playerhptext2;
    // ü�¹�
    public Slider playerHpBarSlider;
    // �� UI

    //�� ü�� ui 1,2 
    // ü�¹� 1,2
    public Slider monsterHpBarSlider01;
    public Slider monsterHpBarSlider02;
    // �ؽ�Ʈ 1,2
    public TextMeshProUGUI monsterhptext1;
    public TextMeshProUGUI monsterhptext2;

    // �� ���� UI
    [SerializeField] GameObject deckNum;
    private TextMeshProUGUI deckNumText;
    public static GameManager Inst { get; private set; }

    // �� ���� ��ư
    [SerializeField] Button turnButton;
    // �ڽ�Ʈ ����

    // �÷��̾� �� UI
    [SerializeField] GameObject playerturnUI;
    //--------------------------------

    // �÷��̾� ü��
    [SerializeField] public float playerHP;

    // ���� ü��
    [SerializeField] public float enemy01HP;
    [SerializeField] public float enemy02HP;

    // ��
    // ��ü �ڽ�Ʈ

    public State state;
    public bool playerLive;
    public bool isLive1; // 1�� ���� ����
    public bool isLive2; // 2�� ���� ����
    float playernowHP;
    float enemynowHP01;
    float enemynowHP02;

    private void Awake()
    {
        state = State.start; // ���� ���� ����
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
        Debug.Log("���� ����");
        // �ϳѱ��
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

        // ī�带 ����� �÷��̾ ����
        Debug.Log("�÷��̾� ����");
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
            // �����
            Debug.Log("�÷��̾� ���� �Ϸ�. ���� �ѱ⼼��.");
        }
    }
    public void ButtonClick()
    {
        if (state == State.playerTurn)
        {
            Debug.Log("�� �ѱ�");
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
        yield return new WaitForSeconds(2f);
        // ���� ����
        Debug.Log("���� ����");
        playernowHP -= 5;
        // �� ������ ������ �÷��̾�� �� �ѱ�
        UIupdate();

        if (playernowHP <= 0)
        {
            Debug.Log("�÷��̾� ���");
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
            Debug.Log("�÷��̾� ���");
            state = State.lose;
            EndBattle();
        }
        else
        {
            state = State.playerTurn;
            Debug.Log("���� ���� �Ϸ�. �÷��̾��� ���Դϴ�.");
            playerturnUI.SetActive(true);
            yield return new WaitForSeconds(1f);
            PlayerAttackButton();
        }        
    }

    // �� ���� ����
    public void UpdateDeckNum(int num)
    {
        deckNumText.text = num.ToString();
    }

    // ü�� �ؽ�Ʈ ������Ʈ
    private void UIupdate()
    {
        playerhptext1.text = ($"{playernowHP}/{playerHP}");
        playerhptext2.text = ($"{playernowHP}/{playerHP}");
        monsterhptext1.text = ($"{enemynowHP01}/{enemy01HP}");
        monsterhptext2.text = ($"{enemynowHP02}/{enemy02HP}");
        HPScrollbar();
    }
    // ü�¹� ������Ʈ
    public void HPScrollbar()
    {
        playerHpBarSlider.value = playernowHP / playerHP;
        monsterHpBarSlider01.value = enemynowHP01 / enemy01HP;
        monsterHpBarSlider02.value = enemynowHP02 / enemy02HP;
    }
}
