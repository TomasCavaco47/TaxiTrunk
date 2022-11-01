using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] CellPhone _cP;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _cP.UseCellPhone();
    }
    
}
