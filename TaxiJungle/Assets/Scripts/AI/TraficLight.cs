using System.Collections;
using UnityEngine;

public class TraficLight : MonoBehaviour
{
    [SerializeField] private int _ncpCrossing;
    [SerializeField] private bool _npcCanCross;
    [SerializeField] private bool _carCanGo;
   [SerializeField] bool _canStartTimer;
    [SerializeField]float _timer=0;

    public int NcpCrossing { get => _ncpCrossing; set => _ncpCrossing = Mathf.Clamp(value,0,1000); }
    public bool NpcCanCross { get => _npcCanCross; set => _npcCanCross = value; }
    public bool CarCanGo { get => _carCanGo; set => _carCanGo = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
       
        if(_canStartTimer)
        {
            
            if (_timer <= 10) 
            {
                _timer += Time.deltaTime;
            }
            else
            {
                _canStartTimer = false;
                _timer=0;
                StartCoroutine(CarsCanPass());
            }
        }
    }
    IEnumerator CarsCanPass()
    {
        _npcCanCross=false;
        yield return new WaitUntil(() => NcpCrossing == 0);
        //Green Light
        _carCanGo=true;
        yield return new WaitForSeconds(20);
        //Red Light
        _canStartTimer = true;
        _carCanGo=false; 
        _npcCanCross=true;
        Debug.Log(_timer);
        Debug.Log(_canStartTimer);

        
    }
}
