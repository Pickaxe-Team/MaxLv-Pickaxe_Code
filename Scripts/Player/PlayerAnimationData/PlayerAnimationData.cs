using System;
using UnityEngine;

[Serializable]
public class PlayerAnimationData
{
    public int IdleParameterHash { get; private set; }
    public int RollParameterHash { get; private set; }
    public int WalkParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }

    public void Initialize()
    {
        IdleParameterHash = Animator.StringToHash(ParameterHash.Idle);
        WalkParameterHash = Animator.StringToHash(ParameterHash.Walk);
        RollParameterHash = Animator.StringToHash(ParameterHash.Roll);
        AttackParameterHash = Animator.StringToHash(ParameterHash.Attack);
    }
}
