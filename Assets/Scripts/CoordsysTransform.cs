﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordsysTransform : MonoBehaviour {

    public Transform testTransform;

    //We are transforming from vTriangleFrom to vTriangleTo
    private Vector3[] vTriangleFrom = new Vector3[3];
    private Vector3[] vTriangleTo = new Vector3[3];

    public float scaling;
    //Meant are the corners of the triangles
    Vector3 cornerToCorner;
    Vector3 cornerToOrigin;
    Quaternion rot1;
    Quaternion rot2;

    private int id = 0;

    public void SavePositionPair(Vector3 from, Vector3 to)
    {
        if (id == 3)
            throw new Exception("Already saved 3 pairs of positions");

        vTriangleFrom[id] = from;
        vTriangleTo[id] = to;
        id++;
    }

    //Creates this transformation based on the saved positions
    //To be called after 3 pairs of positions have been saved
    public void Create()
    {
        //Transform corner to corner
        cornerToCorner = vTriangleTo[0] - vTriangleFrom[0];

        //Calculate scaling
        scaling = Vector3.Distance(vTriangleTo[0], vTriangleTo[1])
            / Vector3.Distance(vTriangleFrom[0], vTriangleFrom[1]);
        
        //Calculate rotation from a side in one triangle to the corresponding side in the other triangle
        rot1 = Quaternion.FromToRotation(vTriangleFrom[1] - vTriangleFrom[0],
            vTriangleTo[1] - vTriangleTo[0]);

        cornerToOrigin = -vTriangleFrom[0];
       
        //Apply calculated transformation to the first triangle
        for (int i = 0; i < vTriangleFrom.Length; i++)
        {
            vTriangleFrom[i] += cornerToOrigin;
            vTriangleFrom[i] = rot1 * vTriangleFrom[i];
        }

        //Calculate rotation from the second side in one triangle to the corresponding side in the other triangle
        rot2 = Quaternion.FromToRotation(vTriangleFrom[2] - vTriangleFrom[0],
            vTriangleTo[2] - vTriangleTo[0]);
    }

    //Applies the calibrated / calculated transformation to v
    public Vector3 ApplyTo(Vector3 v)
    {
        v += cornerToOrigin;
        v = rot1 * v;
        v = rot2 * v;
        v *= scaling;
        v -= cornerToOrigin;
        v += cornerToCorner;

        return v;
    }


}