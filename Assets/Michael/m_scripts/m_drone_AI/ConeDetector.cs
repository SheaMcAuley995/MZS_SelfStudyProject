using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeDetector : MonoBehaviour {

    public List<GameObject> scraps;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            if(!scraps.Contains(other.gameObject))
            {
                scraps.Add(other.gameObject);
                if (other.gameObject.activeSelf == false)
                {
                    scraps.Remove(other.gameObject);
                }
            }
            
        }
        
    }
}
