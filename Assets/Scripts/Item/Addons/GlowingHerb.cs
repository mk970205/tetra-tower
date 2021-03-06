﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 애드온명: 이글거리는 약초
/// 번호: 14
/// </summary>
public class GlowingHerb : Addon
{
    public override void Declare()
    {
        id = 14; name = "이글거리는 약초";
        quality = ItemQuality.Ordinary;
        type = AddonType.Matter;
        sprite = Resources.Load<Sprite>("Sprites/Addons/glowing herb"); ;
        highlight = Resources.Load<Sprite>("Sprites/Addons/glowing herb_border"); ;
        sizeInventory = new Vector2(77.5f, 75);
        addonInfo = "적을 2초간 불태운다.";
    }

    public override float[] DebuffAdder(PlayerAttackInfo attackInfo, Enemy enemyInfo, string combo)
    {
        float[] varArray = new float[(int)EnemyDebuffCase.END_POINTER];
        for (int i = 0; i < (int)EnemyDebuffCase.END_POINTER; i++) varArray[i] = 0f;

        varArray[(int)EnemyDebuffCase.Fire] = 2f;

        return varArray;
    }
}
