﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Room class
/// </summary>
public class Room : MonoBehaviour
{

    /*
     *  variables
     */
    /// <summary>
    /// Room's Location on tetris map.
    /// Not related to real location.
    /// </summary>
    public Vector3 mapCoord;
    /// <summary>
    /// Stage per rooms.
    /// </summary>
    public int stage;
    /// <summary>
    /// Room concept per rooms.
    /// Range from 0 to 3.
    /// </summary>
    public int roomConcept;
    /// <summary>
    /// Item room type per rooms.
    /// 0 for normal room, 1~ for item rooms.
    /// </summary>
    public int itemRoomType;
    /// <summary>
    /// Special room types.
    /// </summary>
    public MapManager.SpecialRoomType specialRoomType;
    /// <summary>
    /// Location of the left door.
    /// </summary>
    public int leftDoorLocation;
    /// <summary>
    /// Location of the right door.
    /// </summary>
    public int rightDoorLocation;
    /// <summary>
    /// Height of side doors.
    /// </summary>
    public int[] doorLocations = { 1, 9, 17 };
    /// <summary>
    /// Fog of the room.
    /// </summary>
    public GameObject fog;
    /// <summary>
    /// Left door of the room.
    /// </summary>
    public GameObject leftTetrisDoor;
    /// <summary>
    /// Right door of the room.
    /// </summary>
    public GameObject rightTetrisDoor;
    /// <summary>
    /// Up door of the room.
    /// </summary>
    public GameObject inGameDoorUp;
    /// <summary>
    /// Down door of the room.
    /// </summary>
    public GameObject inGameDoorDown;
    /// <summary>
    /// Left door of the room.
    /// </summary>
    public GameObject inGameDoorLeft;
    /// <summary>
    /// Right door of the room.
    /// </summary>
    public GameObject inGameDoorRight;
    /// <summary>
    /// Room in InGame of the room.
    /// </summary>
    public RoomInGame roomInGame;
    /// <summary>
    /// Check if room is clear and escapable.
    /// </summary>
    public bool isRoomCleared;
    public bool isUpDoorOpened;
    public bool isDownDoorOpened;
    public bool isLeftDoorOpened;
    public bool isRightDoorOpened;


