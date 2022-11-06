using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickMissionStarter : MonoBehaviour
{
    
    /// /////////////////////////////////////////////////////////////////////////////////
   /////////////////// ///////////// // /////////////////// // //////////////////////////
    private CellPhone _cellPhone;

    [SerializeField] int _whichClient = 0;
    [SerializeField] GameObject[] _clients; /////////////////
    [SerializeField] int _wereTheClientIs = 0;
    [SerializeField] Transform[] _clientLocalization;
    private void Awake()
    {
        _cellPhone = GetComponent<CellPhone>();
    }

    void OnEnable()
    {
        _cellPhone.StartQuickMission += StartQuickMisson;
    }
    void OnDisable()
    {
        _cellPhone.StartQuickMission -= StartQuickMisson;
    }
    void StartQuickMisson()
    {
        GameObject client = Instantiate(_clients[_whichClient]);
        _wereTheClientIs = Random.Range(0, _clientLocalization.Length);
        client.transform.position = _clientLocalization[_wereTheClientIs].position;
        Debug.Log(client);
        
    }

}
