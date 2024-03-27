using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{   
    public float rotationSpeed;
    public FloatingJoystick joystick;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
       if (player.transform.position.y > -1f){
         transform.position = player.transform.position + new Vector3(0f, 5f, 0f);
        transform.Rotate(Vector3.up, joystick.Horizontal * rotationSpeed * Time.deltaTime);
       }
    }
}
