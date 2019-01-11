﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


public class EnemyManager : Singleton<EnemyManager>
{
    // static variable
    // about action
    public enum State
    {
        Idle,
        Track,
        Attack
    } // 상속을 통해 수정할 가능성 높음. 염두만 해 두자.

    public enum ItemType { None, OneStone, TwoStone, ThreeStone, FourStone,
        FiveStone, GoldPotion, AmethystPotion, CommonItem, RareItem,
        EpicItem, LegendaryItem, CommonAdd, RareAdd, EpicAdd,
        LegendaryAdd }

    public delegate void Action();


    // data
    // dictionary
    public readonly Dictionary<int, Dictionary<ItemType, int>> dropTableByID;
    public readonly Dictionary<int, Dictionary<State, Action>> actionDictByID;


    // method
    // constructor
    protected EnemyManager()
    {
        string dropTableDataPath = "";
        string actionTableDataPath = "";

        LoadDropTable(dropTableDataPath);
    }

    // Load Dictionary
    private void LoadDropTable(string dataPath)
    {
        StreamReader strReader = new StreamReader(dataPath, Encoding.UTF8);
        string[] cellValue = null;
        string tableLine = null;
        strReader.ReadLine();
        Dictionary<ItemType, int> dropItemInfo = new Dictionary<ItemType, int>();

        while ((tableLine = strReader.ReadLine()) != null)
        {
            if (string.IsNullOrEmpty(tableLine)) return;

            cellValue = tableLine.Split(',');

            int enemyID = -1;
            int[] weight = {-1};
            int sum = 0;
            int[] cumulatedWeight = { -1 };

            int.TryParse(cellValue[0], out enemyID);
            for(int i=1;i<17;i++)
            {
                int.TryParse(cellValue[i+1], out weight[i]);
            }

            for(int i=0;i<16;i++)
            {
                sum += weight[i];
                cumulatedWeight[i] = sum;
            }
            
            for(int i=0;i<16;i++)
            {
                dropItemInfo.Add((ItemType)i, cumulatedWeight[i]);
            }

            
            dropTableByID.Add(enemyID, dropItemInfo);
        }
    }

   

    // Load ALL CSV Data
    private void LoadEnemyData(string dataPath)
    {
        StreamReader strReader = new StreamReader(dataPath, Encoding.UTF8);
        string[] cellValue = null;
        string tableLine = null;
        strReader.ReadLine();

        while ((tableLine = strReader.ReadLine()) != null)
        {
            cellValue = tableLine.Split(',');
        }
    }
}
