using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{

    //���� ��Ȳ
    public enum State
    {
        start, playerTurn, enemyTurn, win, lose
    }

    [SerializeField] Cardcontroller card;
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
    // ���� ���� UI
    [SerializeField] GameObject gameoverUI;
    // ���� Ŭ���� UI
    [SerializeField] GameObject gameclearUI;
    //--------------------------------

    // �÷��̾� ü��
    [SerializeField] public float playerHP;

    // ���� ü��
    [SerializeField] public float enemy01HP;
    [SerializeField] public float enemy02HP;

    //�÷��̾� �ִ�
    [SerializeField] Animator playeranimator;
    // �� �ִ�
    [SerializeField] Animator enemyanimator01;
    [SerializeField] Animator enemyanimator02;
    // ��
    // ��ü �ڽ�Ʈ

    public State state;
    public bool playerLive;
    public bool isLive1; // 1�� ���� ����
    public bool isLive2; // 2�� ���� ����
    float playernowHP;
    float enemynowHP01;
    float enemynowHP02;
    string monsterName;

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
    public void RegisterCard(Cardcontroller card)
    {
        // ī�� �̺�Ʈ ������ ���
        card.OnAttack01 += AttackCard01;
        card.OnAttack02 += AttackCard02;
        card.OnGuard += OnGuard;
        card.OnHealing += OnHealing;
    }


    private void Update()
    {
        if (state == State.start)
        {
            BattleStart();
        }

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("BattleMapScene");

        if (enemynowHP01 == 0)
            playerDead();
        else if (!isLive1 && !isLive2)
        {
            state = State.win;
            EndBattle();
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
        playerturnUI.SetActive(true);

        if (state != State.playerTurn)
        {
            return;
        }
        StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerAttack()
    {
        yield return new WaitForSeconds(1.5f);

        if (playerturnUI == true)
            playerturnUI.SetActive(false);

        // ī�带 ����� �÷��̾ ����
        Debug.Log("�÷��̾� ����");

        UIupdate();
        string monsterName1 = GetComponent<Cardcontroller>().monsterName;
        monsterName = monsterName1;
        if (enemynowHP01 <= 0)
        {
            enemynowHP01 = 0;
            enemyanimator01.SetTrigger("GoblinDead");
            isLive1 = false;
        }
        else if(enemynowHP02 <= 0)
        {
            enemynowHP02 = 0;
            enemyanimator02.SetTrigger("MuchroomDead");
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
        if (state == State.win)
            gameclearUI.SetActive(true);
        else if (state == State.lose)
            gameoverUI.SetActive(true);       
    }



    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(2f);
        // ���� ����
        Debug.Log("���� ����");
        monsterAttack01();
        // �� ������ ������ �÷��̾�� �� �ѱ�
        monsterAttack02();

        UIupdate();

        if (playernowHP <= 0)
        {
            Debug.Log("�÷��̾� ���");
            playernowHP = 0;
        }

        else
        {
            state = State.playerTurn;
            Debug.Log("���� ���� �Ϸ�. �÷��̾��� ���Դϴ�.");           
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
    public void monsterAttack01()
    {
        playernowHP -= 5;
        playeranimator.SetTrigger("DamageGet");
        enemyanimator01.SetTrigger("GoblinAttack02");
        UIupdate();
    }
    public void monsterAttack02()
    {
        playernowHP -= 5;
        playeranimator.SetTrigger("DamageGet");
        enemyanimator02.SetTrigger("MuchroomAttack02");
        UIupdate();
    }
    public void playerDead()
    {
        // �÷��̾� �׾����� �ڵ�
        // �׾����� �ִ�
        playeranimator.SetTrigger("Dead");
        state = State.lose;
        EndBattle();
        
    }
    public void AttackCard01(string monsterName)
    {
        Debug.Log($"����� ���, ����: {monsterName}");
        // �� ü�� ���� ����
        if (monsterName == enemy1.name)
        {
            enemynowHP01 -= 5;
            enemyanimator01.SetTrigger("GoblinTakeDamage");
        }
        else if (monsterName == enemy2.name)
        {
            enemynowHP02 -= 5;
            enemyanimator02.SetTrigger("MuchroomTakeDamage");
        }
        UIupdate();
    }
    public void AttackCard02(string monsterName)
    {
        Debug.Log($"��μ��� ���, ����: {monsterName}");
        // �� ü�� ���� ����
        if (monsterName == enemy1.name)
        {
            enemynowHP01 -= 20;
            enemyanimator01.SetTrigger("GoblinTakeDamage");
        }
        else if (monsterName == enemy2.name)
        {
            enemynowHP02 -= 20;
            enemyanimator02.SetTrigger("MuchroomTakeDamage");
        }
        UIupdate();
    }
    public void OnGuard()
    {
        // �� ����ϴ� ����

        UIupdate();
    }
    public void OnHealing()
    {
        playernowHP += 10;
        UIupdate();
    }
}
