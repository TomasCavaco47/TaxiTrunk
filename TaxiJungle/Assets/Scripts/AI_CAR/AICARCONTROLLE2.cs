using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AICARCONTROLLE2 : MonoBehaviour
{
    enum TurnDirection
    {
        Left,
        Center,
        Right
    }
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _distanceToWaypoint;
    [Range(0,360)]
    [SerializeField] private float _angle;
    [SerializeField] private LayerMask _waypointsLayer;
    [SerializeField] private LayerMask _aiCarLayer;
    [SerializeField] private LayerMask _playerCarLayer;
    [SerializeField] private TurnDirection _turnDirection;

    [SerializeField] private WheelCollider[] wheels;
    [SerializeField] private float _totalPower;
    [SerializeField] private float _vertical, _horizontal;
    private bool _tractionControl = true;
    private bool _canMove = true;

    private float _antiRoll = 1000f;
    private int _speed;
    //[SerializeField] private GameObject _pointToCastForward;
    //[SerializeField] private GameObject _pointToCastLeft;


    private float _radius = 8;
    //[SerializeField] private WayPoints _wayPoints;
    [SerializeField] private Transform _currentWaypoint;
    [SerializeField] private Transform _nextWaypoint;
    [SerializeField] private Transform _turnWaypoint;
    private Rigidbody _rb;
    [SerializeField] private bool _carInFront;
    [SerializeField] private bool _canTurn;
    [SerializeField] private Transform _checkLeft;
    [SerializeField] private Transform _checkRight;
    [SerializeField] private Transform _checkFront;
    [SerializeField] private int _numberOfcarsPassing;
    [SerializeField] private int _numberOfcarsStoped;
    [SerializeField] private List<GameObject> _carsStopedInFront;
    [SerializeField] private bool _front;
    [SerializeField] private bool _side;

    public int Speed { get => _speed; set => _speed = value; }
    public float DistanceToWaypoint { get => _distanceToWaypoint; set => _distanceToWaypoint = value; }

    //[SerializeField] private bool _hasCheckTurn = false;
    //private float _angleToTurn;
    //[SerializeField] private Transform _checkPointToCheckCar;
    //[SerializeField] private Transform _checkPointToCheckCar2;

    //Vector3 fwd;
    //Vector3 left;
    //Vector3 right;

    //private RaycastHit objectHit;


    private void Awake()
    {
        _rb=GetComponent<Rigidbody>();
    }
    private void Start()
    {
        

    }
    private void Update()
    {
        if(_carsStopedInFront != null)
        {

        }
        //Debug.Log(Speed);
        if (_currentWaypoint == null)
        {
            float timer = +Time.deltaTime;
            if (timer > 3)
            {
                Debug.Log("1");
                List<Collider> wayPoints = new List<Collider>();
                wayPoints = Physics.OverlapSphere(_checkFront.position, 20, _waypointsLayer).ToList();
                Transform newWaypoint = null;
                float newWaypointCurrentAngle = 0;
                for (var i = 0; i < wayPoints.Count; i++)
                {
                    Transform tempTarget = wayPoints[i].transform;
                    Vector3 dir = tempTarget.position - _checkLeft.position; // find target direction
                    Vector3 yourDir = tempTarget.forward;
                    float yourAngle = Vector3.Angle(yourDir, -dir);
                    if (newWaypoint == null)
                    {
                        newWaypoint = wayPoints[i].transform;
                        newWaypointCurrentAngle = yourAngle;
                    }
                    else
                    {
                        if (newWaypointCurrentAngle < yourAngle)
                        {
                            newWaypoint = wayPoints[i].transform;
                            newWaypointCurrentAngle = yourAngle;
                        }
                    }
                }
                _currentWaypoint = newWaypoint;
            }
        }
        Speed = Mathf.RoundToInt(_rb.velocity.magnitude * 3.6f);
        ////////////
        if(_nextWaypoint != null)
        {
            Vector3 localPos = transform.InverseTransformPoint(_nextWaypoint.transform.position);
            if(localPos.x < -2)
            {
                _turnDirection = TurnDirection.Left;
            }
            else if(localPos.x >2)
            {
                _turnDirection = TurnDirection.Right;
            }
            else
            {
                _turnDirection = TurnDirection.Center;
            }

        }
    }
    

    private void FixedUpdate()
    {
        try
        {
            MoveCar();
            SteerVehicle();
            AntiRoll();
            ChangeWaypoint();
            TractionControl();
        }
        catch
        {

        }
    }
    private void OnDrawGizmos()
    {
        //Draw cone FOV
        Gizmos.DrawWireSphere(transform.position, _maxDistance);
       // Gizmos.DrawLine(_checkLeftAndFront.position, Quaternion.AngleAxis(140 / 2, transform.up)*(_checkLeftAndFront.forward * _maxDistance));
        //Gizmos.DrawLine(_checkLeftAndFront.position, Quaternion.AngleAxis(-140 / 2, transform.up) * (_checkLeftAndFront.forward * _maxDistance));
    }
    private void ChangeWaypoint()
    {
        DistanceToWaypoint= Vector3.Distance(transform.position, _currentWaypoint.position);
        //Debug.Log(Vector3.Distance(transform.position, _currentWaypoint.position));
        if(DistanceToWaypoint <=3 && _currentWaypoint.GetComponent<WayPoints>().HasATurn == false)
        {
            
                if(_currentWaypoint.GetComponent<WayPoints>().NextWaypoint.Length == 1)
                {
                    _currentWaypoint = _currentWaypoint.GetComponent<WayPoints>().NextWaypoint[0];
                    _nextWaypoint = null;
                    CheckPath();
              
                }
               
                    
                 
     
        }
        else if (DistanceToWaypoint <= 25&&DistanceToWaypoint>5 && _currentWaypoint.GetComponent<WayPoints>().HasATurn)
        {
            if (_turnDirection == TurnDirection.Center && _currentWaypoint.GetComponent<WayPoints>().Stop ==false)
            {
                _canMove = true;
            }
            else
            {
                if (Speed>20)
                {
                _canMove = false;
                }
                else
                {
                _canMove = true;
                }

            }
        }
        else if(DistanceToWaypoint <= 5 && _currentWaypoint.GetComponent<WayPoints>().HasATurn)
        {
            if (_turnDirection == TurnDirection.Center)
            {
                if (_currentWaypoint.GetComponent<WayPoints>().Stop)
                {
                    _numberOfcarsPassing = 0;
                    _carsStopedInFront = new List<GameObject>();
                    List<Collider> hitColliders2 = new List<Collider>();
                    hitColliders2 = Physics.OverlapSphere(_checkFront.position, 20, _aiCarLayer).ToList();
                    List<Collider> hitColliders = new List<Collider>();
                    hitColliders = Physics.OverlapSphere(_checkFront.position, _maxDistance, _aiCarLayer + _playerCarLayer).ToList();
                    
                    for (var i = 0; i < hitColliders.Count; i++)
                    {
                        
                        Transform tempTarget = hitColliders[i].transform;
                       
                        Vector3 dir = tempTarget.position - _checkLeft.position; // find target direction
                        Vector3 dir2 = tempTarget.position - _checkRight.position; // find target direction
                        Vector3 myDir = _checkLeft.forward;
                        Vector3 myDir2 = _checkRight.forward;
                        Vector3 yourDir = tempTarget.forward;
                        float myAngle = Vector3.Angle(myDir, dir);
                        float yourAngle = Vector3.Angle(yourDir, -dir);
                        float yourAngle2 = Vector3.Angle(yourDir, -dir2);
                       // Debug.Log(tempTarget.parent.name + " " + Vector3.Angle(dir2, _checkRight.right));
                      //  Debug.Log(tempTarget.parent.name + " " + yourAngle+ " and "+ yourAngle2 );

                        if (Vector3.Angle(dir, _checkLeft.right) <= 100 / 2 || Vector3.Angle(dir, -_checkLeft.right) <= 100 / 2)
                        {
                            if (yourAngle < 90 )
                            {
                                Debug.DrawRay(_checkLeft.position, tempTarget.position - _checkLeft.position, Color.magenta);

                                _numberOfcarsPassing++;
                                _canMove = false;
                                _side = true;
                            }

                        }
                       
                    }
                    for (var i = 0; i < hitColliders2.Count; i++)
                    {
                        Transform tempTarget = hitColliders2[i].transform;
                        Vector3 dir3 = tempTarget.position - _checkFront.position; // find target direction
                        Vector3 myDir3 = _checkFront.forward;
                        Vector3 yourDir = tempTarget.forward;
                        float yourAngle3 = Vector3.Angle(yourDir, -dir3);
                        if (Vector3.Angle(dir3, _checkFront.forward) <= 70 / 2)
                        {
                            if (yourAngle3 < 90)
                            {
                                _carsStopedInFront.Add(tempTarget.transform.parent.gameObject);
                                
                                Debug.DrawRay(_checkLeft.position, tempTarget.position - _checkLeft.position, Color.magenta);
                                _canMove = false;
                                _front = true;
                            }
                        }
                    }
                    if(_carsStopedInFront.Count ==0)
                    {
                        _front = false;
                    }
                    if (_numberOfcarsPassing == 0)
                    {
                        if (_front == false)
                        {
                            _currentWaypoint = _nextWaypoint;
                            CheckPath();
                            _canMove = true;
                            _nextWaypoint = null;
                        }
                        else
                        {
                            if(_carsStopedInFront.Count == 0)
                            {
                                _currentWaypoint = _nextWaypoint;
                                CheckPath();
                                _canMove = true;
                                _nextWaypoint = null;
                                _front = false;
                            }
                            else
                            {
                                GameObject tempCar;
                                if(_carsStopedInFront.Count ==1)
                                {
                                    if (_carsStopedInFront[0].GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Center)
                                    {
                                        _currentWaypoint = _nextWaypoint;
                                        CheckPath();
                                        _canMove = true;
                                        _nextWaypoint = null;
                                    }
                                    if (_carsStopedInFront[0].GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Left)
                                    {
                                        _currentWaypoint = _nextWaypoint;
                                        CheckPath();
                                        _canMove = true;
                                        _nextWaypoint = null;
                                    }
                                    if (_carsStopedInFront[0].GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Right)
                                    {
                                        _currentWaypoint = _nextWaypoint;
                                        CheckPath();
                                        _canMove = true;
                                        _nextWaypoint = null;
                                    }

                                }
                                else
                                {

                                    _currentWaypoint = _nextWaypoint;
                                    CheckPath();
                                    _canMove = true;
                                    _nextWaypoint = null;
                                }

                            }
                        }
                    }                
                }
                else
                {
                    Debug.Log("3");
                    _currentWaypoint = _nextWaypoint;
                    
                    CheckPath();
                    _canMove = true;
                    _nextWaypoint = null;
                }
            }
            else if (_turnDirection == TurnDirection.Left)
                {
                if (_currentWaypoint.GetComponent<WayPoints>().Stop)
                {
                    if (_nextWaypoint != null)
                    {
                        _numberOfcarsPassing = 0;

                        _carsStopedInFront = new List<GameObject>();

                        List<Collider> hitColliders = new List<Collider>();
                        List<Collider> hitColliders2 = new List<Collider>();
                        hitColliders = Physics.OverlapSphere(_checkFront.position, _maxDistance, _aiCarLayer + _playerCarLayer).ToList();
                        hitColliders2 = Physics.OverlapSphere(_checkFront.position, 27, _aiCarLayer).ToList();
                        for (var i = 0; i < hitColliders.Count; i++)
                        {

                            Transform tempTarget = hitColliders[i].transform;

                            Vector3 dir = tempTarget.position - _checkLeft.position; // find target direction
                            Vector3 dir2 = tempTarget.position - _checkRight.position; // find target direction
                            Vector3 myDir = _checkLeft.forward;
                            Vector3 myDir2 = _checkRight.forward;
                            Vector3 yourDir = tempTarget.forward;
                            float myAngle = Vector3.Angle(myDir, dir);
                            float yourAngle = Vector3.Angle(yourDir, -dir);
                            float yourAngle2 = Vector3.Angle(yourDir, -dir2);
                            //Debug.Log(tempTarget.parent.name + " " + Vector3.Angle(dir2, _checkRight.right));
                           // Debug.Log(tempTarget.parent.name + " " + yourAngle + " and " + yourAngle2);

                            if (Vector3.Angle(dir, _checkLeft.right) <= 100 / 2 || Vector3.Angle(dir, -_checkLeft.right) <= 100 / 2)
                            {
                                if (yourAngle < 90)
                                {
                                    Debug.DrawRay(_checkLeft.position, tempTarget.position - _checkLeft.position, Color.magenta);

                                    _numberOfcarsPassing++;
                                    _canMove = false;
                                    _side = true;
                                }
                            }
                        }
                        for (var i = 0; i < hitColliders2.Count; i++)
                        {
                            Transform tempTarget = hitColliders2[i].transform;
                            Vector3 dir3 = tempTarget.position - _checkFront.position; // find target direction
                            Vector3 myDir3 = _checkFront.forward;
                            Vector3 yourDir = tempTarget.forward;
                            float yourAngle3 = Vector3.Angle(yourDir, -dir3);
                            if (Vector3.Angle(dir3, _checkFront.forward) <= 70 / 2)
                            {
                                if (yourAngle3 < 90)
                                {
                                    _carsStopedInFront.Add(tempTarget.transform.parent.gameObject);

                                    Debug.DrawRay(_checkLeft.position, tempTarget.position - _checkLeft.position, Color.magenta);
                                    //_carsStopedInFront = tempTarget.parent.gameObject;
                                    _canMove = false;
                                    _front = true;
                                }
                            }
                        }
                        if (_carsStopedInFront.Count == 0)
                        {
                            _front = false;
                        }
                        if (_numberOfcarsPassing == 0)
                        {
                            if (_front == false)
                            {
                                _currentWaypoint = _nextWaypoint;
                                CheckPath();
                                _canMove = true;
                                _nextWaypoint = null;
                            }
                            else
                            {
                                if (_carsStopedInFront.Count == 0)
                                {
                                    _currentWaypoint = _nextWaypoint;
                                    CheckPath();
                                    _canMove = true;
                                    _nextWaypoint = null;
                                    _front = false;
                                }
                                else
                                {
                                    GameObject tempCar;
                                    if (_carsStopedInFront.Count == 1)
                                    {
                                        if (_carsStopedInFront[0].GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Center)
                                        {
                                            _canMove = false;
                                        }
                                        if (_carsStopedInFront[0].GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Left)
                                        {
                                            if (DistanceToWaypoint > _carsStopedInFront[0].GetComponent<AICARCONTROLLE2>().DistanceToWaypoint)
                                            {
                                                _currentWaypoint = _nextWaypoint;
                                                CheckPath();
                                                _canMove = true;
                                                _nextWaypoint = null;
                                            }
                                            else
                                            {
                                                _canMove = false;

                                            }
                                        }
                                        if (_carsStopedInFront[0].GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Right)
                                        {
                                            _canMove = false;

                                        }
                                    }
                                    else
                                    {
                                        tempCar = _carsStopedInFront[0].gameObject;
                                        for (int i = 0; i < _carsStopedInFront.Count; i++)
                                        {
                                            
                                            if (Vector3.Distance(gameObject.transform.position, _carsStopedInFront[i].transform.position) <= Vector3.Distance(gameObject.transform.position, tempCar.transform.position))
                                            {
                                                tempCar = _carsStopedInFront[i];
                                            }
                                        }
                                        //Debug.Log(tempCar.name);

                                        if (tempCar.GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Center)
                                        {
                                            _canMove = false;
                                        }
                                        if (tempCar.GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Left)
                                        {
                                            if (DistanceToWaypoint > tempCar.GetComponent<AICARCONTROLLE2>().DistanceToWaypoint)
                                            {
                                                _currentWaypoint = _nextWaypoint;
                                                CheckPath();
                                                _canMove = true;
                                                _nextWaypoint = null;
                                            }
                                            else
                                            {
                                                _canMove = false;

                                            }
                                        }
                                        if (tempCar.GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Right)
                                        {
                                            _canMove = false;

                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                else
                {
                    if (_nextWaypoint != null)
                    {
                        _numberOfcarsPassing = 0;

                        _carsStopedInFront = new List<GameObject>();

                        List<Collider> hitColliders = new List<Collider>();
                        List<Collider> hitColliders2 = new List<Collider>();
                        hitColliders2 = Physics.OverlapSphere(_checkLeft.position, 20, _aiCarLayer).ToList();
                        for (var i = 0; i < hitColliders2.Count; i++)
                        {
                            Transform tempTarget = hitColliders2[i].transform;
                            Vector3 dir3 = tempTarget.position - _checkLeft.position; // find target direction
                            Vector3 myDir3 = _checkLeft.forward;
                            Vector3 yourDir = tempTarget.forward;
                            float yourAngle3 = Vector3.Angle(yourDir, -dir3);
                            if (Vector3.Angle(dir3, _checkLeft.forward) <= 70 / 2)
                            {
                                if (yourAngle3 < 90)
                                {
                                    _carsStopedInFront.Add(tempTarget.transform.parent.gameObject);

                                    Debug.DrawRay(_checkLeft.position, tempTarget.position - _checkLeft.position, Color.cyan);
                                  //  _carsStopedInFront = tempTarget.parent.gameObject;
                                    _canMove = false;
                                    _front = false;
                                }

                            }
                        }

                        if (_carsStopedInFront.Count == 0)
                        {
                            _currentWaypoint = _nextWaypoint;
                            CheckPath();
                            _canMove = true;
                            _nextWaypoint = null;
                            _front = false;
                        }
                        else
                        {
                            GameObject tempCar;
                            if (_carsStopedInFront.Count == 1)
                            {
                                if (_carsStopedInFront[0].GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Center)
                                {
                                    _canMove = false;
                                }
                                if (_carsStopedInFront[0].GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Left)
                                {
                                    if (DistanceToWaypoint > _carsStopedInFront[0].GetComponent<AICARCONTROLLE2>().DistanceToWaypoint)
                                    {
                                        _currentWaypoint = _nextWaypoint;
                                        CheckPath();
                                        _canMove = true;
                                        _nextWaypoint = null;
                                    }
                                    else
                                    {
                                        _canMove = false;
                                    }
                                }
                                if (_carsStopedInFront[0].GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Right)
                                {
                                    _canMove = false;
                                }
                            }
                            else
                            {

                            }
                        }
                    }
                }
            }
            else if (_turnDirection == TurnDirection.Right)
            {
                if (_currentWaypoint.GetComponent<WayPoints>().Stop)
                {
                    _numberOfcarsPassing = 0;
                    _carsStopedInFront = new List<GameObject>();

                    List<Collider> hitColliders = new List<Collider>();
                    List<Collider> hitColliders2 = new List<Collider>();
                    hitColliders = Physics.OverlapSphere(_checkFront.position, _maxDistance, _aiCarLayer + _playerCarLayer).ToList();
                    hitColliders2 = Physics.OverlapSphere(_checkFront.position, 20, _aiCarLayer).ToList();

                    for (var i = 0; i < hitColliders.Count; i++)
                    {
                        Transform tempTarget = hitColliders[i].transform;
                        Vector3 dir = tempTarget.position - _checkLeft.position; // find target direction
                        Vector3 dir2 = tempTarget.position - _checkRight.position; // find target direction
                        Vector3 myDir = _checkLeft.forward;
                        Vector3 myDir2 = _checkRight.forward;
                        Vector3 yourDir = tempTarget.forward;
                        float myAngle = Vector3.Angle(myDir, dir);
                        float myAngle2 = Vector3.Angle(myDir2, dir2);
                        float yourAngle = Vector3.Angle(yourDir, -dir);
                        float yourAngle2 = Vector3.Angle(yourDir, -dir2);
                        if (Vector3.Angle(dir2, _checkRight.right) <= 100 / 2 || Vector3.Angle(dir, -_checkLeft.right) <= 100 / 2)
                        {
                            if (yourAngle < 90 || yourAngle2 < 90)
                            {
                                Debug.DrawRay(_checkLeft.position, tempTarget.position - _checkLeft.position, Color.yellow);
                                _numberOfcarsPassing++;
                                _canMove = false;
                            }
                        }
                    }
                    for (var i = 0; i < hitColliders2.Count; i++)
                    {
                        Transform tempTarget = hitColliders2[i].transform;
                        Vector3 dir3 = tempTarget.position - _checkFront.position; // find target direction
                        Vector3 myDir3 = _checkFront.forward;
                        Vector3 yourDir = tempTarget.forward;
                        float yourAngle3 = Vector3.Angle(yourDir, -dir3);
                        if (Vector3.Angle(dir3, _checkFront.forward) <= 70 / 2)
                        {
                            if (yourAngle3 < 90)
                            {
                                _carsStopedInFront.Add(tempTarget.transform.parent.gameObject);

                                Debug.DrawRay(_checkLeft.position, tempTarget.position - _checkLeft.position, Color.magenta);
                                //_carsStopedInFront = tempTarget.parent.gameObject;
                                _canMove = false;
                                _front = true;
                            }
                        }
                    }
                    if (_carsStopedInFront.Count == 0)
                    {
                        _front = false;
                    }
                    if (_numberOfcarsPassing == 0)
                    {
                        if (_front == false)
                        {
                            Debug.Log("1");
                            _currentWaypoint = _nextWaypoint;
                            CheckPath();
                            _canMove = true;
                            _nextWaypoint = null;
                        }
                        else
                        {
                            if (_carsStopedInFront.Count == 0)
                            {
                                Debug.Log("2");
                                _currentWaypoint = _nextWaypoint;
                                CheckPath();
                                _canMove = true;
                                _nextWaypoint = null;
                                _front = false;
                            }
                            else
                            {
                                GameObject tempCar;
                                if (_carsStopedInFront.Count == 1)
                                {
                                    if (_carsStopedInFront[0].GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Center)
                                    {
                                        Debug.Log("3");
                                        _currentWaypoint = _nextWaypoint;
                                        CheckPath();
                                        _canMove = true;
                                        _nextWaypoint = null;
                                    }
                                    if (_carsStopedInFront[0].GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Left)
                                    {
                                        Debug.Log("4");
                                        _currentWaypoint = _nextWaypoint;
                                        CheckPath();
                                        _canMove = true;
                                        _nextWaypoint = null;
                                    }
                                    if (_carsStopedInFront[0].GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Right)
                                    {
                                        Debug.Log("5");
                                        _currentWaypoint = _nextWaypoint;
                                        CheckPath();
                                        _canMove = true;
                                        _nextWaypoint = null;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("1");
                    _currentWaypoint = _nextWaypoint;
                    CheckPath();
                    _canMove = true;
                    _nextWaypoint = null;
                }
            }
            
        }
    }

    private void CheckPath()
    {
        if(_currentWaypoint.GetComponent<WayPoints>().NextWaypoint.Length > 1)
        {
            //Debug.Log(_currentWaypoint.GetComponent<WayPoints>().NextWaypoint.Length);
            int i = Random.Range(0, _currentWaypoint.GetComponent<WayPoints>().NextWaypoint.Length);
            _nextWaypoint = _currentWaypoint.GetComponent<WayPoints>().NextWaypoint[i]; 
        }
    }
    private void MoveCar()
    {
 
        foreach (var item in wheels)
        {
            RaycastHit objectHit;
            Debug.DrawRay(new Vector3(_checkFront.position.x, _checkFront.position.y, _checkFront.position.z), transform.forward * 7, Color.green);
            


           
            if (Physics.Raycast(new Vector3(_checkFront.position.x, _checkFront.position.y, _checkFront.position.z), transform.forward, out objectHit, 7, _aiCarLayer + _playerCarLayer))
            {
                //do something if hit object ie
                Vector3 dir = objectHit.transform.position - _checkLeft.position; // find target direction
                Vector3 myDir = transform.forward;
                Vector3 yourDir = objectHit.transform.forward;

                float myAngle = Vector3.Angle(myDir, dir);
                float yourAngle = Vector3.Angle(yourDir, -dir);
                if (Vector3.Angle(dir, _checkLeft.forward) <= 100 / 2)
                {
                    // Debug.Log(myAngle + " " + yourAngle);
                    if (yourAngle > 90)
                    {         
                            _carInFront = true;                                        
                    }
               }
               
            }
            else
            {
                _carInFront = false;
            }
            if (_canMove)
            {
                if(_carInFront ==false)
                {
                    if (Speed< _currentWaypoint.GetComponent<WayPoints>().RoadMaxSpeed)
                    {
                        item.motorTorque = _totalPower;
                        item.brakeTorque = 0;

                    }
                    else if(Speed > _currentWaypoint.GetComponent<WayPoints>().RoadMaxSpeed)
                    {
                        item.motorTorque = 0;
                        item.brakeTorque = 400;
                    }
                    else
                    {
                        item.motorTorque = 0;
                        item.brakeTorque = 0;
                    }
                }
                else
                {
                    if(objectHit.collider.tag == "AICar")
                    {
                        if (Speed < objectHit.transform.GetComponent<AICARCONTROLLE2>().Speed)
                        {
                            item.motorTorque = _totalPower;
                            item.brakeTorque = 0;

                        }
                        else if (Speed > objectHit.transform.GetComponent<AICARCONTROLLE2>().Speed)
                        {
                            item.motorTorque = 0;
                            item.brakeTorque = 400;
                        }
                        else
                        {
                            item.motorTorque = 0;
                            item.brakeTorque = 0;
                        }
                    }
                    else
                    {
                        if (Speed < objectHit.transform.GetComponent<CarControllerScript>().CurrentSpeed)
                        {
                            item.motorTorque = _totalPower;
                            item.brakeTorque = 0;

                        }
                        else if (Speed > objectHit.transform.GetComponent<CarControllerScript>().CurrentSpeed)
                        {
                            item.motorTorque = 0;
                            item.brakeTorque = 400;
                        }
                        else
                        {
                            item.motorTorque = 0;
                            item.brakeTorque = 0;
                        }
                    }
                }
              
            }   
            else
            {
                item.motorTorque = 0;
                item.brakeTorque = 600;
            }
        }
    }
    private void SteerVehicle()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(_currentWaypoint.transform.position);
        relativeVector /= relativeVector.magnitude;
        float newSteer = (relativeVector.x / relativeVector.magnitude) * 2;
        _horizontal = newSteer;


        if (_horizontal > 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (_radius + (1.5f / 2))) * _horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (_radius - (1.5f / 2))) * _horizontal;
        }
        else if (_horizontal < 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (_radius - (1.5f / 2))) * _horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (_radius + (1.5f / 2))) * _horizontal;
        }
        else
        {
            wheels[0].steerAngle = 0;
            wheels[1].steerAngle = 0;
        }
    }
    private void AntiRoll()
    {
        // Front axle
        ApplyAntiRoll(wheels[0], wheels[1]);
        // Back axle
        ApplyAntiRoll(wheels[2], wheels[3]);
    }
    private void ApplyAntiRoll(WheelCollider left, WheelCollider right)
    {
        WheelHit hit;
        float travelLeft = 1f;
        float travelRight = 1f;

        bool isGroundedLeft = left.GetGroundHit(out hit);
        if (isGroundedLeft)
        {
            travelLeft = (-left.transform.InverseTransformPoint(hit.point).y - left.radius) / left.suspensionDistance;
        }
        bool isGroundedRight = right.GetGroundHit(out hit);
        if (isGroundedRight)
        {
            travelRight = (-right.transform.InverseTransformPoint(hit.point).y - right.radius) / right.suspensionDistance;
        }

        float antirollForce = (travelLeft - travelRight) * _antiRoll;

        if (isGroundedLeft)
        {
            _rb.AddForceAtPosition(left.transform.up * -antirollForce, left.transform.position);
        }
        if (isGroundedRight)
        {
            _rb.AddForceAtPosition(right.transform.up * antirollForce, right.transform.position);
        }
    }
    private void TractionControl()
    {
        if (_tractionControl)
        {
            WheelHit hit;
            wheels[0].GetGroundHit(out hit);
            if (hit.forwardSlip >= 0.3f && wheels[0].motorTorque > 0)
            {
                wheels[0].motorTorque *= 0.9f;
            }
            wheels[1].GetGroundHit(out hit);
            if (hit.forwardSlip >= 0.3f && wheels[1].motorTorque > 0)
            {
                wheels[1].motorTorque *= 0.9f;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "WayPoints")
        //{
        //    if (other.GetComponent<WayPoints>().NextWaypoint.Length == 1)
        //    {
        //        _currentWaypoint = other.GetComponent<WayPoints>().NextWaypoint[0];
            

        //    }
           

        //}

    }
}
