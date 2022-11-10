using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Client : MonoBehaviour
{
    [SerializeField] string _clientName;
    [SerializeField] Sprite _clientSprite;
    [SerializeField] Sprite _vinSprite;
    [SerializeField] List<Mission> _missionsArcOne;
    [SerializeField] List<Mission> _missionsArcTwo;
    [SerializeField] List<Mission> _missionsArcThree;

    public List<Mission> MissionsArcOne { get => _missionsArcOne; set => _missionsArcOne = value; }

    private void OnValidate()
    {
        for (int i = 0; i < MissionsArcOne.Count; i++)
        {
            if(MissionsArcOne.Count==0)
            {
                break;
            }
            for (int a = 0; a < MissionsArcOne[i].DialoguesPickUp.Length; a++)
            {
                if(MissionsArcOne[i].DialoguesPickUp.Length!=0)
                {
                    switch (MissionsArcOne[i].DialoguesPickUp[a].WhosTalking)
                    {
                        case WhosTalking.Client:
                            MissionsArcOne[i].DialoguesPickUp[a].Sprite = _clientSprite;
                            break;
                        case WhosTalking.Vin:
                            MissionsArcOne[i].DialoguesPickUp[a].Sprite = _vinSprite;
                            break;
                        default:
                            break;
                    }
                }
                

                if (MissionsArcOne[i].DialoguesInMission.Length != 0)
                {
                    switch (MissionsArcOne[i].DialoguesInMission[a].WhosTalking)
                    {
                        case WhosTalking.Client:
                            MissionsArcOne[i].DialoguesInMission[a].Sprite = _clientSprite;
                            break;
                        case WhosTalking.Vin:
                            break;
                        default:
                            break;
                    }
                }
              

                if (MissionsArcOne[i].DialoguesDestination.Length != 0)
                {
                    switch (MissionsArcOne[i].DialoguesDestination[a].WhosTalking)
                    {
                        case WhosTalking.Client:
                            MissionsArcOne[i].DialoguesDestination[a].Sprite = _clientSprite;
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
