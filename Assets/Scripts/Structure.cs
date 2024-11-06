using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Structure
{
    public enum StructureType
    {
        road,
        residential,
        commercial,
        industrial
    }

    //Dimensional information
    private int size;
    private int widthX;
    private int widthZ;
    private int height;

    //resource information
    private int population;
    private int business;
    private int infrastructure;

    //structure type info
    private StructureType structureType;

    //Population calculation values
    private float populationPow; //power for the population function
    private float populationMult; //multiplier for population function
    private float populationVShift; //verticle shift in population function
    private float populationHShift; //horizontal shift in population function

    //Business calculation values
    private float businessPow; //power for the business function
    private float businessMult; //multiplier for business function
    private float businessVShift; //verticle shift in business function
    private float businessHShift; //horizontal shift in business function

    //Infrastrucuter calculation values
    private float infrastructurePow; //power for the infrastructure function
    private float infrastructureMult; //multiplier for infrastructure function
    private float infrastrucureVShift; //verticle shift in infrastructure function
    private float infrastructureHShift; //horizontal shift in infrastructure function

    //Empty constructor method
    public Structure()
    {

    }

    //Method that sets internal varaibles
    public void SetSize(int widthX, int widthZ, int height)
    {
        this.widthX = widthX;
        this.widthZ = widthZ;
        this.height = height;

        GenerateAttributes();
    }

    //Method that expands the structure by one on either the X-axis or Z-axis
    public void Expand(bool Xaxis)
    {
        if (Xaxis)
        {
            widthX++;
        }
        else
        {
            widthZ++;
        }

        GenerateAttributes();
    }

    //Method that grows the structure by one on height
    public void Grow()
    {
        height++;
        GenerateAttributes();
    }

    //Method that calculates the attributes of the structre (size and resources)
    private void GenerateAttributes()
    {
        ApplySize();
        CalculateResources(GetSize());
    }

    //Method that calculates the size of the structure and updates it
    private void ApplySize()
    {
        size = widthX * widthZ * height;
    }

    //Abstract method that calculatess the resources that a structure contribues
    public int[] CalculateResources(int size)
    {
        int[] resources;
        int population;
        int business;
        int infrastructure;

        population = -CalculateFunction(size, populationPow, populationMult, populationVShift, populationHShift); //aplying function to get population value
        business = CalculateFunction(size, businessPow, businessMult, businessVShift, businessHShift); //applying function to get business value
        infrastructure = -CalculateFunction(size, infrastructurePow, infrastructureMult, infrastrucureVShift, infrastructureHShift); //applying function to get infrastructure value

        resources = new int[3] { population, business, infrastructure };
        return resources;
    }

    //Accessor method for population
    public int GetPopulation()
    {
        return population;
    }

    //Accessor method for business
    public int GetBusiness()
    {
        return business;
    }

    //Accessor method that returns size
    public int GetSize()
    {
        return size;
    }

    public void SetResources(int[] resources)
    {
        population = resources[0];
        business = resources[1];
        infrastructure = resources[2];
    }

    //Accessor method for infrastructure
    public int GetInfrastructure()
    {
        return infrastructure;
    }

    public void SetStructureType(StructureType structureType)
    {
        this.structureType = structureType;
    }

    //Accessor method for structure type
    public StructureType GetStructureType()
    {
        return structureType;
    }

    //Function for calculating building resource values
    public int CalculateFunction(int size, float power, float multiplier, float vertifcalShift, float horizontalShift)
    {
        //applying universal graphing function
        int value = size * Mathf.RoundToInt(multiplier * Mathf.Pow(size - horizontalShift, power) + vertifcalShift);
        return value;
    }
}
