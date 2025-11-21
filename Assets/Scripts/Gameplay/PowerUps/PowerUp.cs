using UnityEngine;
using System;


public abstract class PowerUpBase : MonoBehaviour, IPowerUp
{
    public abstract void Apply(GameObject target);

    // Можно добавить базовые поля, например скорость падения

}
