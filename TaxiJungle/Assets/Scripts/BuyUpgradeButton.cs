using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyUpgradeButton : MonoBehaviour
{
    [SerializeField] List<int> _carPricesList1;
    [SerializeField] List<int> _carPricesList2;
    [SerializeField] List<int> _car3PricesList3;
  
    [SerializeField] Text _text;
    [SerializeField] StoreManager _storeManager;

    

    public List<int> Car1Prices { get => _carPricesList1; set => _carPricesList1 = value; }
    public List<int> Car2Prices { get => _carPricesList2; set => _carPricesList2 = value; }
    public List<int> Car3Prices { get => _car3PricesList3; set => _car3PricesList3 = value; }
 

    public void ChangePriceInTheButton(int levelUpgrade)
    {
      switch(_storeManager.CurrentDisplaying)
        {
            case (0):
                
                _text.text = _carPricesList1[levelUpgrade].ToString() + "$";
                break;
            case (1):
               
                _text.text = Car2Prices[levelUpgrade].ToString() + "$";
                break;
            case (2):

                _text.text = Car3Prices[levelUpgrade].ToString() + "$";
                break;
            default:
                break;

        }


    }




}
