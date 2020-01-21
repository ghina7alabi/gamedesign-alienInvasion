using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sugarcanes : MonoBehaviour {

    public static int health = 0;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "guard" || collision.gameObject.name == "Player" || collision.gameObject.name == "Floor")
        {
            health++;
            Destroy(this.gameObject);
        }
    }
}
