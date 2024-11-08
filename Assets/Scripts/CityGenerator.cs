using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UIElements;

public class CityGenerator : MonoBehaviour
{
    [Header("External References")]
    [SerializeField] private Transform visualsHolder;

    [Header("Generation Variables")]
    [SerializeField] public int widthX; //size of the generated grid in the X direction
    [SerializeField] public int widthZ; //size of the generated grid in the Z direction
    [SerializeField] public string seed; //seed used to give the map a desired generation

    [Header("Road Variables")]
    [SerializeField]
    [Range(0, 1)] private float randomRoadChance; //chance that a column or row on the grid will be a road

    [Header("Building Variables")]
    [SerializeField]
    [Range(1, 5)] private int maxBuildingSize; //maximum size of a building along any side
    [SerializeField]
    [Range(1, 25)] private int maxBuildingHeight; // maximum height of buildings
    [SerializeField]
    [Range(0, 1)] private float buildingExpandChance; //chance building will expand horiztonally
    [SerializeField]
    [Range(0, 1)] private float buildingGrowChance; //chance building will grow vertically
    [SerializeField]
    [Range(0, 25)] private int buildingGrowRequirement; //requirement for building to grow vertically
    [SerializeField]
    [Range(1, 25)] private int buildingCheckSize; //size of check area for building growth requirement

    [Header("Prefabs")]
    [SerializeField] private GameObject ground; 
    [SerializeField] private GameObject road;
    [SerializeField] private GameObject buildingCorner;
    [SerializeField] private GameObject buildingWall;
    [SerializeField] private GameObject buildingCenter;
    [SerializeField] private GameObject buildingConnector;
    [SerializeField] private GameObject buildingEnd;
    [SerializeField] private GameObject building1x1;

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

    //Function that generates city
    public void GenerateCity(string seed)
    {
        //clearing previous generation
        foreach (Transform child in visualsHolder)
        {
            Destroy(child);
        }
        
        System.Random rand;

        if (string.IsNullOrEmpty(seed))
        {
            rand = new System.Random();
        }
        else
        {
            rand = new System.Random(seed.GetHashCode());
        }

        GenerateTerrain();
        GenerateRoads(rand);
        GenerateBuildings(rand);

        /*
        foreach (CityUnit cityUnit in structures)
        {

            Debug.Log(cityUnit);
        }
        */
    }

    //function that generates the terrain that the city-scale sits on
    private void GenerateTerrain()
    {
        GameObject instance;

        //loop through every position placing the ground tile
        for (float x = -widthX/2; x<widthX/2; x++)
        {
            for (float z = -widthZ/2; z<widthZ/2; z++)
            {
                Vector3 position = new Vector3(x, -1, z);
                instance = Instantiate(ground, position, Quaternion.identity);
                instance.transform.parent = visualsHolder.transform;
            }
        }
    }

    //function that generates the road grid to block out city regions
    private void GenerateRoads(System.Random rand)
    {
        int x,z;
        bool spawnRoad;

        //Adding roads to Z axis
        for (x=0; x<widthX; x++)
        {
            //top row
            z = 0;
            structures[z,x] = new Road();
            SpawnRoad(z, x);

            //bottom row
            z = widthZ-1;
            structures[z, x] = new Road();
            SpawnRoad(z, x); 
        }

        //Adding roads to X borders
        for (z=0; z<widthZ; z++)
        {
            //top row
            x = 0;
            structures[z, x] = new Road();
            SpawnRoad(z, x);

            //bottom row
            x = widthZ-1;
            structures[z, x] = new Road();
            SpawnRoad(z, x);    
        }

        //spawning x aligned roads
        for (x = 0; x < widthX; x++)
        {
            spawnRoad = (float)rand.NextDouble() < randomRoadChance;
            if (!spawnRoad) continue;

            if (x - 1 >= 0 && x + 1 < widthX)
            {
                //check for adjacent roads
                if (structures[1, x - 1] != null) continue;
                if (structures[1, x + 1] != null) continue;

            }

            for (z = 1; z < widthZ - 1; z++)
            {
                structures[z, x] = new Road();
                SpawnRoad(z, x);
            }

            spawnRoad = false;
        }

        
        //spawning z aligned roads
        for (z = 0; z < widthX; z++)
        {
            spawnRoad = (float)rand.NextDouble() < randomRoadChance;
            if (!spawnRoad) continue;

            if (z - 1 >= 0 && z + 1 < widthZ)
            {
                //check for adjacent roads
                if (structures[z - 1, 1] != null) continue;
                if (structures[z + 1, 1] != null) continue;

            }

            for (x = 1; x < widthX - 1; x++)
            {
                if (structures[z,x] != null) continue;
                structures[z, x] = new Road();
                SpawnRoad(z, x);
            }

            spawnRoad = false;
        }
        
    }

