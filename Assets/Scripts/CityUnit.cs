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

    public CityUnit(CityUnitType unitType, int height)
    {
        this.unitType = unitType;
        this.height = height;
    }

    public CityUnitType GetCityUnitType()
    {
        return unitType;
    }

    public int GetHeight()
    {
        return height;
    }
    
}
