﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInGame : MonoBehaviour {

    /// <summary>
    /// The enum for door information.
    /// </summary>
    public DoorInfo doorInfo;
    public enum DoorInfo { None, Left, Right, Both };
}