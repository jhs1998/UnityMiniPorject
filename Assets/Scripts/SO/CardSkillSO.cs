using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Skill
{
    public string name;
    // �ο��̹��� ����
    public RawImage rawImage; 
    // ������ ��ο� �� Ȯ��
    public float drawpercent;

    // ī�� ���� : ��ü �� ���� ����ī��, �� ī��, ü�� ȸ�� ī��
}

[CreateAssetMenu(fileName = "CardSkillSO", menuName = "Scriptable Object/CardSkillSO")]
public class CardSkillSO : ScriptableObject
{
    public Skill[] skills;
}
