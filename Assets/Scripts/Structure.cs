using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public abstract class Structure
{
    public enum StructureType
    {
        road,
        residential,
        business,
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

    private StructureType structureType;

    //Construction method for structure
    public Structure(int widthX, int widthZ, int height)
    {
        this.widthX = widthX;
        this.widthZ = widthZ;
        this.height = height;

        CalculateAttributes();
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

        CalculateAttributes();
    }

    //Method that grows the structure by one on height
    public void Grow()
    {
        height++;
        CalculateAttributes();
    }

    //Method that calculates the attributes of the structre (size and resources)
    private void CalculateAttributes()
    {
        CalculateSize();
        CalculateResources();
    }

    //Method that calculates the size of the structure and updates it
    private void CalculateSize()
    {
        size = widthX * widthZ * height;
    }

    //Abstract method that calculatess the resources that a structure contribues
    public abstract void CalculateResources();

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

    //Accessor method for infrastructure
    public int GetInfrastructure()
    {
        return infrastructure;
    }

    //Accessor method for structure type
    public StructureType GetStructureType()
    {
        return structureType;
    }
}
