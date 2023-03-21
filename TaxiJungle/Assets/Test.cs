using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Test : MonoBehaviour
{
    bool _mainCamera=true;

    public static Test instance;

  [SerializeField]  CinemachineVirtualCamera _cam1;
   [SerializeField] CinemachineVirtualCamera _cam2;
    // Start is called before the first frame update
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
    private void Start()
    {
        _cam1.Follow = GameManager.instance.CurrentCarInUse.transform;
        _cam1.LookAt = GameManager.instance.CurrentCarInUse.transform;
        _cam2.Follow = GameManager.instance.CurrentCarInUse.transform;
        _cam2.LookAt = GameManager.instance.CurrentCarInUse.transform;
    }

    public void ChangeCameraReverse()
    {
        _cam1.Priority = 0;
        _cam2.Priority = 1;
    }
    public void ChangeCameraFowards()
    {
        _cam1.Priority = 1;
        _cam2.Priority = 0;
    }
    public void ChangeCameraTargets()
    {
        _cam1.Follow = GameManager.instance.CurrentCarInUse.transform;
        _cam1.LookAt = GameManager.instance.CurrentCarInUse.transform;
        _cam2.Follow = GameManager.instance.CurrentCarInUse.transform;
        _cam2.LookAt = GameManager.instance.CurrentCarInUse.transform;
    }
}