﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 애드온명: 소형 렌즈
/// 번호: 18
/// </summary>
public class SmallLens : Addon
{
    public override void Declare()
    {
        id = 18; name = "소형 렌즈";
        quality = ItemQuality.Ordinary;
        type = AddonType.Component;
        sprite = Resources.Load<Sprite>("Sprites/Addons/parchment piece"); ;
        highlight = Resources.Load<Sprite>("Sprites/Addons/parchment piece"); ;
        sizeInventory = new Vector2(80, 80);
    }

    public override void OtherEffect(string combo)
    {
        GameObject.Find("Player").GetComponent<PlayerAttack>().comboTime *= 1.75f;
    }
}