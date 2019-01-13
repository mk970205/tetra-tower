﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;

public class LifeStoneTest : MonoBehaviour {

    public GameObject numText, ameNumText, goldPerText, goldText, amethystText, destroyText, rowText;
    public GameObject ui;
    LifeStoneManager uii;

    private void Start()
    {
        uii = ui.GetComponent<LifeStoneManager>();
    }

    public void push()
    {
        uii.PushLifeStone(uii.CreateLifeStoneInfo(Convert.ToInt32(numText.GetComponent<InputField>().text), Convert.ToSingle(goldPerText.GetComponent<InputField>().text), Convert.ToInt32(ameNumText.GetComponent<InputField>().text)));
    }
    public void gold()
    {
        int tmp = Convert.ToInt32(goldText.GetComponent<InputField>().text);
        if (tmp > 0) uii.ChangeFromNormal(2, tmp);
        else if (tmp < 0) uii.ChangeToNormal(2, -tmp);
    }
    public void amethyst()
    {
        int tmp = Convert.ToInt32(amethystText.GetComponent<InputField>().text);
        if (tmp > 0) uii.ChangeFromNormal(3, tmp);
        else if (tmp < 0) uii.ChangeToNormal(3, -tmp);
    }
    public void dest()
    {
        uii.DestroyStone(Convert.ToInt32(destroyText.GetComponent<InputField>().text));
    }
    public void row()
    {
        uii.ExpandRow(Convert.ToInt32(rowText.GetComponent<InputField>().text));
    }
}