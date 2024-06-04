using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
   private void OnCollisionEnter(Collision collision)
   {
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit" + collision.gameObject.name + "!");
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            print("hit the Enemy");
            Destroy(gameObject);
        }
   }
}
