using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCollisionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other);
        if (other.gameObject.tag == "Target")
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }

    }

    //set colliding object to null
    void OnTriggerExit(Collider other)
    {

    }
}
