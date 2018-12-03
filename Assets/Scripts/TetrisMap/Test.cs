﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    MapManager MM;
    TetriminoSpawner TS;
    public static Vector3 tetrisCameraCoord = new Vector3(180, 
        0, -1);
    public static float tetrisMapSize = 300;

    public void ChangeTetrimino()
    {
        Destroy(MM.currentTetrimino.gameObject);
        Destroy(MM.currentGhost.gameObject);
        TS.MakeTetrimino();
    }
    public void SpawnBossTetrimino()
    {
        MM.spawnBossTetrimino = true;
    }
    public void Gold()
    {
        MM.UpgradeRoom(MapManager.SpecialRoomType.Gold);
    }
    public void Amethyst()
    {
        MM.UpgradeRoom(MapManager.SpecialRoomType.Amethyst);
    }
    public void BothSide()
    {
        MM.UpgradeRoom(MapManager.SpecialRoomType.BothSide);
    }
    public void Boss()
    {
        SpawnBossTetrimino();
    }

    private void Awake()
    {
        MM = GameObject.Find("MapManager").GetComponent<MapManager>();
        TS = GameObject.Find("TetriminoSpawner").GetComponent<TetriminoSpawner>();
    }
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeTetrimino();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SpawnBossTetrimino();
        /*if (Input.GetKeyDown(KeyCode.Tab) && GameManager.gameState != GameManager.GameState.Tetris)
        {
            GameManager.gameState = GameManager.GameState.Tetris;
            GameObject.Find("Main Camera").transform.position = tetrisCameraCoord;
            GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize = tetrisMapSize;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && GameManager.gameState == GameManager.GameState.Tetris)
            GameManager.gameState = GameManager.GameState.Ingame;*/
    }
}
