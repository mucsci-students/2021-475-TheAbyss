using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SharkManager : MonoBehaviour
{
    [Serializable]
    public class PointOfInterest
    {
        public string key;
        public Transform point;
    }


    public GameObject shark;
    
    [SerializeField]
    public PointOfInterest[] points;
    private Dictionary<string, Transform> m_pointsOfInterest;
    
    

    void Start()
    {
        m_pointsOfInterest = new Dictionary<string, Transform>();
        foreach(PointOfInterest p in points)
        {
            string newKey = p.key;
            Transform newTransform = p.point;
            m_pointsOfInterest.Add(newKey, newTransform);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            SetSharkInterestPointToKey("StartingArea");
        }
    }

    public void TeleportSharkToPoint(Vector3 point)
    {
        shark.transform.position = point;
    }

    public void SetNewPointOfInterest(Transform point)
    {
        shark.GetComponent<SharkAI>().SetInterestPoint(point);
    }

    public void SetPatrolRadius(float radius)
    {
        shark.GetComponent<SharkAI>().patrolRadius = radius;
    }

    public void TeleportSharkToKeyPoint(string key)
    {
        Transform t;
        bool found = m_pointsOfInterest.TryGetValue(key, out t);
        if(found)
        {
            shark.GetComponent<SharkAI>().TeleportSharkToTransform(t);
        }
        else
        {
            Debug.LogError("The key \"" + key + "\" does not exist under SharkManager. Please add a Transform with a key to the SharkManager.");
        }
    }

    public void SetSharkInterestPointToKey(string key)
    {
        Transform t;
        bool found = m_pointsOfInterest.TryGetValue(key, out t);
        if(found)
        {
            shark.GetComponent<SharkAI>().SetInterestPoint(t);
        }
        else
        {
            Debug.LogError("The key \"" + key + "\" does not exist under SharkManager. Please add a Transform with a key to the SharkManager.");
        }
    }
}