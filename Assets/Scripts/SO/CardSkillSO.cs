using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill
{
    public string name;
    // 카드 데미지
    public int damage;
    // 방어도 수치
    public int guard;
    // 스프라이트 
    public Sprite sprite; 
    // 덱에서 드로우 될 확률
    public float drawpercent;

    // 카드 종류 : 공격카드, 방어도 카드, 체력 회복 카드, 회피 카드
}

[CreateAssetMenu(fileName = "CardSkillSO", menuName = "Scriptable Object/CardSkillSO")]
public class CardSkillSO : ScriptableObject
{
    public Skill[] skills;
}
