using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygon
{
    public List<int> triangle;

    public Polygon(int firstVertex, int secondVertex, int thirdVertex)
    {

        triangle = new List<int>() { firstVertex, secondVertex, thirdVertex };
    }
}
