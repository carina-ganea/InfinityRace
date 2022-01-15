using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;
    public GameObject cameraParent;
    public Transform[] cameraChild;
    public Vector3 offset;
    [Range(0,1)] public float smoothing = 0.5f;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        cameraParent = Player.transform.Find("CAMERA").gameObject;
        cameraChild = cameraParent.GetComponentsInChildren<Transform>();
    }
    void Update(){
        if (Input.GetKeyDown ("f")) {
            cameraParent.transform.rotation *= Quaternion.Euler(0, 180, 0);
        }
    }
    
    void FixedUpdate()
    {
        transform.position = cameraChild[2].position * (1 - smoothing) + transform.position * smoothing + offset;
        transform.LookAt(cameraChild[1].transform);
        
    }

}
