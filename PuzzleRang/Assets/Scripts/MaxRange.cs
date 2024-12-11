using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class MaxRange : MonoBehaviour
{
    int distance = 42;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if objects x/z values go out of the set area
        // Force them back into the game area
        if (gameObject.transform.position.x > distance)
        {
            transform.position = new Vector3(distance, 0, gameObject.transform.position.z);
        }
        else if (gameObject.transform.position.x < -distance)
        {
            transform.position = new Vector3(-distance, 0, gameObject.transform.position.z);
        }
        if (gameObject.transform.position.z > distance)
        {
            transform.position = new Vector3(gameObject.transform.position.x, 0, distance);
        }
        else if (gameObject.transform.position.z < -distance)
        {
            transform.position = new Vector3(gameObject.transform.position.x, 0, -distance);
        }

        // Check if object start floating
        // Put them back on the ground
        if (gameObject.transform.position.y > 1)
        {
            transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        }
    }
}
