﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 아이템명: 활
/// 번호: 2
/// </summary>
public class Bow : Item {
    GameObject arrow;
    GameObject player;

	public override void Declare()
    {
        id = 2; name = "활";
        quality = ItemQuality.Study;
        skillNum = 2;
        combo = new string[3] { "BB", "BC", "" };
        attachable = new bool[4] { true, true, true, true };
        sprite = Resources.Load<Sprite>("Sprites/Items/bow");
        highlight = Resources.Load<Sprite>("Sprites/Items/bow_border");
        animation[0] = Resources.Load<AnimationClip>("Animations/bowAttack1");
        animation[1] = Resources.Load<AnimationClip>("Animations/bowAttack2");
        animation[2] = null;
        sizeInventory = new Vector2(137.5f, 137.5f);
        itemInfo = "무난한 원거리 무기. 화살을 쏜 후 경직시간을 조심해야 한다.";
        comboName = new string[3] { "쏘기", "위로 쏘기", "" };

        player = GameManager.Instance.player;
        arrow = Resources.Load<GameObject>("Prefabs/Projectiles/bow_arrow");
    }

    protected override void PlaySkill1()
    {
        player.GetComponent<Player>().StartCoroutine(Shoot1());
    }
    IEnumerator Shoot1()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject tmpObj = Object.Instantiate(arrow, player.transform.position - new Vector3(0.7f * Mathf.Sign(player.transform.localScale.x), 0), Quaternion.identity);
        tmpObj.transform.localScale = new Vector3(Mathf.Sign(player.transform.localScale.x), 1, 1);
        tmpObj.GetComponent<Rigidbody2D>().velocity = new Vector2(-15f * Mathf.Sign(player.transform.localScale.x), 0f);
        tmpObj.GetComponent<AttackProperty>().Init(combo[0]);
    }
    protected override void PlaySkill2()
    {
        player.GetComponent<Player>().StartCoroutine(Shoot2());
    }
    IEnumerator Shoot2()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject tmpObj = Object.Instantiate(arrow, player.transform.position - new Vector3(0.5f * Mathf.Sign(player.transform.localScale.x), -0.8f), Quaternion.Euler(0, 0, -90f));
        tmpObj.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,15f);
        tmpObj.GetComponent<AttackProperty>().Init(combo[1]);
    }
}
