using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    [Header("Generation Variables")]
    [SerializeField] private int widthX; //size of the generated grid in the X direction
    [SerializeField] private int widthZ; //size of the generated grid in the Z direction
    [SerializeField] private string seed; //seed used to give the map a desired generation

    [Header("Prefabs")]
    [SerializeField] private GameObject ground;
    [SerializeField] private GameObject road;
    [SerializeField] private GameObject residential;
    [SerializeField] private GameObject commercial;
    [SerializeField] private GameObject industrial;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //function that returns the largest side of the generated city
    public int GetLargestSide()
    {
        if (widthX > widthZ)
        {
            return widthX;
        }
        else
        {

            return widthZ;
        }
    }

    private void GenerateTerrain()
    {
        for (float x = -widthX/2; x<widthX/2; x++)
        {
            for (float z = -widthZ/2; z<widthZ/2; z++)
            {
                Vector3 position = new Vector3(x, -1, z);
                Instantiate(ground, position, Quaternion.identity);
            }
        }
    }

    private void GenerateRoads()
    {

    }
}
