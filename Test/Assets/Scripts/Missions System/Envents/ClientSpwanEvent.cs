using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSpwanEvent : MonoBehaviour
{
    CellPhone _cP;
    private void Awake()
    {
        _cP = GetComponent<CellPhone>();
    }

    void OnEnable()
    {
        _cP.PassagerSpwanEvent += SpawnPasseger;
    }
    void OnDisable()
    {
        _cP.PassagerSpwanEvent -= SpawnPasseger;
    }

    [SerializeField] int _whichClient = 0;
    [SerializeField] GameObject[] _clients;
    [SerializeField] int _wereTheClientIs = 0;
    [SerializeField] Transform[] _clientLocalization;
    void SpawnPasseger()
    {
        if (_whichClient >= _clients.Length || _whichClient < 0)
        {
            _whichClient = 0;
        }

        if (_wereTheClientIs >= _clientLocalization.Length || _wereTheClientIs < 0)
        {
            _wereTheClientIs = 0;
        }

        GameObject client = Instantiate(_clients[_whichClient]);
        _wereTheClientIs = Random.Range(0, _clientLocalization.Length);
        client.transform.position = _clientLocalization[_wereTheClientIs].position;
        Debug.Log(client);
        
    }

}
