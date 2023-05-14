using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilmingCamara : MonoBehaviour
{
    [SerializeField] Transform _target;
    // Start is called before the first frame update
    void Start()
    {
       _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_target);
    }
}
