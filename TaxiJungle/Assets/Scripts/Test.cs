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
        ChangeCameraTargets();
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
        switch (GameManager.instance.CurrentCarInUse.name)
        {
            case "Our taxi_ basic car (Clone)":
                var camera = _cam1.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
                camera.ShoulderOffset = new Vector3(0,2.7f,0);
                camera.CameraDistance = 5;
                break;
            case "Our taxi_ cadillac(Clone)":
                var camera2 = _cam1.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

                camera2.ShoulderOffset = new Vector3(0, 3, 0);
                camera2.CameraDistance = 8;
                break;
            case "our taxi_ mustang(Clone)":
                var camera3 = _cam1.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

                camera3.ShoulderOffset = new Vector3(0, 3, 0);
                camera3.CameraDistance = 6;
                break;


            default:
                break;
        }
       


    }
    public void ExitStoreCameraFix()
    {
        _cam1.transform.position= new Vector3(-235.063354f, 118.249199f, -91.0201416f);
        _cam2.transform.position = new Vector3(-235.063354f, 118.249199f, -91.0201416f);
    }
}
