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
    public List<string> Car2Prices { get => _car2Prices; set => _car2Prices = value; }
    public List<string> Car3Prices { get => _car3Prices; set => _car3Prices = value; }

    public void CkeckUpgradesButtons(int levelUpgrade)
    {
      switch(_storeManager.CurrentDisplaying)
        {
            case (0):

                _text.text = _car1Prices[levelUpgrade];
                break;
            case (1):
                _text.text = Car2Prices[levelUpgrade];
                break;
            case (2):
                _text.text = Car3Prices[levelUpgrade];
                break;
            default:
                break;

        }


    }




}
