using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType
{
    AtoB,
    Tetris,
    Coffee
}
[Serializable]

public class Mission
{
    [Dropdown("_clients", "_clientName")]
    [SerializeField] Client _client;
    [SerializeField] private MissionType _missionType;

    List<GameObject> _placess;
    [Dropdown("_places")]
    [SerializeField] private GameObject _origin;

    
    [Dropdown("_places")]
    [SerializeField] private GameObject _destination;


    [SerializeField] private int _reward;
    [SerializeField] private Dialogue[] _dialoguesPickUp;
    [SerializeField] private Dialogue[] _dialoguesInMission;
    [SerializeField] private Dialogue[] _dialoguesDestination;



    public Dialogue[] DialoguesPickUp { get => _dialoguesPickUp; set => _dialoguesPickUp = value; }
    public Dialogue[] DialoguesInMission { get => _dialoguesInMission; set => _dialoguesInMission = value; }
    public Dialogue[] DialoguesDestination { get => _dialoguesDestination; set => _dialoguesDestination = value; }
    public MissionType MissionType { get => _missionType; set => _missionType = value; }
    public GameObject Origin { get => _origin; set => _origin = value; }
    public GameObject Destination { get => _destination; set => _destination = value; }
    public Client Client { get => _client; set => _client = value; }
}

public enum WhosTalking
{
    Client,
    Vin
}
[Serializable]
public struct Dialogue
{
  
    [SerializeField] private WhosTalking _whosTalking;
    [SerializeField] private Sprite sprite;
    [SerializeField] private string _text;

    public Sprite Sprite { get => sprite; set => sprite = value; }
    public WhosTalking WhosTalking { get => _whosTalking; set => _whosTalking = value; }
    public string Text { get => _text; set => _text = value; }
}




