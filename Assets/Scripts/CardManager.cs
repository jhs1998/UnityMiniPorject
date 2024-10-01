using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CardManager : MonoBehaviour
{
    public static CardManager Inst { get; private set; }
    void Awake() => Inst = this;

    [SerializeField] CardSkillSO cardskillSO;

    [SerializeField] GameObject attackCard01;  // ����� ī�� ������
    [SerializeField] GameObject attackCard02;  // ��μ��� ī�� ������
    [SerializeField] GameObject guardCard;     // ���� ī�� ������
    [SerializeField] GameObject healingCard;   // ġ�� ī�� ������
    [SerializeField] Transform cardParent;
    // ī�� ���� ��ġ
    [SerializeField] Transform cardspon;

    // ī�� ��ġ ����
    [SerializeField] Vector2 minSpawnPosition;
    [SerializeField] Vector2 maxSpawnPosition;



    public static List<Skill> SkillBuffer;
    private List<GameObject> activeCards = new List<GameObject>(); // Ȱ�� ī�� ����Ʈ

    public Skill PopCard()
    {
        if (SkillBuffer.Count == 0)
            SetupSkillBuffer();

        Skill skill = SkillBuffer[0];
        SkillBuffer.RemoveAt(0);

        GameManager.Inst.UpdateDeckNum(SkillBuffer.Count);

        return skill;
    }

    void SetupSkillBuffer()
    {
        SkillBuffer = new List<Skill>();
        for (int i = 0; i < cardskillSO.skills.Length; i++)
        {
            Skill skill = cardskillSO.skills[i];
            for (int j = 0; j < skill.drawpercent; j++)
                SkillBuffer.Add(skill);
        }

        for (int i = 0; i < SkillBuffer.Count; i++)
        {
            int rand = Random.Range(i, SkillBuffer.Count);
            Skill temp = SkillBuffer[i];
            SkillBuffer[i] = SkillBuffer[rand];
            SkillBuffer[rand] = temp;
        }
    }

    private void Start()
    {       

        SetupSkillBuffer();

        for (int i = 0; i < 5; i++)
        {
            Invoke("AddCard", i);
        }

        GameManager.Inst.UpdateDeckNum(SkillBuffer.Count);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
            AddCard();
    }


    void AddCard()
    {
        Skill skill = PopCard();
        GameObject cardPrefab = FindName(skill.name);

        if (cardPrefab != null)
        {
            Vector3 spawnPosition = cardspon.transform.position;
            Quaternion spawnRotation = cardspon.transform.rotation;

            GameObject newCard = Instantiate(cardPrefab, spawnPosition, spawnRotation, cardParent);
            newCard.name = skill.name; // ������ ī�� ������Ʈ�� �̸��� ����

            // ī�� ����Ʈ�� �߰�
            activeCards.Add(newCard);

            // ī�� ���� �� �̵�
            AlignCards();

            // ī�� �ڵ忡 �̵�
            MoveCardToHand(newCard);
        }
        else
        {
            Debug.LogError($"�������� �����ϴ�: {skill.name}");
        }
    }

    GameObject FindName(string cardName)
    {
        switch (cardName)
        {
            case "�����":
                return attackCard01;
            case "��μ���":
                return attackCard02;
            case "����":
                return guardCard;
            case "ġ��":
                return healingCard;
            default:
                return null; // �ش��ϴ� �������� ������ null ��ȯ
        }
    }
    void MoveCardToHand(GameObject card)
    {
        AlignCards(); // ī�� ���� ȣ��
    }

    void AlignCards()
    {
        int cardCount = activeCards.Count;
        float spacing = Mathf.Max(50f, 200f / cardCount); // ī�� ���� ����

        // ī�� ��ġ ���� ���
        float startX = minSpawnPosition.x + (spacing / 2);
        float endX = maxSpawnPosition.x - (spacing / 2);
        float totalWidth = endX - startX;

        // �߾� ������ ���� ������
        float offset = Mathf.Max(0, totalWidth - (spacing * cardCount)) / 2;

        for (int i = 0; i < cardCount; i++)
        {
            GameObject card = activeCards[i];
            Vector3 targetPosition = new Vector3(startX + offset + (i * spacing), minSpawnPosition.y, 0);

            // DOTween���� ī�� �̵�
            card.transform.DOMove(targetPosition, 1f).SetEase(Ease.OutBounce);
        }
    }
}