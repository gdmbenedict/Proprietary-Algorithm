using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    [Header("Generation Variables")]
    [SerializeField] private int widthX; //size of the generated grid in the X direction
    [SerializeField] private int widthZ; //size of the generated grid in the Z direction
    [SerializeField] private string seed; //seed used to give the map a desired generation

    [Header("Road Variables")]
    [SerializeField]
    [Range(0, 100)] private float randomRoadChance; //chance that a column or row on the grid will be a road

    [Header("Building Variables")]
    [SerializeField]
    [Range(1, 5)] private int maxBuildingSize; //maximum size of a building along any side
    [SerializeField]
    [Range(1, 25)] private int maxBuildingHeight;

    [Header("Prefabs")]
    [SerializeField] private GameObject ground; 
    [SerializeField] private GameObject road;
    [SerializeField] private GameObject building;

    private CityUnit[,] structures;

    //Below is code relating to expanded functionality that was not able to be finished

    /*
    [Header("Residential Variables")]
    [SerializeField] private int residentialCommercialAttraction; //how attracted is a residential structure to a commercial one
    [SerializeField] private int residentialIndustrialAttraction; //how attracted is a residential structure to a industrial one

    [Header("Commercial Varaibles")]
    [SerializeField] private int commercialResidentialAttraction; //how attracted is a commercial structure to a residential one
    [SerializeField] private int commercialIndustrialAttraction; //how attracted is a commercial structure to a industrial one

    
    [Header("Industrial Variables")]
    [SerializeField] private int industrialResidentialAttraction; //how attracted is a industrial structure to a residential one
    [SerializeField] private int industrialCommercialAttraction; //how attracted is a industrial structure to a commercial one
    */

    //[SerializeField] private GameObject residential;
    //[SerializeField] private GameObject commercial;
    //[SerializeField] private GameObject industrial;

    //private int totalPopulationScore;
    //private int totalBusinessScore;
    //private int totalInfrastructureScore;

    //private Structure[,] structures;

    // Start is called before the first frame update
    void Awake()
    {
        structures = new CityUnit[widthZ, widthX];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateCity()
    {
        GenerateTerrain();
        GenerateRoads();
    }

    //function that generates the terrain that the city-scale sits on
    private void GenerateTerrain()
    {
        //loop through every position placing the ground tile
        for (float x = -widthX/2; x<widthX/2; x++)
        {
            for (float z = -widthZ/2; z<widthZ/2; z++)
            {
                Vector3 position = new Vector3(x, -1, z);
                Instantiate(ground, position, Quaternion.identity);
            }
        }
    }

    //function that generates the road grid to block out city regions
    private void GenerateRoads()
    {
        int x,z;
        Vector3 position;

        //Adding roads to Z borders
        for (x=0; x<widthX; x++)
        {
            //top row
            z = 0;
            structures[z,x] = new Road();
            position = new Vector3(x - widthX / 2, 0, z - widthZ / 2);
            Instantiate(road, position, Quaternion.identity);

            //bottom row
            z = widthZ-1;
            structures[z, x] = new Road();
            position = new Vector3(x - widthX / 2, 0, z - widthZ / 2);
            Instantiate(road, position, Quaternion.identity);

        }

        //Adding roads to X borders
        for (z=0; z<widthZ; z++)
        {
            //top row
            x = 0;
            structures[z, x] = new Road();
            position = new Vector3(x - widthX / 2, 0, z - widthZ / 2);
            Instantiate(road, position, Quaternion.identity);

            //bottom row
            x = widthZ-1;
            structures[z, x] = new Road();
            position = new Vector3(x - widthX / 2, 0, z - widthZ / 2);
            Instantiate(road, position, Quaternion.identity);
        }
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

    //function that returns the max height of buildings in the cityscape
    public int GetMaxHeight()
    {
        return maxBuildingHeight;
    }
}
