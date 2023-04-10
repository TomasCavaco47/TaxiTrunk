using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyUpgradeButton : MonoBehaviour
{
    [SerializeField] private List<string> _car1Prices;
    [SerializeField] private List<string> _car2Prices;
    [SerializeField] private List<string> _car3Prices;
    [SerializeField] Text _text;
    [SerializeField] StoreManager _storeManager;

    public List<string> Car1Prices { get => _car1Prices; set => _car1Prices = value; }

    public void CkeckUpgradesButtons(int levelUpgrade)
    {
      switch(_storeManager.CurrentDisplaying)
        {
            case (0):

                _text.text = _car1Prices[levelUpgrade];
                break;
            case (1):
                _text.text = _car2Prices[levelUpgrade];
                break;
            case (2):
                _text.text = _car3Prices[levelUpgrade];
                break;
            default:
                break;

        }


    }




}
