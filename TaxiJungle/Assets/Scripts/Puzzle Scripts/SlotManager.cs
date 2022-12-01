using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotManager : MonoBehaviour
{
    [SerializeField] GameObject _currentMovingItem;
    public static SlotManager instance;
    //[SerializeField] SlotScript _lastslot;
    //private IntVector2 otherItemPos = IntVector2.zero;


    public GameObject CurrentMovingItem { get => _currentMovingItem; set => _currentMovingItem = value; }
    //public IntVector2 OtherItemPos { get => otherItemPos; set => otherItemPos = value; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);

        }
        else
        {
            instance = this;
        }
    }

 
}
