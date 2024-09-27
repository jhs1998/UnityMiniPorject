using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Skill
{
    public string name;
    // 로우이미지 삽입
    public RawImage rawImage; 
    // 덱에서 드로우 될 확률
    public float drawpercent;

    // 카드 종류 : 전체 및 단일 공격카드, 방어도 카드, 체력 회복 카드
}

[CreateAssetMenu(fileName = "CardSkillSO", menuName = "Scriptable Object/CardSkillSO")]
public class CardSkillSO : ScriptableObject
{
    public Skill[] skills;
}
