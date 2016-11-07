﻿using UnityEngine;
using System.Collections;

public class SwapAbility : Ability
{
    [SerializeField]
    private Character _char1;
    [SerializeField]
    private Character _char2;

    protected override AbilityType GetAbilityType()
    {
        return AbilityType.Swap;
    }

    protected override void Cast(Character caster, AbilityCastEventArgs e)
    {
        RaycastHit2D hit = Physics2D.Raycast(e.Position, Vector2.zero, 0f);
        if (hit)
        {
            var charSelected = hit.transform.GetComponent<Character>();
            if (charSelected)
            {
                charSelected.GetComponent<SpriteRenderer>().color = Color.cyan;
                if (_char1 == null)
                {
                    _char1 = charSelected;
                }
                else if (_char2 == null)
                {
                    _char2 = charSelected;
                    // Swap characters after some time
                    StartCoroutine(SwapCharacters(caster));
                }
            }
        }
    }

    IEnumerator SwapCharacters(Character caster)
    {
        Cooldown.ResetCooldown();
        yield return new WaitForSeconds(Data.Casttime);
        if (caster.Stun.IsStunned) yield break;
        var char1pos = _char1.transform.position;
        _char1.transform.position = _char2.transform.position;
        _char2.transform.position = char1pos;
        _char1.GetComponent<SpriteRenderer>().color = Color.white;
        _char2.GetComponent<SpriteRenderer>().color = Color.white;
        _char1 = null;
        _char2 = null;        
    }
}
