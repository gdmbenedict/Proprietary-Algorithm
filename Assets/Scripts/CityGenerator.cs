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
    [SerializeField][Range(0, 100)] private float randomRoadChance; //chance that a column or row on the grid will be a road

    [Header("Residential Variables")]
    [SerializeField] private int residentialCommercialAttraction; //how attracted is a residential structure to a commercial one
    [SerializeField] private int residentialIndustrialAttraction; //how attracted is a residential structure to a industrial one

    [Header("Commercial Varaibles")]
    [SerializeField] private int commercialResidentialAttraction; //how attracted is a commercial structure to a residential one
    [SerializeField] private int commercialIndustrialAttraction; //how attracted is a commercial structure to a industrial one

    [Header("Industrial Variables")]
    [SerializeField] private int industrialResidentialAttraction; //how attracted is a industrial structure to a residential one
    [SerializeField] private int industrialCommercialAttraction; //how attracted is a industrial structure to a commercial one

    [Header("Prefabs")]
    [SerializeField] private GameObject ground; 
    [SerializeField] private GameObject road;
    [SerializeField] private GameObject residential;
    [SerializeField] private GameObject commercial;
    [SerializeField] private GameObject industrial;

    private int totalPopulationScore;
    private int totalBusinessScore;
    private int totalInfrastructureScore;

    private Structure[,] structures;

    // Start is called before the first frame update
    void Start()
    {
        structures = new Structure[widthX, widthZ];
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
        //Adding roads to Z borders
        for (int x=0; x<widthX; x++)
        {
            
        }

        //Adding roads to X borders
        for (int z=0; z<widthZ; z++)
        {

        }
    }
}
