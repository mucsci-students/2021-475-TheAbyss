using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuShark : MonoBehaviour
{
    public Transform[] swimPoints;

    private int index = 0;

    void Update()
    {
        Vector3 relativePos = swimPoints[index].position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);

        transform.Translate(Vector3.forward * Time.deltaTime * 9.0f);

        if(Vector3.Distance(transform.position, swimPoints[index].position) <= 5.0f)
        {
            index++;
            if(index >= swimPoints.Length)
            {
                index = 0;
            }
        }
    }
}
