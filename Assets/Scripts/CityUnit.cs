using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CityUnit
{
    public enum CityUnitType
    {
        road,
        building
    }

    private CityUnitType unitType; // type of structure in the city block
    private int height; // the height of the structure in the city block
    private int lengthX; //the size of the structure in the city block in the x direction
    private int lengthZ; //the size of the structure in the city block in the z direction

    //Constructor method
    public CityUnit(CityUnitType unitType, int height, int lengthZ, int lengthX)
    {
        this.unitType = unitType;
        this.height = height;
        this.lengthX = lengthX;
        this.lengthZ = lengthZ;
    }

    //function that increases the height of structure in city block by 1
    public void Grow()
    {
        height++;
    }

    //function that returns the type of the city unity
    public CityUnitType GetCityUnitType()
    {
        return unitType;
    }

    //function that returns the height of the structure
    public int GetHeight()
    {
        return height;
    }

    //Function that returns the size of the structure in a vector2
    public int[] GetSize()
    {
        return new int[2] {lengthZ, lengthX};
    }
    
}
