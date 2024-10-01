using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CardManager : MonoBehaviour
{
    public static CardManager Inst { get; private set; }
    void Awake() => Inst = this;

    [SerializeField] CardSkillSO cardskillSO;

    [SerializeField] GameObject attackCard01;  // 약공격 카드 프리팹
    [SerializeField] GameObject attackCard02;  // 골부수기 카드 프리팹
    [SerializeField] GameObject guardCard;     // 막기 카드 프리팹
    [SerializeField] GameObject healingCard;   // 치유 카드 프리팹
    [SerializeField] Transform cardParent;
    // 카드 생성 위치
    [SerializeField] Transform cardspon;

    // 카드 배치 범위
    [SerializeField] Vector2 minSpawnPosition;
    [SerializeField] Vector2 maxSpawnPosition;



    public static List<Skill> SkillBuffer;
    private List<GameObject> activeCards = new List<GameObject>(); // 활성 카드 리스트

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
            newCard.name = skill.name; // 생성된 카드 오브젝트의 이름을 설정

            // 카드 리스트에 추가
            activeCards.Add(newCard);

            // 카드 정렬 및 이동
            AlignCards();

            // 카드 핸드에 이동
            MoveCardToHand(newCard);
        }
        else
        {
            Debug.LogError($"프리팹이 없습니다: {skill.name}");
        }
    }

    GameObject FindName(string cardName)
    {
        switch (cardName)
        {
            case "약공격":
                return attackCard01;
            case "골부수기":
                return attackCard02;
            case "막기":
                return guardCard;
            case "치유":
                return healingCard;
            default:
                return null; // 해당하는 프리팹이 없으면 null 반환
        }
    }
    void MoveCardToHand(GameObject card)
    {
        AlignCards(); // 카드 정렬 호출
    }

    void AlignCards()
    {
        int cardCount = activeCards.Count;
        float spacing = Mathf.Max(50f, 200f / cardCount); // 카드 간격 조정

        // 카드 배치 범위 계산
        float startX = minSpawnPosition.x + (spacing / 2);
        float endX = maxSpawnPosition.x - (spacing / 2);
        float totalWidth = endX - startX;

        // 중앙 정렬을 위한 오프셋
        float offset = Mathf.Max(0, totalWidth - (spacing * cardCount)) / 2;

        for (int i = 0; i < cardCount; i++)
        {
            GameObject card = activeCards[i];
            Vector3 targetPosition = new Vector3(startX + offset + (i * spacing), minSpawnPosition.y, 0);

            // DOTween으로 카드 이동
            card.transform.DOMove(targetPosition, 1f).SetEase(Ease.OutBounce);
        }
    }
}