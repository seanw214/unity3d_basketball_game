using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtBBRim : MonoBehaviour
{

    public GameObject target;
    public GameObject parentGameObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.rotation = Quaternion.LookRotation(target.transform.position);
        //this.transform.localRotation = Quaternion.LookRotation(target.transform.position);
        this.transform.LookAt(target.transform);
        this.transform.position = parentGameObject.transform.position;
    }
}
