using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Weapon
{
    public override void Attack()
    {
        playerAnimator = GameObject.Find("GFX").GetComponent<Animator>();
        playerAnimator.SetTrigger("Attack Hammer");
    }
}
