using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealable
{
    // �׳� HealEffect��� ȿ�� ����°� ������ (�� �������̽��� ���߿� ��������)
    void Heal(float amount);
}
