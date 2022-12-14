using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{
    [SerializeField] RectTransform _rect;
    // Start is called before the first frame update
    void Start()
    {
        _rect.localScale = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
