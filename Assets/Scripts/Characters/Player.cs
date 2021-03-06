﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    LifeStoneManager lifeStoneManager;
    public static int tx, ty;
    public static float X = 0.7f, Y = 1.6f;
    public int ttx;
    public int tty;

    public GameObject particlePrefab;
    GameObject[] particles;
    bool gameover;


    // Use this for initialization
    void Start () {
        ttx = (int)(transform.position.x / 24f);
        tty = (int)(transform.position.y - 0.8f / 24f);
        lifeStoneManager = LifeStoneManager.Instance;


        particles = new GameObject[40];
        for(int i=0; i<particles.Length; i++)
        {
            particles[i] = Instantiate(particlePrefab,transform);
            particles[i].SetActive(false);
        }
        gameover = false;
    }
	
	// Update is called once per frame
	void Update () {
        tx = (int)(transform.position.x / 24f);
        ty = (int)((transform.position.y - 0.8f) / 24f);
        if ((ttx != tx || tty != ty) && MapManager.isRoomFalling != true)
        {
            MapManager.tempRoom = MapManager.mapGrid[tx, ty];
            if (tx < ttx)
            {
                MapManager.currentRoom.CloseDoor("Left", true);
            }
            else if (tx > ttx)
            {
                MapManager.currentRoom.CloseDoor("Right", true);
            }
            else if (ty < tty)
            {
                MapManager.currentRoom.CloseDoor("Down", true);
            }
            else if (ty > tty)
            {
                transform.position += new Vector3(0, 0.5f, 0);
                MapManager.currentRoom.CloseDoor("Up", true);
            }
        }
        ttx = tx;
        tty = ty;
        if (!gameover && lifeStoneManager.CountType() == 0)
        {
            gameover = true;
            StartCoroutine(GameOverCoroutine());
        }
            
	}

    IEnumerator GameOverCoroutine()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        foreach(GameObject obj in particles)
        {
            obj.SetActive(true);
            obj.transform.localPosition = Vector3.zero;
            obj.GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle.normalized * Random.Range(3f,7f);
        }
        yield return new WaitForSeconds(3f);
        GameManager.gameState = GameState.GameOver;
    }
    
}
