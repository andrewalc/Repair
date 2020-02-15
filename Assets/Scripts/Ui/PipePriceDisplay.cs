using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePriceDisplay : TextIntDisplay
{
    protected override int GetAmount(Simulation sim)
    {
        return (int)sim.config.pipePrice;
    }
}
