﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof(RectTransform))]
public class LifeStoneManager : MonoBehaviour {
    /// <summary>
    /// Location of lifeStoneFrame on Canvas
    /// </summary>
    public Vector2 lifeStoneLocation;
    /// <summary>
    /// standard prefab of every image
    /// </summary>
    public GameObject standardImage;
    /// <summary>
    /// The number of lifeStoneRows
    /// </summary>
    public int lifeStoneRowNum;
    /// <summary>
    /// Size of lifeStone
    /// </summary>
    public float lifeStoneSize;
    /// <summary>
    /// The sprites
    /// </summary>
    public Sprite[] sprites;
    /// <summary>
    /// super Object of frames
    /// </summary>
    public GameObject frameSuper;
    /// <summary>
    /// super Object fo stones
    /// </summary>
    public GameObject stoneSuper;
	/// <summary>
	/// lifeStoneUnit Prefab
	/// </summary>
	public GameObject lifeUnitPrefab;
	/// <summary>
	/// strength of vibration when Lifestone falls
	/// </summary>
	public float vibrationVariable;
    /// <summary>
    /// Array of lifestone
    /// 0 row is the bottom
    /// 0: empty
    /// 1: normal lifestone
    /// 2: gold lifestone
    /// 3: amethyst lifestone
    /// </summary>
    public int[,] lifeStoneArray;
	/// <summary>
	/// Array of lifestone GameObject
	/// </summary>
	[HideInInspector]public GameObject[,] lifeStoneUnit;
    [HideInInspector]public LifeStoneFrame lifeStoneFrame;

	public GameObject droppedLifeStonePrefab;

	void Start () {
        transform.position = new Vector3(lifeStoneLocation.x, lifeStoneLocation.y, 0);
        lifeStoneFrame = new LifeStoneFrame(frameSuper.transform, standardImage, lifeStoneRowNum, lifeStoneSize, sprites);
        lifeStoneArray = new int[50, 3];
		lifeStoneUnit = new GameObject[50, 3];
        for (int i = 0; i < 50; i++) for (int j = 0; j < 3; j++) lifeStoneArray[i, j] = 0;
		StartCoroutine("TestEnumerator");
	}
	IEnumerator TestEnumerator()
	{

        PushLifeStone(CreateLifeStoneInfo(5, 0.2f, 3));
        yield return new WaitForSeconds(2);
        PushLifeStone(CreateLifeStoneInfo(3, 0.2f, 0));
        yield return new WaitForSeconds(2);
        PushLifeStone(CreateLifeStoneInfo(4, 0.2f, 0));
        yield return new WaitForSeconds(2);
        InstantiateDroppedLifeStone(CreateLifeStoneInfo(4, 0.1f, 0), GameObject.Find("Player").transform.position + new Vector3(2,2,0));
        /*PushLifeStone(new LifeStoneInfo(new Vector2Int(3, 8), "AAAAAAAAAAAAAAAAAAAAAAAA"));
		PushLifeStone(new LifeStoneInfo(new Vector2Int(2, 5), " AAAABA A "));
		yield return new WaitForSeconds(2);
		PushLifeStone(new LifeStoneInfo(new Vector2Int(2, 3), " AAA A"));
		yield return new WaitForSeconds(2);
		ChangeFromNormal(2, 5);
		yield return new WaitForSeconds(2);
		ChangeToNormal(2, 3);
		yield return new WaitForSeconds(2);
		DestroyStone(3);*/
    }
    public void InstantiateDroppedLifeStone(LifeStoneInfo info, Vector3 pos)
    {
        GameObject tmpObj = Instantiate(droppedLifeStonePrefab);
        tmpObj.GetComponent<DroppedLifeStone>().Init(info, pos);
    }
    public LifeStoneInfo CreateLifeStoneInfo(Vector2Int size, int num, float goldPer, int ameNum)
    {
        System.Random rnd = new System.Random();
        num = Mathf.Max(1, num);
        size.y = Mathf.Min(3, size.y);
        if (num >= size.x * size.y)
            return CreateLifeStoneInfo(size, goldPer, ameNum);

        int[,] tmpArray = new int[size.y, size.x] ;
        for (int j = 0; j < size.y; j++)
            for (int i = 0; i < size.x; i++)
                tmpArray[j, i] = 0;

        tmpArray[rnd.Next(size.y), rnd.Next(size.x)] = 1;

        //making shape of lifestone
        for(int n = 1; n < num; n++)
        {
            ArrayList candArray = new ArrayList();
            for (int j = 0; j < size.y; j++)
                for (int i = 0; i < size.x; i++)
                    //check if adjacent cell is lifestone
                    if(tmpArray[j,i] == 0 &&
                        (j - 1 >= 0 && tmpArray[j - 1, i] == 1 ||
                        j + 1 < size.y && tmpArray[j + 1, i] == 1 ||
                        i - 1 >= 0 && tmpArray[j, i - 1] == 1 ||
                        i + 1 < size.x && tmpArray[j, i + 1] == 1))
                        candArray.Add(new Vector2Int(i, j));
            if (candArray.Count == 0) break;
            Vector2Int vtmp = (Vector2Int)candArray[rnd.Next(candArray.Count)];
            tmpArray[vtmp.y, vtmp.x] = 1;
        }

        //recalibrate the size
        Vector2Int maxPoint = new Vector2Int(-1, -1);
        Vector2Int minPoint = new Vector2Int(size.x + 1, size.y + 1);
        for (int j = 0; j < size.y; j++)
            for (int i = 0; i < size.x; i++)
                if(tmpArray[j,i] == 1)
                {
                    maxPoint.x = Mathf.Max(i, maxPoint.x);
                    maxPoint.y = Mathf.Max(j, maxPoint.y);
                    minPoint.x = Mathf.Min(i, minPoint.x);
                    minPoint.y = Mathf.Min(j, minPoint.y);
                }
        size = maxPoint - minPoint + Vector2Int.one;

        //making fill string
        string fill = "";
        for (int j = minPoint.y; j <= maxPoint.y; j++)
            for (int i = minPoint.x; i <= maxPoint.x; i++)
                if (tmpArray[j, i] == 1) fill += 'A';
                else fill += ' ';

        //change to amethyst
        ArrayList sCandArray = new ArrayList();
        for (int i = 0; i < fill.Length; i++)
            if (fill[i] == 'A')
                sCandArray.Add(i);
        char[] repChar = fill.ToCharArray();
        for(int i = 0; i < ameNum && sCandArray.Count > 0; i++)
        {
            int tmp = rnd.Next(sCandArray.Count);
            repChar[(int)sCandArray[tmp]] = 'C';
            sCandArray.RemoveAt(tmp);
        }
        for (int i = 0; i < fill.Length; i++)
            if (repChar[i] == 'A' && Random.Range(0f, 1f) < goldPer)
                repChar[i] = 'B';
        fill = new string(repChar);
        
        return new LifeStoneInfo(size, fill);

    }
    public LifeStoneInfo CreateLifeStoneInfo(Vector2Int size, float goldPer, int ameNum)
    {
        return CreateLifeStoneInfo(size, size.x * size.y, goldPer, ameNum);
    }
    public LifeStoneInfo CreateLifeStoneInfo(int num, float goldPer, int ameNum)
    {
        return CreateLifeStoneInfo(new Vector2Int(3, 20), num, goldPer, ameNum);
    }
    public LifeStoneInfo CreateLifeStoneInfo(LifeStoneInfo lifeStoneInfo)
    {
        return lifeStoneInfo;
    }

