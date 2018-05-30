using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
    public float speed;
    public float cameraRotSpeed;
    public float rotClampMin;
    public float rotClampMax;
    public GameObject rot;
    private GameObject cameraRot;
    public GameObject getAnim;
    private Animator anim;
    private float angle;
	// Use this for initialization
	void Start () {
        cameraRot = transform.GetChild(1).gameObject;
        anim = getAnim.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        anim.SetFloat("VelocityX", Input.GetAxis("Horizontal"));
        anim.SetFloat("VelocityY", Input.GetAxis("Vertical"));
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        float y = Input.GetAxis("Mouse Y");
        transform.Translate(hor * speed,0,ver * speed);
        transform.Rotate(0, Input.GetAxis("Mouse X") * cameraRotSpeed, 0);
        angle -= y * cameraRotSpeed;
        var clampedAngle = Mathf.Clamp(angle, rotClampMin, rotClampMax);
        cameraRot.transform.localRotation = Quaternion.Euler(clampedAngle, 0, 0);
    }
}
