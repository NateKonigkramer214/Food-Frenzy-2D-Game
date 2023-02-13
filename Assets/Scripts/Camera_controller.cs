using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_controller : MonoBehaviour
{
    public Transform player;
    
    // Update is called once per frame
    private void Update()
    {
        //Changes camera when player position changes
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }

    
}