	/// <summary>
	/// push LifeStone in LifeStoneFrame
	/// </summary>
	/// <param name="pushInfo"></param>
	void PushLifeStone(LifeStoneInfo pushInfo)
	{
		System.Random rnd = new System.Random();
		Vector2Int pSize = pushInfo.getSize();
		string pFill = pushInfo.getFill();
		int[] minRow = new int[] { lifeStoneRowNum, lifeStoneRowNum, lifeStoneRowNum };
		int selectedCol = 0, selectedRow = lifeStoneRowNum;
		ArrayList selColCand = new ArrayList();
		{
			int i, j, pi, pj;
			for (i = 0; i <= 3 - pSize.x; i++)
			{
				for (j = lifeStoneRowNum - 1; j >= 0; j--)
				{
					for (pi = 0; pi < pSize.x; pi++)
					{
						for (pj = 0; pj < pSize.y; pj++)
						{
							if (pFill[pj * pSize.x + pi] != ' ' && lifeStoneArray[j + pj, i + pi] != 0) break;
						}
						if (pj != pSize.y) break;
					}
					if (pi != pSize.x) break;
					minRow[i] = j;
				}
			}
		}

		for (int i = 0; i <= 3 - pSize.x; i++)
			if (minRow[i] < selectedRow) selectedRow = minRow[i];

		for (int i = 0; i <= 3 - pSize.x; i++)
			if (minRow[i] == selectedRow) selColCand.Add(i);


		selectedCol = (int)selColCand[rnd.Next(selColCand.Count)];

		float vibration = pushInfo.getAmount() * vibrationVariable * lifeStoneSize;
		for (int pj = 0; pj < pSize.y; pj++)
		{
			if (selectedRow + pj >= lifeStoneRowNum) break;
			for (int pi = 0; pi < pSize.x; pi++)
				if (pFill[pj * pSize.x + pi] != ' ')
				{
					int xtmp = selectedCol + pi, ytmp = selectedRow + pj;
					lifeStoneArray[ytmp, xtmp] = pFill[pj * pSize.x + pi] - 'A' + 1;
					lifeStoneUnit[ytmp, xtmp] = Instantiate(lifeUnitPrefab, stoneSuper.transform);

					lifeStoneUnit[ytmp, xtmp].GetComponent<LifeUnitInFrame>().Init(
						lifeStoneArray[ytmp, xtmp], 
						lifeStoneSize, 
						new Vector2Int(xtmp, ytmp), 
						new Vector2Int(xtmp, lifeStoneRowNum + pj), 
						new Vector2(0.2f * lifeStoneSize, 0.2f * lifeStoneSize),
						vibration);
					vibration = 0;
				}
		}
	}

