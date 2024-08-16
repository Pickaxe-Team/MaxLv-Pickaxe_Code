using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    // 체력 필드
    public float MaxHP;
    public float CurHP; //현재 체력

    public event Action OnChangeHP;

    // 사망 이벤트
    public event Action OnDeath;
    public bool IsDead => CurHP == 0;

    public void InitHP(float curHP, float maxHP)
    {
        CurHP = curHP;
        MaxHP = maxHP;
    }

    // 체력을 감소시키는 메서드
    public void TakeDamage(float damage)
    {
        CurHP -= damage;
        if (CurHP <= 0)
        {
            CurHP = 0;
            OnDeath?.Invoke();
        }

        OnChangeHP?.Invoke();
    }

    // 체력을 회복시키는 메서드
    public void Heal(float amount)
    {
        CurHP += amount;

        // 최대 체력을 넘지 않도록 제한
        CurHP = Mathf.Min(CurHP, MaxHP);

        OnChangeHP?.Invoke();
    }
}