    /*
     *  functions
     */
    /// <summary>
    /// Select which doors would be opened.
    /// </summary>
    public void SetDoors()
    {
        if (mapCoord.x < MapManager.width - 1 && MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y] != null)
            rightDoorLocation = MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y].leftDoorLocation;
        else
            rightDoorLocation = Random.Range(0, 3);
        if (mapCoord.x > 0 && MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y] != null)
            leftDoorLocation = MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y].rightDoorLocation;
        else
            leftDoorLocation = Random.Range(0, 3);
    }
    /// <summary>
    /// Create doors.
    /// </summary>
    public void CreateDoors(GameObject _leftTetrisDoor, GameObject _rightTetrisDoor, GameObject _inGameDoorUp, GameObject _inGameDoorDown, GameObject _inGameDoorLeft, GameObject _inGameDoorRight)
    {
        float standardSize = MapManager.tetrisMapSize / 24;
        Tilemap outerWallMap = roomInGame.transform.GetChild(4).GetComponent<Tilemap>();
        leftTetrisDoor = Instantiate(_leftTetrisDoor, transform.position + new Vector3(standardSize, doorLocations[leftDoorLocation], 0), Quaternion.identity, transform);
        rightTetrisDoor = Instantiate(_rightTetrisDoor, transform.position + new Vector3(standardSize * 23, doorLocations[rightDoorLocation], 0), Quaternion.identity, transform);

        inGameDoorUp = Instantiate(_inGameDoorUp, transform.position + new Vector3(standardSize * 11, standardSize * 23, 2), Quaternion.identity, transform);
        inGameDoorUp.GetComponent<Animator>().SetInteger("doorPosition", 0);
        inGameDoorDown = Instantiate(_inGameDoorDown, transform.position + new Vector3(standardSize * 11, 0, 2), Quaternion.identity, transform);
        inGameDoorDown.GetComponent<Animator>().SetInteger("doorPosition", 2);
        inGameDoorLeft = Instantiate(_inGameDoorLeft, transform.position + new Vector3(0, doorLocations[leftDoorLocation], 2), Quaternion.identity, transform);
        inGameDoorLeft.GetComponent<Animator>().SetInteger("doorPosition", 3);
        inGameDoorRight = Instantiate(_inGameDoorRight, transform.position + new Vector3(standardSize * 23f, doorLocations[rightDoorLocation], 2), Quaternion.identity, transform);
        inGameDoorRight.GetComponent<Animator>().SetInteger("doorPosition", 1);
        for (int i = 0; i < 3; i++)
        {
            if (i != leftDoorLocation)
            {
                outerWallMap.SetTile(new Vector3Int(0, doorLocations[i] + 1, 0), outerWallMap.GetTile(new Vector3Int(0, 0, 0)));
                outerWallMap.SetTile(new Vector3Int(0, doorLocations[i], 0), outerWallMap.GetTile(new Vector3Int(0, 0, 0)));
            }
            if (i != rightDoorLocation)
            {
                outerWallMap.SetTile(new Vector3Int(23, doorLocations[i] + 1, 0), outerWallMap.GetTile(new Vector3Int(0, 0, 0)));
                outerWallMap.SetTile(new Vector3Int(23, doorLocations[i], 0), outerWallMap.GetTile(new Vector3Int(0, 0, 0)));
            }
        }
    }


    public void OpenDoorTest(GameObject door)
    {
        Animator animator;
        animator = door.GetComponent<Animator>();
        animator.SetBool("doorOpen", true);
        animator.SetBool("doorClose", false);
    }
    public void CloseDoorTest(GameObject door)
    {
        Animator animator;
        animator = door.GetComponent<Animator>();
        animator.SetBool("doorOpen", false);
        animator.SetBool("doorClose", true);
    }



    /// <summary>
    /// Open selected door of this room.
    /// </summary>
    /// <param name="direction">Direction of the door.</param>
    /// <returns></returns>
    public IEnumerator OpenDoor(string direction)
    {
        float standardSize = MapManager.tetrisMapSize / 24;
        for(int i = 0; i < 25; i++)
        {
            switch (direction)
            {
                case "Up":
                    inGameDoorUp.transform.GetChild(0).transform.position += new Vector3(standardSize / 20, 0, 0);
                    inGameDoorUp.transform.GetChild(1).transform.position += new Vector3(-standardSize / 25, 0, 0);
                    MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y + 1].inGameDoorDown.transform.GetChild(0).transform.position += new Vector3(-standardSize / 20, 0, 0);
                    MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y + 1].inGameDoorDown.transform.GetChild(1).transform.position += new Vector3(standardSize / 25, 0, 0);
                    isUpDoorOpened = true;
                    MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y + 1].isDownDoorOpened = true;
                    break;
                case "Down":
                    inGameDoorDown.transform.GetChild(0).transform.position += new Vector3(-standardSize / 20, 0, 0);
                    inGameDoorDown.transform.GetChild(1).transform.position += new Vector3(standardSize / 25, 0, 0);
                    MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y - 1].inGameDoorUp.transform.GetChild(0).transform.position += new Vector3(standardSize / 20, 0, 0);
                    MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y - 1].inGameDoorUp.transform.GetChild(1).transform.position += new Vector3(-standardSize / 25, 0, 0);
                    isDownDoorOpened = true;
                    MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y - 1].isUpDoorOpened = true;
                   break;
                case "Left":
                    inGameDoorLeft.transform.GetChild(0).transform.position += new Vector3(0, standardSize / 20, 0);
                    inGameDoorLeft.transform.GetChild(1).transform.position += new Vector3(0, -standardSize / 25, 0);
                    MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y].inGameDoorRight.transform.GetChild(0).transform.position += new Vector3(0, -standardSize / 20, 0);
                    MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y].inGameDoorRight.transform.GetChild(1).transform.position += new Vector3(0, standardSize / 25, 0);
                    isLeftDoorOpened = true;
                    MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y].isRightDoorOpened = true;
                   break;
                case "Right":
                     inGameDoorRight.transform.GetChild(0).transform.position += new Vector3(0, -standardSize / 20, 0);
                     inGameDoorRight.transform.GetChild(1).transform.position += new Vector3(0, standardSize / 25, 0);
                     MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y].inGameDoorLeft.transform.GetChild(0).transform.position += new Vector3(0, standardSize / 20, 0);
                     MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y].inGameDoorLeft.transform.GetChild(1).transform.position += new Vector3(0, -standardSize / 25, 0);
                     isRightDoorOpened = true;
                     MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y].isLeftDoorOpened = true;
                    break;
            }
            yield return new WaitForSeconds(0.04f);
        }
    }
    /// <summary>
    /// Close selected door of this room.
    /// </summary>
    /// <param name="direction">Direction of the door.</param>
    /// <returns></returns>
    public IEnumerator CloseDoor(string direction)
    {
        float standardSize = MapManager.tetrisMapSize / 24;
        for (int i = 0; i < 25; i++)
        {
            switch (direction)
            {
                case "Up":
                    if(isUpDoorOpened == true)
                    {
                        inGameDoorUp.transform.GetChild(0).transform.position += new Vector3(-standardSize / 20, 0, 0);
                        inGameDoorUp.transform.GetChild(1).transform.position += new Vector3(standardSize / 25, 0, 0);
                        if (mapCoord.y < MapManager.realHeight && MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y + 1] != null && isUpDoorOpened == true)
                        {
                            MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y + 1].inGameDoorDown.transform.GetChild(0).transform.position += new Vector3(standardSize / 20, 0, 0);
                            MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y + 1].inGameDoorDown.transform.GetChild(1).transform.position += new Vector3(-standardSize / 25, 0, 0);
                        }
                    }
                    break;
                case "Down":
                    if(isDownDoorOpened == true)
                    {
                        inGameDoorDown.transform.GetChild(0).transform.position += new Vector3(standardSize / 20, 0, 0);
                        inGameDoorDown.transform.GetChild(1).transform.position += new Vector3(-standardSize / 25, 0, 0);
                        if (mapCoord.y > 0 && MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y - 1] != null && isDownDoorOpened == true)
                        {
                            MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y - 1].inGameDoorUp.transform.GetChild(0).transform.position += new Vector3(-standardSize / 20, 0, 0);
                            MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y - 1].inGameDoorUp.transform.GetChild(1).transform.position += new Vector3(standardSize / 25, 0, 0);
                        }
                    }
                    break;
                case "Left":
                    if(isLeftDoorOpened == true)
                    {
                        inGameDoorLeft.transform.GetChild(0).transform.position += new Vector3(0, -standardSize / 20, 0);
                        inGameDoorLeft.transform.GetChild(1).transform.position += new Vector3(0, standardSize / 25, 0);
                        if (mapCoord.x > 0 && MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y] != null && isLeftDoorOpened == true)
                        {
                            MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y].inGameDoorRight.transform.GetChild(0).transform.position += new Vector3(0, standardSize / 20, 0);
                            MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y].inGameDoorRight.transform.GetChild(1).transform.position += new Vector3(0, -standardSize / 25, 0);
                        }
                    }
                    break;
                case "Right":
                    if(isRightDoorOpened == true)
                    {
                        inGameDoorRight.transform.GetChild(0).transform.position += new Vector3(0, standardSize / 20, 0);
                        inGameDoorRight.transform.GetChild(1).transform.position += new Vector3(0, -standardSize / 25, 0);
                        if (mapCoord.x < MapManager.width - 1 && MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y] != null && isRightDoorOpened == true)
                        {
                            MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y].inGameDoorLeft.transform.GetChild(0).transform.position += new Vector3(0, -standardSize / 20, 0);
                            MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y].inGameDoorLeft.transform.GetChild(1).transform.position += new Vector3(0, standardSize / 25, 0);
                        }
                    }
                    break;
            }
            yield return new WaitForSeconds(0.04f);
        }
        switch (direction)
        {
            case "Up":
                if (isUpDoorOpened == true)
                    isUpDoorOpened = false;
                if (mapCoord.y < MapManager.realHeight && MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y + 1] != null && isUpDoorOpened == true)
                    MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y + 1].isDownDoorOpened = false;
                break;
            case "Down":
                if (isDownDoorOpened == true)
                    isDownDoorOpened = false;
                if (mapCoord.y > 0 && MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y - 1] != null && isDownDoorOpened == true)
                    MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y - 1].isUpDoorOpened = false;
                break;
            case "Left":
                if (isLeftDoorOpened == true)
                    isLeftDoorOpened = false;
                if (mapCoord.x > 0 && MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y] != null && isLeftDoorOpened == true)
                    MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y].isRightDoorOpened = false;
                break;
            case "Right":
                if (isRightDoorOpened == true)
                    isRightDoorOpened = false;
                if (mapCoord.x < MapManager.width - 1 && MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y] != null && isRightDoorOpened == true)
                    MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y].isLeftDoorOpened = false;
                break;
        }
    }
    
    /// <summary>
    /// Clear the cleared room.
    /// </summary>
    public void ClearRoom()
    {
        if(isRoomCleared != true)
        {
            if (mapCoord.y < MapManager.realHeight && MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y + 1] != null && isUpDoorOpened != true)
            {
                OpenDoorTest(inGameDoorUp);
                MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y + 1].OpenDoorTest(MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y + 1].inGameDoorDown);
            }
                //StartCoroutine(OpenDoor("Up"));
            if (mapCoord.y > 0 && MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y - 1] != null && isDownDoorOpened != true)
            {
                OpenDoorTest(inGameDoorDown);
                MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y - 1].OpenDoorTest(MapManager.mapGrid[(int)mapCoord.x, (int)mapCoord.y - 1].inGameDoorUp);
            }
                //StartCoroutine(OpenDoor("Down"));
            if (mapCoord.x > 0 && MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y] != null && isLeftDoorOpened != true)
            {
                OpenDoorTest(inGameDoorLeft);
                MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y].OpenDoorTest(MapManager.mapGrid[(int)mapCoord.x - 1, (int)mapCoord.y].inGameDoorRight);
            }
                //StartCoroutine(OpenDoor("Left"));
            if (mapCoord.x < MapManager.width - 1 && MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y] != null && isRightDoorOpened != true)
            {
                OpenDoorTest(inGameDoorRight);
                MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y].OpenDoorTest(MapManager.mapGrid[(int)mapCoord.x + 1, (int)mapCoord.y].inGameDoorLeft);
            }
                //StartCoroutine(OpenDoor("Right"));
        }
        isRoomCleared = true;

        //Need to make extra works.
    }
}