    //function that generates buildings in city
    private void GenerateBuildings(System.Random rand)
    {
        //declaring size variables
        int sizeX, sizeZ;
        bool hasGrown;
        Building building;

        //looping through x
        for (int x=1; x<widthX-1; x++)
        {
            for (int z=1; z<widthZ-1; z++)
            {
                //check that space is empty
                if (structures[z, x] != null) continue;
                //check if space is by a road
                if (!IsBesideRoad(z, x)) continue;

                //reset variables for determining building size
                sizeX = 1;
                sizeZ = 1;
                hasGrown = true;

                //determine building size
                while (hasGrown)
                {
                    hasGrown = false;

                    if (TryExpand(false, x, z + sizeZ - 1, rand))
                    {
                        hasGrown = true;
                        sizeZ++;
                    }
                    if (TryExpand(true, x + sizeX - 1, z, rand))
                    {
                        hasGrown = true;
                        sizeX++;
                    }
                }

                Debug.Log(sizeZ + "," + sizeX);
                building = new Building(sizeZ, sizeX);

                //setting building in structures
                for (int i=x; i<x+sizeX; i++)
                {
                    for (int j=z; j<z+sizeZ; j++)
                    {
                        structures[j,i] = building;
                    }
                }

                //spawning visual for building
                SpawnBuilding(z, x, sizeZ, sizeX, 1);
            }
        }
    }

    //function that spawns a road visual from map position
    private void SpawnRoad(float z, float x)
    {
        Vector3 position;
        GameObject instance;
        position = new Vector3(x - widthX / 2, 0, z - widthZ / 2);
        instance = Instantiate(road, position, Quaternion.identity);
        instance.transform.parent = visualsHolder.transform;
    }

