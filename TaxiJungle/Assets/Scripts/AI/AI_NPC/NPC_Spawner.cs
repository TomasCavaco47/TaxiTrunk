using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Spawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _pedestriansPrefab;
    [SerializeField] private int _pedestriansToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(_pedestriansPrefab.Count);
        StartCoroutine(Spawn());
    }
    IEnumerator Spawn()
    {
        int count = 0;
        while (count < _pedestriansToSpawn)
        {
            
            GameObject obj = Instantiate(_pedestriansPrefab[Random.Range(0,_pedestriansPrefab.Count)]);
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
