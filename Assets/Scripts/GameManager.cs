﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public enum GameState { MainMenu, Ingame, Tetris, Pause, Inventory, GameOver }
    /// <summary>
    /// Which state this game is.
    /// change later
    /// </summary>
    public static GameState gameState;

    // Use this for initialization
    void Start () {
        gameState = GameState.Ingame;
        GameObject.Find("TetriminoSpawner").GetComponent<TetriminoSpawner>().MakeInitialTetrimino();
        Vector2 coord = MapManager.currentRoom.transform.position;
        GameObject.Find("Player").transform.position = new Vector2(coord.x, coord.y) + new Vector2(3, 3);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && CameraController.isSceneChanging != true)
        {
            if (gameState == GameState.Ingame)
                gameState = GameState.Tetris;
            else if (gameState == GameState.Tetris)
                gameState = GameState.Ingame;
            StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().ChangeScene());
        }
    }
}
