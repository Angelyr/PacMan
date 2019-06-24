using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class GameControl : MonoBehaviour {

    public TextAsset mapInput;
    public GameObject tile;
    public GameObject pellet;
    public GameObject node;
    public GameObject pacman;
    public GameObject teleport;
    public GameObject red;
    public GameObject blue;
    public GameObject pink;
    public GameObject orange;

    // Use this for initialization
    void Start () {
        loadMap();
	}

    //reads text file map and loads in game objects based on the locations
    void loadMap()
    {
        string map = reverse(mapInput.text);
        int x = 0;
        int y = 0;

        for(int i=0; i < map.Length; i++)
        {
            create(map, i, "o", orange, x, y);
            create(map, i, "p", pink, x, y);
            create(map, i, "b", blue, x, y);
            create(map, i, "r", red, x, y);
            create(map, i, "T", teleport, x, y);
            create(map, i, "P", pacman, x, y);
            create(map, i, "#", tile, x, y);
            create(map, i, ".", pellet, x, y);
            if (map.Substring(i, 1) != "#")
            {
                GameObject tempNode = Instantiate(node);
                tempNode.transform.position += new Vector3(x, y, 0);
            }
            if (map.Substring(i, 1) == "\n")
            {
                x = 0;
                y++;
                continue;
            }
            x++;
        }
    }

    //instantiates obj from map in location i to x,y
    void create(string map, int i, string tile, GameObject obj, int x, int y)
    {
        if (map.Substring(i, 1) == tile)
        {
            GameObject temp = Instantiate(obj);
            temp.transform.position += new Vector3(x, y, 0);
        }
    }

    //reverses the characters in input
    string reverse(string input)
    {
        string output = "";
        for(int i = input.Length-1; i >= 0; i--)
        {
            output += input.Substring(i, 1);
        }
        return output;
    }
}
