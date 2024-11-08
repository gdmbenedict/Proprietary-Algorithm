using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CityUnit;

public class Building : CityUnit
{
    public Building(int lengthZ, int lengthX) : base(CityUnitType.road, 1, lengthZ, lengthX)
    {

    }
}
