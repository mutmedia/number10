﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public abstract class Character : MonoBehaviour
{
    public Dictionary<AbilityType, Ability> AbilityBuilder;

    protected Ability Ability;
    public Health Health;
    public Stun Stun;

    public bool IsCasting;

    public bool CanCastAbility
    {
        get
        {
            return (this.Ability != null)
                   && !Stun.IsStunned
                   && !Ability.Cooldown.OnCooldown
                   && !IsCasting;

        }
    }

    public virtual void Start()
    {
        Health = GetComponentInChildren<Health>();
        Stun = GetComponentInChildren<Stun>();
        // The collider is trigger
        GetComponent<CircleCollider2D>().isTrigger = true;
    }

    public virtual void Update()
    {
        if (Health.Value <= 0.0)
        {
            Destroy(gameObject);
        }
    }

    public delegate void OnDestroyCallback();
    public event OnDestroyCallback OnDestroyCallbacks;
    public void OnDestroy()
    {
        OnDestroyCallbacks.Invoke();
    }

    protected virtual void CastAbility(object sender, AbilityCastEventArgs e)
    {
        if(Ability == null)
        {
            Debug.Log("No ability selected");
            return;
        }
        if (Stun.IsStunned) return;
        if (IsCasting) return;
        Ability.TryCast(this, e);
    }
}
