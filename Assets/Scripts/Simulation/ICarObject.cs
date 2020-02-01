using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICarObject
{
    CarObjectType Type { get; }

    float GetPlantGrowthModifier();

    bool BlocksIrrigation();

    bool IsEmpty();

    bool IsWaterSource();
}
