using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundItself : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.RotateAround(gameObject.transform.position, Vector3.up, Time.deltaTime * Random.Range(0f, 4f));

        gameObject.transform.RotateAround(gameObject.transform.position, Vector3.right, Time.deltaTime * Random.Range(1f, 7f));

        gameObject.transform.RotateAround(gameObject.transform.position, Vector3.back, Time.deltaTime * Random.Range(3f, 15f));
    }
}
