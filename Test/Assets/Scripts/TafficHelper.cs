using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TafficHelper
{
    public enum TurnDirection
    {
        Left,
        Center,
        Right
    }
    [SerializeField] private TurnDirection _turnDirection;
    [SerializeField] private Transform[] _waypointsToCheck;

    private TafficHelper(TurnDirection turnDirection, Transform[] transforms)
    {
        this._turnDirection = turnDirection;
        this._waypointsToCheck = transforms;
    }

}
