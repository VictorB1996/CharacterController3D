using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    public override void Attack()
    {
        playerAnimator = GameObject.Find("GFX").GetComponent<Animator>();
        playerAnimator.SetTrigger("Attack Sword");
    }
}