    //function that spawns a building visual from a map position
    private void SpawnBuilding(int z, int x, int lengthZ, int lengthX, int height)
    {
        Vector3 position;
        GameObject instance;

        //check if building is 1x1
        if (lengthZ == 1 && lengthX == 1)
        {
            position = new Vector3(x - widthX / 2, height - 1, z - widthZ / 2);
            Instantiate(building1x1, position, Quaternion.identity);
        }
        else
        {
            int orientation;
            Quaternion rotation;

            for (int i=z; i<z+lengthZ; i++)
            {
                for (int j=x; j<x+lengthX; j++)
                {
                    //setting position
                    position = new Vector3(j - widthX / 2, height - 1, i - widthZ / 2);

                    //resetting orientation
                    orientation = 0;

                    //determining orientation using truth table
                    if (j + 1 < x + lengthX) {
                        orientation += 1;
                    }
                    if (j - 1 >= x)
                    {
                        orientation += 2;
                    }
                    if (i + 1 < z + lengthZ)
                    {
                        orientation += 4;
                    }
                    if (i -1 >= z)
                    {
                        orientation += 8;
                    }

                    //using orientation to spawn visual in right position
                    switch (orientation)
                    {
                        //left end
                        case 1:
                            rotation = Quaternion.Euler(0, 90, 0);
                            instance = Instantiate(buildingEnd, position, rotation);
                            instance.transform.parent = visualsHolder.transform;
                            break;

                        //right end
                        case 2:
                            rotation = Quaternion.Euler(0, 270, 0);
                            instance = Instantiate(buildingEnd, position, rotation);
                            instance.transform.parent = visualsHolder.transform;
                            break;

                        //right-left connector
                        case 3:
                            rotation = Quaternion.Euler(0, 90, 0);
                            instance = Instantiate(buildingConnector, position, rotation);
                            instance.transform.parent = visualsHolder.transform;
                            break;

                        //bottom end
                        case 4:
                            rotation = Quaternion.Euler(0, 0, 0);
                            instance = Instantiate(buildingEnd, position, rotation);
                            instance.transform.parent = visualsHolder.transform;
                            break;

                        //bottom left corner
                        case 5:
                            rotation = Quaternion.Euler(0, 0, 0);
                            instance = Instantiate(buildingCorner, position, rotation);
                            instance.transform.parent = visualsHolder.transform;
                            break;

                        //bottom right corner
                        case 6:
                            rotation = Quaternion.Euler(0, 270, 0);
                            instance = Instantiate(buildingCorner, position, rotation);
                            instance.transform.parent = visualsHolder.transform;
                            break;

                        //bottom wall
                        case 7:
                            rotation = Quaternion.Euler(0, 0, 0);
                            instance = Instantiate(buildingWall, position, rotation);
                            instance.transform.parent = visualsHolder.transform;
                            break;

                        //top end
                        case 8:
                            rotation = Quaternion.Euler(0, 180, 0);
                            instance = Instantiate(buildingEnd, position, rotation);
                            instance.transform.parent = visualsHolder.transform;
                            break;

                        //top left corner
                        case 9:
                            rotation = Quaternion.Euler(0, 90, 0);
                            instance = Instantiate(buildingCorner, position, rotation);
                            instance.transform.parent = visualsHolder.transform;
                            break;

                        //top right corner
                        case 10:
                            rotation = Quaternion.Euler(0, 180, 0);
                            instance = Instantiate(buildingCorner, position, rotation);
                            instance.transform.parent = visualsHolder.transform;
                            break;

                        //top wall
                        case 11:
                            rotation = Quaternion.Euler(0, 180, 0);
                            instance = Instantiate(buildingWall, position, rotation);
                            instance.transform.parent = visualsHolder.transform;
                            break;

                        //up-down connector
                        case 12:
                            rotation = Quaternion.Euler(0, 0, 0);
                            instance = Instantiate(buildingConnector, position, rotation);
                            instance.transform.parent = visualsHolder.transform;
                            break;

                        //left wall
                        case 13:
                            rotation = Quaternion.Euler(0, 90, 0);
                            instance = Instantiate(buildingWall, position, rotation);
                            instance.transform.parent = visualsHolder.transform;
                            break;

                        //right wall
                        case 14:
                            rotation = Quaternion.Euler(0, 270, 0);
                            instance = Instantiate(buildingWall, position, rotation);
                            instance.transform.parent = visualsHolder.transform;
                            break;

                        //middle section
                        case 15:
                            rotation = Quaternion.Euler(0, 0, 0);
                            instance = Instantiate(buildingCenter, position, rotation);
                            instance.transform.parent = visualsHolder.transform;
                            break;

                        //send debug message that an incorrect value was reached
                        default:
                            Debug.Log("Incorrect calculation of orientation: " + orientation);
                            break;
                    }
                }
            }
        }
    }

    //function that returns if the building can expand
    private bool TryExpand(bool xAxis, int x, int z, System.Random rand)
    {
        //randomly decide if can expand through random generation
        if (!((float)rand.NextDouble() < buildingExpandChance)) return false;

        //determine which axis to expand on
        if (xAxis)
        {
            //cannot expand if space is not empty
            if (structures[z, x + 1] != null) return false;
            return true;
        }
        else
        {
            //cannot expand if space is not empty
            if (structures[z + 1, x] != null) return false;
            return true;
        }
    }

    private bool IsBesideRoad(int z, int x)
    {
        //check right
        if(structures[z, x + 1] != null)
        {
            if (x + 1 < widthX && structures[z, x + 1].GetCityUnitType() == CityUnit.CityUnitType.road) return true;
        }

        //check left
        if (structures[z, x - 1] != null)
        {
            if (x + 1 < widthX && structures[z, x - 1].GetCityUnitType() == CityUnit.CityUnitType.road) return true;
        }

        //check up
        if (structures[z + 1, x] != null)
        {
            if (x + 1 < widthX && structures[z + 1, x].GetCityUnitType() == CityUnit.CityUnitType.road) return true;
        }

        //check down
        if (structures[z - 1, x] != null)
        {
            if (x + 1 < widthX && structures[z - 1, x].GetCityUnitType() == CityUnit.CityUnitType.road) return true;
        }

        return false;
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
