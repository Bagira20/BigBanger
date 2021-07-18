using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundItself : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.RotateAround(gameObject.transform.position, Vector3.up, Time.deltaTime);

        gameObject.transform.RotateAround(gameObject.transform.position, Vector3.right, Time.deltaTime);

        gameObject.transform.RotateAround(gameObject.transform.position, Vector3.back, Time.deltaTime * 5);
    }
}
