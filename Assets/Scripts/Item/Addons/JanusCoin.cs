﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 애드온명: 야누스의 동전
/// 번호: 24
/// </summary>
public class JanusCoin : Addon
{
    public override void Declare()
    {
        id = 24; name = "야누스의 동전";
        quality = ItemQuality.Superior;
        type = AddonType.Prop;
        sprite = Resources.Load<Sprite>("Sprites/Addons/janus coin");
        highlight = Resources.Load<Sprite>("Sprites/Addons/janus coin_border");
        sizeInventory = new Vector2(75, 80);
        addonInfo = "타격시 50% 확률로 2의 피해를 입거나 적의 현재 체력의 25%에 해당하는 추가 피해를 입힌다.";
    }
    public override float DamageFinalAdder(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        LifeStoneManager lifeStoneManager = LifeStoneManager.Instance;
        if(Random.Range(0,2) == 0)
        {
            lifeStoneManager.DestroyStone(2);
            return 0;
        }
        else
        {
            return enemyInfo.CurrHealth * 0.25f;
        }
    }
}