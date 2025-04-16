using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Health>().TakeDamage(20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
