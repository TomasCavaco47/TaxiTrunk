using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _pedestrianPrefab;
    [SerializeField] private int _pedestriansToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }
    IEnumerator Spawn()
    {
        int count = 0;
        while (count < _pedestriansToSpawn)
        {
            GameObject obj = Instantiate(_pedestrianPrefab);
            Transform child = transform.GetChild(Random.Range(0, transform.childCount - 1));
            obj.GetComponent<NPC_WaypointNavigator>().CurrentWaypoint = child.GetComponent<NPC_Waypoint>();
            obj.transform.position=child.position;
            yield return new WaitForEndOfFrame();
            count++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
