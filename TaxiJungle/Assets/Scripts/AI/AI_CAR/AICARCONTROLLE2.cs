using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

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
    [Range(0, 360)]
    [SerializeField] private float _angle;
    [SerializeField] private LayerMask _waypointsLayer;
    [SerializeField] private LayerMask _aiCarLayer;
    [SerializeField] private LayerMask _playerCarLayer;
    [SerializeField] private LayerMask _npcLayer;
    [SerializeField] private TurnDirection _turnDirection;

    [SerializeField] private WheelCollider[] wheels;
    [SerializeField] private float _totalPower;
    [SerializeField] private float _vertical, _horizontal;
    private bool _tractionControl = true;
    [SerializeField]private bool _canMove = true;

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
    [SerializeField] private bool _nPCCrossing;
    [SerializeField] float timer;

    [SerializeField] private Transform _massCenter;
    private Rigidbody _rigidBody;


   [SerializeField] List<GameObject> _tempcarsinfront;
    public int Speed { get => _speed; set => _speed = value; }
    public float DistanceToWaypoint { get => _distanceToWaypoint; set => _distanceToWaypoint = value; }
    public Transform CurrentWaypoint { get => _currentWaypoint; set => _currentWaypoint = value; }
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rigidBody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        _rigidBody.centerOfMass = _massCenter.localPosition;
    }
    private void Update()
    {
        if (CurrentWaypoint == null)
        {
            timer += Time.deltaTime;
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
                    Vector3 dir = tempTarget.position - _checkFront.position; // find target direction
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
                CurrentWaypoint = newWaypoint;
            }
        }
        Speed = Mathf.RoundToInt(_rb.velocity.magnitude * 3.6f);
        if (_nextWaypoint != null)
        {
            Vector3 localPos = transform.InverseTransformPoint(_nextWaypoint.transform.position);
            if (localPos.x < -2)
            {
                _turnDirection = TurnDirection.Left;
            }
            else if (localPos.x > 2)
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
        Gizmos.DrawWireSphere(transform.position, _maxDistance);
    }
    private void ChangeWaypoint()
    {
        DistanceToWaypoint = Vector3.Distance(transform.position, CurrentWaypoint.position);
        if (DistanceToWaypoint <= 5 && CurrentWaypoint.GetComponent<WayPoints>().HasATurn == false && CurrentWaypoint.GetComponent<WayPoints>().HasATraficLight == false)
        {
            if (CurrentWaypoint.GetComponent<WayPoints>().NextWaypoint.Length == 1)
            {
                CurrentWaypoint = CurrentWaypoint.GetComponent<WayPoints>().NextWaypoint[0];
                _nextWaypoint = null;
                CheckPath();
            }
        }
        else if (DistanceToWaypoint <= 25 && DistanceToWaypoint > 5 && CurrentWaypoint.GetComponent<WayPoints>().HasATurn || DistanceToWaypoint <= 25 && DistanceToWaypoint > 5 && CurrentWaypoint.GetComponent<WayPoints>().HasATraficLight)
        {
            if (CurrentWaypoint.GetComponent<WayPoints>().HasATraficLight)
            {
                if (CurrentWaypoint.GetComponent<WayPoints>().TraficLight.CarCanGo)
                {
                    _canMove = true;

                }
                else
                {
                    if (Speed > 20)
                    {
                        _canMove = false;
                    }
                    else
                    {
                        _canMove = true;
                    }
                }
            }
            else
            {
                if (_turnDirection == TurnDirection.Center && CurrentWaypoint.GetComponent<WayPoints>().Stop == false && CurrentWaypoint.GetComponent<WayPoints>().HasATraficLight == false)
                {
                    _canMove = true;
                }
                else
                {
                    if (Speed > 20)
                    {
                        _canMove = false;
                    }
                    else
                    {
                        _canMove = true;
                    }
                }
            }
        }
        else if (DistanceToWaypoint <= 5 && CurrentWaypoint.GetComponent<WayPoints>().HasATurn || DistanceToWaypoint <= 5 && CurrentWaypoint.GetComponent<WayPoints>().HasATraficLight)
        {
            if (CurrentWaypoint.GetComponent<WayPoints>().HasATraficLight)
            {
                if (CurrentWaypoint.GetComponent<WayPoints>().TraficLight.CarCanGo)
                {
                    _canMove = true;
                    CheckPath();
                    CurrentWaypoint = CurrentWaypoint.GetComponent<WayPoints>().NextWaypoint[0];
                }
                else
                {
                    _canMove = false;
                }
            }
            else
            {
                if (_turnDirection == TurnDirection.Center)
                {
                    if (CurrentWaypoint.GetComponent<WayPoints>().Stop)
                    {
                        _numberOfcarsPassing = 0;
                        _carsStopedInFront = new List<GameObject>();
                        List<Collider> hitColliders2 = new List<Collider>();
                        hitColliders2 = Physics.OverlapSphere(_checkFront.position, _maxDistance + 10, _aiCarLayer).ToList();
                        List<Collider> hitColliders = new List<Collider>();
                        hitColliders = Physics.OverlapSphere(_checkFront.position, _maxDistance, _aiCarLayer + _playerCarLayer).ToList();
                        for (var i = 0; i < hitColliders.Count; i++)
                        {
                            Transform tempTarget = hitColliders[i].transform;
                            Vector3 dir = tempTarget.position - _checkRight.position; // find target direction
                            Vector3 dir2 = tempTarget.position - _checkLeft.position; // find target direction
                            Vector3 myDir = _checkFront.forward;
                            Vector3 myDir2 = _checkFront.forward;
                            Vector3 yourDir = tempTarget.forward;
                            float myAngle = Vector3.Angle(myDir, dir);
                            float yourAngle = Vector3.Angle(yourDir, -dir);
                            float yourAngle2 = Vector3.Angle(yourDir, -dir2);
                            if (Vector3.Angle(dir, _checkLeft.right) <= 110 / 2 || Vector3.Angle(dir, -_checkRight.right) <= 110 / 2)
                            {
                                if (yourAngle < 110)
                                {
                                    if (tempTarget.GetComponent<AICARCONTROLLE2>().CurrentWaypoint.GetComponent<WayPoints>().Stop == false)
                                    {
                                        Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, Color.blue);

                                        if (tempTarget.gameObject != this.gameObject)
                                        {
                                            _numberOfcarsPassing++;
                                            _canMove = false;
                                            _side = true;
                                        }
                                    }         
                                }
                                else
                                {
                                    Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, Color.red);

                                }
                            }
                            else
                            {
                                Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, Color.green);

                            }

                        }
                        for (var i = 0; i < hitColliders2.Count; i++)
                        {
                            Transform tempTarget = hitColliders2[i].transform;
                            Vector3 dir3 = tempTarget.position - _checkFront.position; // find target direction
                            Vector3 myDir3 = _checkFront.forward;
                            Vector3 yourDir = tempTarget.forward;
                            float yourAngle3 = Vector3.Angle(yourDir, -dir3);
                            if (Vector3.Angle(dir3, _checkFront.forward) <= 110 / 2)
                            {
                                if (yourAngle3 < 90)
                                {
                                  
                                        _carsStopedInFront.Add(tempTarget.transform.gameObject);
                                        _carsStopedInFront = _carsStopedInFront.OrderBy(x => (this.transform.position - x.transform.position).sqrMagnitude).ToList();
                                        Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, Color.white);
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
                                CurrentWaypoint = _nextWaypoint;
                                CheckPath();
                                _canMove = true;
                                _nextWaypoint = null;
                            }
                            else
                            {
                                if (_carsStopedInFront.Count == 0)
                                {
                                    CurrentWaypoint = _nextWaypoint;
                                    CheckPath();
                                    _canMove = true;
                                    _nextWaypoint = null;
                                    _front = false;
                                }
                                else
                                {
                                    if (_carsStopedInFront.Count == 1)
                                    {
                                        if (_carsStopedInFront[0].GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Center)
                                        {
                                            CurrentWaypoint = _nextWaypoint;
                                            CheckPath();
                                            _canMove = true;
                                            _nextWaypoint = null;
                                        }
                                        if (_carsStopedInFront[0].GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Left)
                                        {
                                            CurrentWaypoint = _nextWaypoint;
                                            CheckPath();
                                            _canMove = true;
                                            _nextWaypoint = null;


                                        }
                                        if (_carsStopedInFront[0].GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Right)
                                        {
                                            CurrentWaypoint = _nextWaypoint;
                                            CheckPath();
                                            _canMove = true;
                                            _nextWaypoint = null;
                                        }
                                    }
                                    else
                                    {
                                        CurrentWaypoint = _nextWaypoint;
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
                        CurrentWaypoint = _nextWaypoint;
                        CheckPath();
                        _canMove = true;
                        _nextWaypoint = null;
                    }
                }
                else if (_turnDirection == TurnDirection.Left)
                {
                    if (CurrentWaypoint.GetComponent<WayPoints>().Stop)
                    {
                        if (_nextWaypoint != null)
                        {
                            _numberOfcarsPassing = 0;
                            _carsStopedInFront = new List<GameObject>();
                            List<Collider> hitColliders = new List<Collider>();
                            List<Collider> hitColliders2 = new List<Collider>();
                            hitColliders = Physics.OverlapSphere(_checkFront.position, _maxDistance, _aiCarLayer ).ToList();
                            hitColliders2 = Physics.OverlapSphere(_checkFront.position, _maxDistance + 10, _aiCarLayer).ToList();
                            for (var i = 0; i < hitColliders.Count; i++)
                            {
                                Transform tempTarget = hitColliders[i].transform;
                                Vector3 dir = tempTarget.position - _checkRight.position; // find target direction
                                Vector3 dir2 = tempTarget.position - _checkLeft.position; // find target direction
                                Vector3 myDir = _checkFront.forward;
                                Vector3 myDir2 = _checkFront.forward;
                                Vector3 yourDir = tempTarget.forward;
                                float myAngle = Vector3.Angle(myDir, dir);
                                float yourAngle = Vector3.Angle(yourDir, -dir);
                                float yourAngle2 = Vector3.Angle(yourDir, -dir2);
                                if (Vector3.Angle(dir, _checkLeft.right) <= 110 / 2 || Vector3.Angle(dir, -_checkRight.right) <= 110 / 2)
                                {
                                    if (yourAngle < 110)
                                    {
                                        if(tempTarget.GetComponent<AICARCONTROLLE2>().CurrentWaypoint.GetComponent<WayPoints>().Stop ==false)
                                        {
                                            Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, Color.magenta);

                                            if (tempTarget.gameObject != this.gameObject)
                                            {
                                                _numberOfcarsPassing++;
                                                _canMove = false;
                                                _side = true;
                                            }
                                        }
                                        
                                    }
                                    else
                                    {
                                        Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, Color.red);

                                    }
                                }
                                else
                                {
                                    Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, Color.green);

                                }
                            }
                            for (var i = 0; i < hitColliders2.Count; i++)
                            {
                                Transform tempTarget = hitColliders2[i].transform;
                                Vector3 dir3 = tempTarget.position - _checkFront.position; // find target direction
                                Vector3 myDir3 = _checkFront.forward;
                                Vector3 yourDir = tempTarget.forward;
                                float yourAngle3 = Vector3.Angle(yourDir, -dir3);
                                if (Vector3.Angle(dir3, _checkFront.forward) <= 110 / 2)
                                {
                                    if (yourAngle3 < 90)
                                    {
                                      
                                            _carsStopedInFront.Add(tempTarget.transform.gameObject);
                                            _carsStopedInFront = _carsStopedInFront.OrderBy(x => (this.transform.position - x.transform.position).sqrMagnitude).ToList();
                                            Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, Color.white);
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
                                    CurrentWaypoint = _nextWaypoint;
                                    CheckPath();
                                    _canMove = true;
                                    _nextWaypoint = null;
                                }
                                else
                                {
                                    if (_carsStopedInFront.Count == 0)
                                    {
                                        CurrentWaypoint = _nextWaypoint;
                                        CheckPath();
                                        _canMove = true;
                                        _nextWaypoint = null;
                                        _front = false;
                                    }
                                    else
                                    {
                                        GameObject tempCar;
                                        if (_carsStopedInFront.Count > 0)
                                        {
                                            if (_carsStopedInFront[0].GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Center)
                                            {
                                                _canMove = false;
                                            }
                                            if (_carsStopedInFront[0].GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Left)
                                            {
                                                if (DistanceToWaypoint > _carsStopedInFront[0].GetComponent<AICARCONTROLLE2>().DistanceToWaypoint)
                                                {
                                                    CurrentWaypoint = _nextWaypoint;
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
                                            if (tempCar.GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Center)
                                            {
                                                _canMove = false;
                                            }
                                            if (tempCar.GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Left)
                                            {
                                                if (DistanceToWaypoint > tempCar.GetComponent<AICARCONTROLLE2>().DistanceToWaypoint)
                                                {
                                                    CurrentWaypoint = _nextWaypoint;
                                                    CheckPath();
                                                    _canMove = true;
                                                    _nextWaypoint = null;
                                                }
                                                else
                                                {
                                                    _canMove = false;
                                                }
                                                _canMove = false;
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
                            List<Collider> hitColliders2 = new List<Collider>();
                            hitColliders2 = Physics.OverlapSphere(_checkFront.position, _maxDistance + 10, _aiCarLayer).ToList();
                            for (var i = 0; i < hitColliders2.Count; i++)
                            {
                                Transform tempTarget = hitColliders2[i].transform;
                                Vector3 dir3 = tempTarget.position - _checkFront.position; // find target direction
                                Vector3 myDir3 = _checkFront.forward;
                                Vector3 yourDir = tempTarget.forward;
                                float yourAngle3 = Vector3.Angle(yourDir, -dir3);
                                if (Vector3.Angle(dir3, _checkFront.forward) <= 110 / 2)
                                {
                                    if (yourAngle3 < 90)
                                    {
                                        if (tempTarget.GetComponent<AICARCONTROLLE2>().CurrentWaypoint.GetComponent<WayPoints>().Stop == false)
                                        {
                                            _carsStopedInFront.Add(tempTarget.transform.gameObject);
                                            _carsStopedInFront = _carsStopedInFront.OrderBy(x => (this.transform.position - x.transform.position).sqrMagnitude).ToList();

                                            Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, Color.white);
                                            
                                        }
                                        
                                    }
                                }
                            }
                            if (_carsStopedInFront.Count == 0)
                            {
                                CurrentWaypoint = _nextWaypoint;
                                CheckPath();
                                _canMove = true;
                                _nextWaypoint = null;
                                _front = false;
                            }
                            else
                            {
                                if (_carsStopedInFront.Count >0)
                                {
                                    if (_carsStopedInFront[0].GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Center)
                                    {
                                        _canMove = false;
                                    }
                                    if (_carsStopedInFront[0].GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Left)
                                    {
                                        if (DistanceToWaypoint > _carsStopedInFront[0].GetComponent<AICARCONTROLLE2>().DistanceToWaypoint)
                                        {
                                            CurrentWaypoint = _nextWaypoint;
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
                            }
                        }
                    }
                }
                else if (_turnDirection == TurnDirection.Right)
                {
                    if (CurrentWaypoint.GetComponent<WayPoints>().Stop)
                    {
                        _numberOfcarsPassing = 0;
                        _carsStopedInFront = new List<GameObject>();
                        List<Collider> hitColliders = new List<Collider>();
                        List<Collider> hitColliders2 = new List<Collider>();
                        hitColliders = Physics.OverlapSphere(_checkFront.position, _maxDistance, _aiCarLayer ).ToList();
                        hitColliders2 = Physics.OverlapSphere(_checkFront.position, _maxDistance+10, _aiCarLayer).ToList();
                        for (var i = 0; i < hitColliders.Count; i++)
                        {
                            Transform tempTarget = hitColliders[i].transform;
                            Vector3 dir = tempTarget.position - _checkRight.position; // find target direction
                            Vector3 dir2 = tempTarget.position - _checkLeft.position; // find target direction
                            Vector3 myDir = _checkFront.forward;
                            Vector3 myDir2 = _checkFront.forward;
                            Vector3 yourDir = tempTarget.forward;
                            float myAngle = Vector3.Angle(myDir, dir);
                            float myAngle2 = Vector3.Angle(myDir2, dir2);
                            float yourAngle = Vector3.Angle(yourDir, -dir);
                            float yourAngle2 = Vector3.Angle(yourDir, -dir2);
                            if (Vector3.Angle(dir2, _checkLeft.right) <= 110 / 2 || Vector3.Angle(dir, -_checkRight.right) <=110 / 2)
                            {
                                if (yourAngle < 110 || yourAngle2 < 110)
                                {
                                  
                                    if (tempTarget.GetComponent<AICARCONTROLLE2>().CurrentWaypoint.GetComponent<WayPoints>().Stop == false)
                                    {
                                        Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, Color.yellow);

                                        if (tempTarget.gameObject != this.gameObject)
                                        {
                                            _numberOfcarsPassing++;
                                            _canMove = false;
                                            _side = true;
                                        }
                                    }

                                }
                                else
                                {
                                    Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, Color.red);

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
                            if (Vector3.Angle(dir3, _checkFront.forward) <= 110 / 2)
                            {
                                if (yourAngle3 < 90)
                                {
                                  
                                        _carsStopedInFront.Add(tempTarget.transform.gameObject);
                                        _carsStopedInFront = _carsStopedInFront.OrderBy(x => (this.transform.position - x.transform.position).sqrMagnitude).ToList();
                                        Debug.DrawRay(_checkFront.position, tempTarget.position - _checkFront.position, Color.white);
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
                                CurrentWaypoint = _nextWaypoint;
                                CheckPath();
                                _canMove = true;
                                _nextWaypoint = null;
                            }
                            else
                            {
                                if (_carsStopedInFront.Count == 0)
                                {
                                    CurrentWaypoint = _nextWaypoint;
                                    CheckPath();
                                    _canMove = true;
                                    _nextWaypoint = null;
                                    _front = false;
                                }
                                else
                                {
                                    if (_carsStopedInFront.Count >0)
                                    {
                                        if (_carsStopedInFront[0].GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Center)
                                        {
                                            CurrentWaypoint = _nextWaypoint;
                                            CheckPath();
                                            _canMove = true;
                                            _nextWaypoint = null;
                                        }
                                        if (_carsStopedInFront[0].GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Left)
                                        {
                                            CurrentWaypoint = _nextWaypoint;
                                            CheckPath();
                                            _canMove = true;
                                            _nextWaypoint = null;
                                        }
                                        if (_carsStopedInFront[0].GetComponent<AICARCONTROLLE2>()._turnDirection == TurnDirection.Right)
                                        {
                                            CurrentWaypoint = _nextWaypoint;
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
                        CurrentWaypoint = _nextWaypoint;
                        CheckPath();
                        _canMove = true;
                        _nextWaypoint = null;
                    }
                }
            }
        }
    }
    private void CheckPath()
    {
       
            int i = Random.Range(0, CurrentWaypoint.GetComponent<WayPoints>().NextWaypoint.Length);
            _nextWaypoint = CurrentWaypoint.GetComponent<WayPoints>().NextWaypoint[i];
        
    }
    private void MoveCar()
    {  
        foreach (var item in wheels)
        {
            _tempcarsinfront = new List<GameObject>();

            List<Collider> hitColliders2 = new List<Collider>();
            hitColliders2 = Physics.OverlapSphere(_checkFront.position, 15, _aiCarLayer+_playerCarLayer).ToList();

            for (var i = 0; i < hitColliders2.Count; i++)
            {
                Transform tempTarget = hitColliders2[i].transform;
                Vector3 dir3 = tempTarget.position - _checkFront.position; // find target direction
                Vector3 yourDir = tempTarget.forward;
                float yourAngle3 = Vector3.Angle(yourDir, -dir3);
                if (Vector3.Angle(dir3, _checkFront.forward) <= 50 / 2)
                {
                    if (yourAngle3 > 100)
                    {

                        _tempcarsinfront.Add(tempTarget.transform.gameObject);
                        _carInFront = true;
                    }
                   
                }
                if(_tempcarsinfront.Count==0)
                {
                     
                    
                        _carInFront = false;
                    
                }
            }
            
            if (_canMove)
            {
                if (_carInFront == false)
                {
                    if (Speed < CurrentWaypoint.GetComponent<WayPoints>().RoadMaxSpeed)
                    {
                        item.motorTorque = _totalPower;
                        item.brakeTorque = 0;


                    }
                    else if (Speed > CurrentWaypoint.GetComponent<WayPoints>().RoadMaxSpeed)
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
                    if (_carInFront == true)
                    {
                        if(_tempcarsinfront[0].GetComponent<AICARCONTROLLE2>() != null)
                        {
                            if (Speed < _tempcarsinfront[0].transform.GetComponent<AICARCONTROLLE2>().Speed)
                            {
                                item.motorTorque = _totalPower;
                                item.brakeTorque = 0;

                            }
                            else if (Speed > _tempcarsinfront[0].transform.GetComponent<AICARCONTROLLE2>().Speed)
                            {
                                item.motorTorque = 0;
                                item.brakeTorque = 1000;
                            }
                            else
                            {
                                item.motorTorque = 0;
                                item.brakeTorque = 0;
                            }
                        }
                        else
                            if (Speed < _tempcarsinfront[0].transform.GetComponent<CarControllerTest>().CurrentSpeed)
                        {
                            item.motorTorque = _totalPower;
                            item.brakeTorque = 0;

                        }
                        else if (Speed > _tempcarsinfront[0].transform.GetComponent<CarControllerTest>().CurrentSpeed - 5)
                        {
                            item.motorTorque = 0;
                            item.brakeTorque = 1000;
                        }
                        else
                        {
                            item.motorTorque = 0;
                            item.brakeTorque = 0;
                        }

                    }
                    else
                    {
                        if (_tempcarsinfront[0].GetComponent<AICARCONTROLLE2>() != null)
                        {
                            if (Speed < _tempcarsinfront[0].transform.GetComponent<AICARCONTROLLE2>().Speed)
                            {
                                item.motorTorque = _totalPower;
                                item.brakeTorque = 0;


                            }
                            else if (Speed > _tempcarsinfront[0].transform.GetComponent<AICARCONTROLLE2>().Speed)
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
                            if (Speed < _tempcarsinfront[0].transform.GetComponent<CarControllerTest>().CurrentSpeed)
                            {
                                item.motorTorque = _totalPower;
                                item.brakeTorque = 0;


                            }
                            else if (Speed > _tempcarsinfront[0].transform.GetComponent<CarControllerTest>().CurrentSpeed - 5)
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
        Vector3 relativeVector = transform.InverseTransformPoint(CurrentWaypoint.transform.position);
        relativeVector /= relativeVector.magnitude;
        float newSteer = (relativeVector.x / relativeVector.magnitude) * 4;
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
        foreach (WheelCollider element in wheels)
        {
            HandleWheelTransform(element);
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
    void HandleWheelTransform(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform tyre = collider.transform.GetChild(0);
        Vector3 position;
        Quaternion rotation;

        collider.GetWorldPose(out position, out rotation);
        tyre.transform.position = position;
        tyre.transform.rotation = rotation;
    }
}
