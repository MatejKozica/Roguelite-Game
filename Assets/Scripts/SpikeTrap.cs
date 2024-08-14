using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public float downwardForce;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<LivingBase>() && collider.GetComponent<PlayerController>().GetVelocity().y < downwardForce)
        {
            collider.GetComponent<LivingBase>().OnDeath();
        }
    }
}
