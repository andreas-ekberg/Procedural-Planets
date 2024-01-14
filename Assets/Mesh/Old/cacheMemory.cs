using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cacheMemory
{
    public int index;
    public int smallerIndex;
    public int biggerIndex;

    public cacheMemory(int _index, int _smallerIndex, int _biggerIndex)
    {
        index = _index;
        smallerIndex = _smallerIndex;
        biggerIndex = _biggerIndex;
    }

    public bool containsIndex() { return true; }
}
