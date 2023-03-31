using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Functions
{
    public static ObjectFraming GetObjectFraming(int raysCount, int x, int y)
    {
        int middle = Mathf.FloorToInt(raysCount / 2);
        int middleRange = middle <= 5 ? 0 : Mathf.FloorToInt(middle / Mathf.FloorToInt(middle / 2));
        int third = raysCount / 3;
        int[] leftThird = new int[2];
        int[] rightThird = new int[2];
        leftThird[0] = Mathf.FloorToInt(third); leftThird[1] = Mathf.CeilToInt(third);
        rightThird[0] = Mathf.FloorToInt(third * 2); rightThird[1] = Mathf.CeilToInt(third * 2);

        //Debug.Log("X: " + x + " --- Y: "+ y);
        ObjectFraming objF;
        if (x == middle && y == middle)
        {
            objF = ObjectFraming.Center;
        }
        else if (x < middle - middleRange)
        {
            if (x >= leftThird[0]-1 && x <= leftThird[1]-1)
            {
                objF = ObjectFraming.Left;
            }
            else
            {
                objF = ObjectFraming.LeftGeneral;
            }
        }
        else if (x > middle + middleRange)
        {
            if (x >= rightThird[0] && x <= rightThird[1])
            {
                objF = ObjectFraming.Right;
            }
            else
            {
                objF = ObjectFraming.RightGeneral;
            }
        }
        else if ((x >= middle - middleRange && x <= middle + middleRange) && (y >= middle - middleRange && y <= middle + middleRange))
        {
            objF = ObjectFraming.Middle;
        }
        else
        {
            objF = ObjectFraming.General;
        }

        return objF;
    }
}
