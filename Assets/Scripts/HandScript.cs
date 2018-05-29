using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour {

    //Vars for Meshes
    private GameObject OpenHand;
    private GameObject ClosedHand;
    private MeshRenderer HandMeshRenderer;

    //Vars for picking up Obj's
    public GameObject collidingObject;
    public GameObject objectInHand;

    public bool HandType;

    public GameObject prefabBullet;

    private bool Shot = false;
    private IEnumerator coroutine;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.GetComponent<Rigidbody>())
        {
            Debug.Log(other.gameObject);
            collidingObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        collidingObject = null;
    }

    // Use this for initialization
    void Start () {

       //get references to hand types
        ClosedHand = transform.GetChild(0).gameObject;
        OpenHand = transform.GetChild(1).gameObject;



    }
	
	// Update is called once per frame
	void Update () {

        //right hand change mesh
        if (HandType)
        {
            if (Input.GetButtonDown("XBOX_RIGHT_BUMPER"))
            {
                ClosedHand.SetActive(true);
                OpenHand.SetActive(false);
            }
            else if (Input.GetButtonUp("XBOX_RIGHT_BUMPER"))
            {
                ClosedHand.SetActive(false);
                OpenHand.SetActive(true);
            }

            if (Input.GetAxis("XBOX_RIGHT_BUMPER") > 0.2 && collidingObject)
            {
                GrabObject();
            }
            if (Input.GetAxis("XBOX_RIGHT_BUMPER") < 0.2 && objectInHand)
            {
                ReleaseObject();
            }

            if (Input.GetAxis("CONTROLLER_RIGHT_TRIGGER") > 0.2 && objectInHand.gameObject.name == "Gun")
            {
                if (Shot == false)
                {
                    Shoot();
                }
            }

        }
        else
        {
            if (Input.GetButtonDown("XBOX_LEFT_BUMPER"))
            {
                ClosedHand.SetActive(true);
                OpenHand.SetActive(false);
            }
            else if (Input.GetButtonUp("XBOX_LEFT_BUMPER"))
            {
                ClosedHand.SetActive(false);
                OpenHand.SetActive(true);
            }

            if (Input.GetAxis("XBOX_LEFT_BUMPER") > 0.2 && collidingObject)
            {
                GrabObject();
            }
            if (Input.GetAxis("XBOX_LEFT_BUMPER") < 0.2 && objectInHand)
            {
                ReleaseObject();
            }

            if (Input.GetAxis("CONTROLLER_LEFT_TRIGGER") > 0.2 && objectInHand.gameObject.name == "Gun")
            {
                if (Shot == false)
                {
                    Shoot();
                } 
            }
        }
    }


    //funcs
    void GrabObject()
    {
        if (collidingObject.name != "Bullet")
        {


            objectInHand = collidingObject;
            if (objectInHand.name != "PrefabHandLeft" || objectInHand.name != "PrefabHandRight")
            {
                objectInHand.transform.SetParent(this.transform);
                objectInHand.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    void ReleaseObject()
    {
        if (objectInHand.name != "PrefabHandLeft" || objectInHand.name != "PrefabHandRight" || objectInHand.name != "Bullet")
        {
            objectInHand.GetComponent<Rigidbody>().isKinematic = false;
            objectInHand.transform.SetParent(null);
            objectInHand = null;
        }
    }
    void Shoot()
    {
        Shot = true;
        GameObject bullet = Instantiate(prefabBullet, objectInHand.transform.position, objectInHand.transform.rotation);
        //bullet.transform.eulerAngles = new Vector3(90, bullet.transform.eulerAngles.y, 90);
        bullet.transform.position += bullet.transform.forward * 0.1f;
        bullet.transform.position += bullet.transform.up * 0.08f;
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 1000);

        coroutine = WaitAndPrint();
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitAndPrint()
    {
            yield return new WaitForSeconds(0.3f);
            Shot = false;
    }
}