	public int CountType(int type)
	{
		int count = 0;
		for (int j = 0; j < 3; j++)
			for (int i = 0; i < lifeStoneRowNum; i++)
				if (lifeStoneArray[j, i] == type)
					count++;
		return count;
	}

	public void DestroyStone(int num)
	{
		System.Random rnd = new System.Random();
		ArrayList candArray = new ArrayList();
		for (int i = 0; i < num; i++)
		{
			for(int pj = lifeStoneRowNum-1; pj>=0;pj--)
			{
				ArrayList sCandArray = new ArrayList();
				for(int pi = 0; pi < 3; pi++)
				{
					if (lifeStoneArray[pj, pi] != 0)
						sCandArray.Add(new Vector2Int(pi, pj));
				}

				if (sCandArray.Count > 0)
				{
					int tmp = rnd.Next(sCandArray.Count);
					Vector2Int vtmp = (Vector2Int)sCandArray[tmp];
					candArray.Add(vtmp);
					lifeStoneArray[vtmp.y, vtmp.x] = 0;
					break;
				}
			}
		}
		StartCoroutine(DestroyInPhase(candArray));
	}

	IEnumerator DestroyInPhase(ArrayList candArray)
	{
		for (int i = 0; i < candArray.Count; i++)
		{
			Vector2Int vtmp = (Vector2Int)candArray[i];
			lifeStoneUnit[vtmp.y, vtmp.x].GetComponent<LifeUnitInFrame>().unitDestroy();
			lifeStoneUnit[vtmp.y, vtmp.x] = null;
			yield return new WaitForSeconds(0.1f);
		}
	}

	public void ChangeFromNormal(int type, int num)
	{
		System.Random rnd = new System.Random();
		ArrayList candArray = new ArrayList();
		for (int j = 0; j < lifeStoneRowNum; j++)
			for (int i = 0; i < 3; i++)
				if (lifeStoneArray[j, i] == 1)
					candArray.Add(new Vector2Int(i, j));
		while (candArray.Count > num)
			candArray.RemoveAt(rnd.Next(candArray.Count));
		for(int i=0; i<candArray.Count; i++)
		{
			Vector2Int vtmp = (Vector2Int)candArray[i];
			lifeStoneArray[vtmp.y, vtmp.x] = type;
		}
		StartCoroutine(ChangeInPhase(candArray,type));
	}

	public void ChangeToNormal(int type, int num)
	{
		System.Random rnd = new System.Random();
		ArrayList candArray = new ArrayList();
		for (int j = 0; j < lifeStoneRowNum; j++)
			for (int i = 0; i < 3; i++)
				if (lifeStoneArray[j, i] == type)
					candArray.Add(new Vector2Int(i, j));
		while (candArray.Count > num)
			candArray.RemoveAt(rnd.Next(candArray.Count));
		for (int i = 0; i < candArray.Count; i++)
		{
			Vector2Int vtmp = (Vector2Int)candArray[i];
			lifeStoneArray[vtmp.y, vtmp.x] = 1;
		}
		StartCoroutine(ChangeInPhase(candArray, 1));
	}

	IEnumerator ChangeInPhase(ArrayList candArray, int type)
	{
		System.Random rnd = new System.Random();
		while (candArray.Count > 0)
		{
			int tmp = rnd.Next(candArray.Count);
			Vector2Int vtmp = (Vector2Int)candArray[tmp];
			lifeStoneUnit[vtmp.y, vtmp.x].GetComponent<LifeUnitInFrame>().ChangeType(type);
			candArray.RemoveAt(tmp);
			yield return new WaitForSeconds(0.1f);
		}
	}

	public IEnumerator vibrateEnumerator(float vibration)
	{
		while(vibration > lifeStoneSize * 0.05f)
		{
			Vector2 tmpVector = Random.insideUnitCircle;
			transform.position = new Vector3(lifeStoneLocation.x + tmpVector.x * vibration * 0.3f, lifeStoneLocation.y + tmpVector.y * vibration, 0);
			vibration *= 0.8f;
			yield return null;
		}
		transform.position = new Vector3(lifeStoneLocation.x, lifeStoneLocation.y, 0);
	}		
}
