using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprinklerPriceDisplay : TextIntDisplay
{
    protected override int GetAmount(Simulation sim)
    {
        return (int)sim.config.sprinklerPrice;
    }
}
