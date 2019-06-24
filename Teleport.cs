using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    private GameObject otherTele;

	// Use this for initialization
	void Start ()
    {
        //finds the location of the other teleporter
        GameObject[] teleports = GameObject.FindGameObjectsWithTag("teleport");
        for(int i = 0; i < teleports.Length; i++)
        {
            if(teleports[i].transform.position != transform.position)
            {
                otherTele = teleports[i];
            }
        }
	}

    // Is called when trigger collider hits something
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Teleports packman or other enemy towards other teleporter
        if(collision != null && (collision.tag == "enemy" || collision.tag == "pacman"))
        {
            Vector3 target = new Vector2(-otherTele.transform.position.x, 0);
            collision.transform.position = otherTele.transform.position + target.normalized;
        }
    }
}
