using UnityEngine;

public class DamageTester : MonoBehaviour
{
    public Health target;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            target.TakeDamage(10);
        }
    }
}