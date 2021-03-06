﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
    public GameObject player;
    /// <summary>
    /// Check if scene is changing now.
    /// </summary>
    public static bool isSceneChanging = false;
    const float cameraXLimit = 7f;
    const float cameraYLimit = 3.5f;
    public Vector3 tetrisCameraCoord = new Vector3(108, 240, -1);
    public const float tetrisCameraSize = 300f;
    public const float inGameCameraSize = 4.5f;
    Vector3 inGameClockCoord = new Vector3(-120, 460, 0);
    Vector3 tetrisClockCoord = new Vector3(-645, 950, 0);
    Vector3 inGameLeftRoomCoord = new Vector3(-120, 380, 0);
    Vector3 tetrisLeftRoomCoord = new Vector3(-645, 875, 0);

    public Vector3 originPos;
    
    private void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        player = GameManager.Instance.player;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (GameManager.gameState == GameState.Ingame)
        {
            FollowPlayer();
            originPos = transform.position;
        }
        else if (GameManager.gameState == GameState.Tetris || GameManager.gameState == GameState.Portal)
        {
            originPos = tetrisCameraCoord;
        }
    }

    /// <summary>
    /// Shake camera.
    /// </summary>
    /// <param name="_amount">Amount of shaking camera.</param>
    /// <returns></returns>
    public IEnumerator CameraShake(float _amount)
    {
        float amount = _amount;
        while (amount > 0)
        {
            transform.position = new Vector3(0.2f * Random.insideUnitCircle.x * amount * GetComponent<Camera>().orthographicSize, 
                Random.insideUnitCircle.y * amount * GetComponent<Camera>().orthographicSize, 0) + originPos;
            amount -= _amount / 40;
            yield return null;
        }
        transform.localPosition = originPos;
    }

    /// <summary>
    /// Change scene between tetris and ingame.
    /// </summary>
    /// <returns></returns>
    public IEnumerator ChangeScene(GameState gameState)
    {
        if(isSceneChanging != true)
        {
            GameObject grid = MapManager.Instance.grid.gameObject;
            float sizeDestination = 0;
            isSceneChanging = true;
            if (gameState == GameState.Ingame)
            {
                GameManager.Instance.minimap.SetActive(true);
                MapManager.Instance.clock.GetComponent<RectTransform>().Translate(inGameClockCoord - tetrisClockCoord);
                GameManager.Instance.leftRoomCount.rectTransform.Translate(inGameLeftRoomCoord - tetrisLeftRoomCoord);
                InventoryManager.Instance.coolUI.GetComponent<CanvasGroup>().alpha = 0.7f;
                GameManager.gameState = GameState.Ingame;
                StartCoroutine(MapManager.Instance.RoomFadeIn(MapManager.currentRoom));
                grid.transform.position = Vector3.zero;
                sizeDestination = inGameCameraSize;
                GetComponent<Camera>().cullingMask = ~0;
                while (GetComponent<Camera>().orthographicSize > sizeDestination + 0.01)
                {
                    yield return null;
                    FollowPlayer();
                    GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, sizeDestination, Mathf.Sqrt(Time.deltaTime));
                }
            }
            else if (gameState == GameState.Tetris || gameState == GameState.Portal)
            {
                GameManager.Instance.minimap.SetActive(false);
                MapManager.Instance.clock.GetComponent<RectTransform>().Translate(tetrisClockCoord - inGameClockCoord);
                GameManager.Instance.leftRoomCount.rectTransform.Translate(tetrisLeftRoomCoord - inGameLeftRoomCoord);
                InventoryManager.Instance.coolUI.GetComponent<CanvasGroup>().alpha = 0;
                if (gameState == GameState.Tetris)
                    GameManager.gameState = GameState.Tetris;
                else if(gameState == GameState.Portal)
                {
                    GameManager.gameState = GameState.Portal;
                    MapManager.portalDestination = MapManager.currentRoom.mapCoord;
                    MapManager.mapGrid[(int)MapManager.portalDestination.x, (int)MapManager.portalDestination.y].portalSurface.GetComponent<SpriteRenderer>().sprite =
                        MapManager.Instance.portalSelected;
                }
                StartCoroutine(MapManager.Instance.RoomFadeOut(MapManager.currentRoom));
                grid.transform.position = new Vector3(0, 0, 2);
                sizeDestination = tetrisCameraSize;
                while (GetComponent<Camera>().orthographicSize < sizeDestination - 2)
                {
                    yield return null;
                    Vector2 coord = Vector2.Lerp(transform.position, tetrisCameraCoord, Mathf.Sqrt(Time.deltaTime));
                    transform.position = new Vector3(coord.x, coord.y, -1);
                    GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, sizeDestination, Mathf.Sqrt(Time.deltaTime));
                }
                GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Tetris") | 1 << LayerMask.NameToLayer("Portal") | 1 << LayerMask.NameToLayer("UI");
                transform.position = tetrisCameraCoord;
            }
            GetComponent<Camera>().orthographicSize = sizeDestination;
            isSceneChanging = false;
        }
    }

    void FollowPlayer()
    {
        float posx = player.transform.position.x;
        float posy = player.transform.position.y;
        if (RoomCol("Up") != -1)
            posy = RoomCol("Up") - cameraYLimit;
        if (RoomCol("Down") != -1)
            posy = RoomCol("Down") + cameraYLimit;
        if (RoomCol("Left") != -1)
            posx = RoomCol("Left") + cameraXLimit;
        if (RoomCol("Right") != -1)
            posx = RoomCol("Right") - cameraXLimit;
        /*if (RoomCol("Left") != -1 && RoomCol("Right") != -1)
        {
            float middle = Player.tx * 24f + 12f;
            if (middle - RoomCol("Left") > 20f)
                posx = RoomCol("Left") + cameraXLimit;
            else if (RoomCol("Right") - middle > 20f)
                posx = RoomCol("Right") - cameraXLimit;
            else
                posx = player.transform.position.x;
            //방의 중심과 비교하여 어느게 더 가까운가
        }*/
        if (MapManager.isRoomFalling != true)
            transform.position = Vector3.Lerp(transform.position, new Vector3(posx, posy, -1), 8f * Time.deltaTime);
        else if (MapManager.isRoomFalling == true)
            transform.position = Vector3.Lerp(transform.position, new Vector3(posx, posy, -1), 0.9f);
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }

    float RoomCol(string direction)
    {
        Vector2 position = player.transform.position;
        switch (direction)
        {
            case "Up":
                if (position.y + cameraYLimit >= MapManager.currentRoom.transform.position.y + MapManager.currentRoom.roomInGame.roomSize.y - 1)
                    return MapManager.currentRoom.transform.position.y + MapManager.currentRoom.roomInGame.roomSize.y - 1;
                break;
            case "Down":
                if (position.y - cameraYLimit <= MapManager.currentRoom.transform.position.y + 1)
                    return MapManager.currentRoom.transform.position.y + 1;
                break;
            case "Left":
                if (position.x - cameraXLimit <= MapManager.currentRoom.transform.position.x + 1)
                    return MapManager.currentRoom.transform.position.x + 1;
                break;
            case "Right":
                if (position.x + cameraXLimit >= MapManager.currentRoom.transform.position.x + MapManager.currentRoom.roomInGame.roomSize.x - 1)
                    return MapManager.currentRoom.transform.position.x + MapManager.currentRoom.roomInGame.roomSize.x - 1;
                break;
        }
        return -1;
    }
}