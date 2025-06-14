using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealable
{
    // 그냥 HealEffect라는 효과 만드는게 나을듯 (이 인터페이스는 나중에 삭제예정)
    void Heal(float amount);
}
