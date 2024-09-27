using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill
{
    public string name;
    // ī�� ������
    public int damage;
    // �� ��ġ
    public int guard;
    // ��������Ʈ 
    public Sprite sprite; 
    // ������ ��ο� �� Ȯ��
    public float drawpercent;

    // ī�� ���� : ����ī��, �� ī��, ü�� ȸ�� ī��, ȸ�� ī��
}

[CreateAssetMenu(fileName = "CardSkillSO", menuName = "Scriptable Object/CardSkillSO")]
public class CardSkillSO : ScriptableObject
{
    public Skill[] skills;
}
