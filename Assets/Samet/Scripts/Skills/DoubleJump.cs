using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : Skill
{
    public override void UseSkill()
    {
        base.UseSkill();
        PlayerManager.instance.player.SetVelocity(PlayerManager.instance.player.rb.velocity.x,0);
        PlayerManager.instance.player.SetVelocity(PlayerManager.instance.player.rb.velocity.x, PlayerManager.instance.player.jumpForce);
    }
}
