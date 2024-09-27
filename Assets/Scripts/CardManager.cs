using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CardManager : MonoBehaviour
{
    public static CardManager Inst {  get; private set; }
    void Awake () => Inst = this;

    [SerializeField] CardSkillSO cardskillSO;
    [SerializeField] GameObject cardPrefab;

    List<Skill> SkillBuffer;


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
    }

    void AddCard()
    {

    }
}
