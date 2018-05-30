using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour {

    double stringOrigin = 0.1677596;

    private bool grabbing = false;
    public bool shootArrow = false;

    //Vars for Meshes
    private GameObject OpenHand;
    private GameObject ClosedHand;
    private MeshRenderer HandMeshRenderer;

    //Vars for picking up Obj's
    public GameObject collidingObject;
    public GameObject objectInHand;

    private GameObject Arrow;
    private GameObject BowString;

    public bool HandType;


    public GameObject prefabArrow;


    //get other collisider and set it as current colliding object
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other);
        if (other.gameObject.name != "PrefabHandLeft(Clone)" && other.gameObject.name != "PrefabHandRight(Clone)" && other.gameObject.name != "ArrowToShoot(Clone)" && other.gameObject.name != "ArrowToShootMesh")
        {
            if (other.GetComponent<Rigidbody>())
            {
                Debug.Log(other.gameObject);
                collidingObject = other.gameObject;
            }
        }
    }


    //set colliding object to null
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name != "PrefabHandLeft(Clone)" && other.gameObject.name != "PrefabHandRight(Clone)" && other.gameObject.name != "ArrowToShoot(Clone)" && other.gameObject.name != "ArrowToShootMesh")
        {
            collidingObject = null;
        }
    }


    // Use this for initialization
    void Start () {
       //get references to open and closed hand meshed
        ClosedHand = transform.GetChild(0).gameObject;
        OpenHand = transform.GetChild(1).gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        //right hand change mesh
        if (HandType)
        {
            //change mesh
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
        }
    }


    //funcs
    void GrabObject()
    {
        if (collidingObject.name != "Bullet")
        {
            objectInHand = collidingObject;
            if (objectInHand.name != "PrefabHandLeft" && objectInHand.name != "PrefabHandRight")
            {


                if (objectInHand.name == "Bow") {
                    BowScript temp = objectInHand.GetComponent<BowScript>();

                    if (temp.RightHandHolding == false && temp.LeftHandHolding == false)
                    {

                        if (HandType)
                        {
                            temp.RightHandHolding = true;
                        }
                        else
                        {
                            temp.LeftHandHolding = true;
                        }

                        objectInHand.transform.SetParent(this.transform);
                        objectInHand.GetComponent<Rigidbody>().isKinematic = true;
                    }

                    if (temp.RightHandHolding == true) {

                        if (!HandType)
                        {
                            BowString = objectInHand.transform.GetChild(0).transform.GetChild(0).gameObject;
                            Arrow = BowString.transform.GetChild(0).gameObject;
                            Arrow.GetComponent<MeshRenderer>().enabled = true;

                            //get delta location
                            var dist = Vector3.Distance(gameObject.transform.position, objectInHand.transform.GetChild(1).transform.position);
                            Debug.Log(dist);

                            BowString.transform.localPosition = new Vector3((dist) + 0.1677596f, BowString.transform.localPosition.y, BowString.transform.localPosition.z);
                            grabbing = true;
                            temp.SecondHandPulling = true;
                        }

                    }
                    else if (temp.LeftHandHolding == true)
                    {

                        if (HandType)
                        {
                            BowString = objectInHand.transform.GetChild(0).transform.GetChild(0).gameObject;
                            Arrow = BowString.transform.GetChild(0).gameObject;
                            Arrow.GetComponent<MeshRenderer>().enabled = true;
                            //get delta location

                            //get delta location
                            var dist = Vector3.Distance(gameObject.transform.position, objectInHand.transform.GetChild(1).transform.position);
                            Debug.Log(dist);

                            grabbing = true;
                            BowString.transform.localPosition = new Vector3((dist) + 0.1677596f, BowString.transform.localPosition.y, BowString.transform.localPosition.z);
                            temp.SecondHandPulling = true;
                        }

                    }

                }


            }
            ClosedHand.SetActive(false);
            OpenHand.SetActive(false);
        }
    }

    void ReleaseObject()
    {
        if (objectInHand.name != "PrefabHandLeft" && objectInHand.name != "PrefabHandRight" && objectInHand.name != "Bullet")
        {

            if (objectInHand.name == "Bow")
            {
                BowScript temp = objectInHand.GetComponent<BowScript>();


                if (temp.RightHandHolding == true)
                {

                    if (HandType)
                    {
                        BowString = objectInHand.transform.GetChild(0).transform.GetChild(0).gameObject;
                        Arrow = BowString.transform.GetChild(0).gameObject;
                        Arrow.GetComponent<MeshRenderer>().enabled = false;

                        temp.RightHandHolding = false;
                        objectInHand.GetComponent<Rigidbody>().isKinematic = false;
                        objectInHand.transform.SetParent(null);
                        objectInHand = null;
                    }
                    else {

                        if (temp.SecondHandPulling)
                        {
                            temp.SecondHandPulling = false;
                            shootArrow = true;
                        }
                        else {
                            BowString = objectInHand.transform.GetChild(0).transform.GetChild(0).gameObject;
                            Arrow = BowString.transform.GetChild(0).gameObject;
                            Arrow.GetComponent<MeshRenderer>().enabled = false;

                            BowString.transform.localPosition = new Vector3(0.1677596f, 0, 0);


                                ShootArrow();

                            shootArrow = false;
                        }

                    }

                }
                else if (temp.LeftHandHolding == true)
                {

                    if (!HandType)
                    {
                        BowString = objectInHand.transform.GetChild(0).transform.GetChild(0).gameObject;
                        Arrow = BowString.transform.GetChild(0).gameObject;
                        Arrow.GetComponent<MeshRenderer>().enabled = false;

                        temp.LeftHandHolding = false;
                        objectInHand.GetComponent<Rigidbody>().isKinematic = false;
                        objectInHand.transform.SetParent(null);
                        objectInHand = null;
                    }
                    else
                    {
                        if (temp.SecondHandPulling)
                        {
                            temp.SecondHandPulling = false;
                            shootArrow = true;

                        }
                        else
                        {
                            BowString = objectInHand.transform.GetChild(0).transform.GetChild(0).gameObject;
                            Arrow = BowString.transform.GetChild(0).gameObject;
                            Arrow.GetComponent<MeshRenderer>().enabled = false;

                            BowString.transform.localPosition = new Vector3(0.1677596f, 0, 0);
                            
                                ShootArrow();
                            
                            shootArrow = false;


                        }

                    }

                }
            }
        }
        ClosedHand.SetActive(false);
        OpenHand.SetActive(true);
    }


    void ShootArrow()
    {

        if (shootArrow == true) {
            GameObject arrow = Instantiate(prefabArrow, objectInHand.transform.GetChild(1).transform.GetChild(0).transform.position, objectInHand.transform.GetChild(1).transform.GetChild(0).transform.rotation);
            arrow.GetComponent<Rigidbody>().AddForce(arrow.transform.forward * 800);
           
            //arrow.transform.forward * 1000

            
            Destroy(arrow, 4);
            shootArrow = false;
        }


    }

}
