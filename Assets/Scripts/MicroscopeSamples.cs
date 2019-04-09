using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Phidget22;
using Phidget22.Events;

public class MicroscopeSamples
{
    public RFIDTag sampleTag;
    public string category;

    public MicroscopeSamples(RFIDTag newSampleTag, string newCatgory)
    {
        sampleTag = newSampleTag;
        category = newCatgory;
    }
}
