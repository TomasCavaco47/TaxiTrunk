using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Client : MonoBehaviour
{
    [SerializeField] string _clientName;
    [SerializeField] Sprite _clientSprite;
    [SerializeField] Mission[] _missions;

    private void OnValidate()
    {
        for (int i = 0; i < _missions.Length; i++)
        {
            if(_missions.Length==0)
            {
                break;
            }
            for (int a = 0; a < _missions[i].DialoguesPickUp.Length; a++)
            {
                if(_missions[i].DialoguesPickUp.Length!=0)
                {
                    switch (_missions[i].DialoguesPickUp[a].WhosTalking)
                    {
                        case WhosTalking.Client:
                            _missions[i].DialoguesPickUp[a].Sprite = _clientSprite;
                            break;
                        case WhosTalking.Vin:
                            break;
                        default:
                            break;
                    }
                }
                

                if (_missions[i].DialoguesInMission.Length != 0)
                {
                    switch (_missions[i].DialoguesInMission[a].WhosTalking)
                    {
                        case WhosTalking.Client:
                            _missions[i].DialoguesInMission[a].Sprite = _clientSprite;
                            break;
                        case WhosTalking.Vin:
                            break;
                        default:
                            break;
                    }
                }
              

                if (_missions[i].DialoguesDestination.Length != 0)
                {
                    switch (_missions[i].DialoguesDestination[a].WhosTalking)
                    {
                        case WhosTalking.Client:
                            _missions[i].DialoguesDestination[a].Sprite = _clientSprite;
                            break;
                        case WhosTalking.Vin:
                            break;
                        default:
                            break;
                    }
                }
               
            }
        }
    }
}
