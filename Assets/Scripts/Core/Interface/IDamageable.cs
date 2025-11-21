using UnityEngine;

public interface IDamageable
{
    bool IsDead { get; }
    void TakeHit(int amount = 1);
}