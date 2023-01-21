using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Client
{
    [SerializeField] string _clientName;
    [SerializeField] Sprite _clientSprite;

    public string ClientName { get => _clientName; set => _clientName = value; }
    public Sprite ClientSprite { get => _clientSprite; set => _clientSprite = value; }
}
public class DataBase : MonoBehaviour
{
    [SerializeField] List<Transform> _places;
    [SerializeField] List<Client> _clients;
    [SerializeField] Sprite _vinSprite;


    public List<Transform> Places { get => _places; set => _places = value; }
    public List<Client> Clients { get => _clients; set => _clients = value; }
    public Sprite VinSprite { get => _vinSprite; set => _vinSprite = value; }
}
