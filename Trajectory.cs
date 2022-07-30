using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Numerics;
using System.IO;
public class Coordinate
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public UnityEngine.Vector3 Coords { get; set; }
    public float Thrust { get; set; }
    public Coordinate(UnityEngine.Vector3 coords, float thrust)
    {
        Coords = coords;
        Thrust = thrust;
    }
}

public class NewTrajectory : MonoBehaviour
{
    public TextAsset coordinateData;
    public List<Coordinate> refuelerCoordinates = new List<Coordinate>();
    public List<Coordinate> jwstCoordinates = new List<Coordinate>();
    public List<Coordinate> earthCoordinates = new List<Coordinate>();
    public List<Coordinate> moonCoordinates = new List<Coordinate>();

    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;
    public GameObject obj4;
    public TrailRenderer trail1;
    public TrailRenderer trail2;
    public TrailRenderer trail3;
    public TrailRenderer trail4;
    public TrailRenderer trailT;

    int tracker = 0;

    void Start()
    {

        string[] coordinateRows = coordinateData.text.Split('\n'); // Divides .csv into rows
        for (int i = 1; i < coordinateRows.Length; i++) // Loops through all rows
        {
            // Adds row contents to respective List
            string[] coordinates = coordinateRows[i].Split(',');
            refuelerCoordinates.Add(new Coordinate(new UnityEngine.Vector3(float.Parse(coordinates[1]), float.Parse(coordinates[2]), float.Parse(coordinates[3])), float.Parse(coordinates[4])));
            jwstCoordinates.Add(new Coordinate(new UnityEngine.Vector3(float.Parse(coordinates[5]), float.Parse(coordinates[6]), float.Parse(coordinates[7])), 0f));
            earthCoordinates.Add(new Coordinate(new UnityEngine.Vector3(float.Parse(coordinates[8]), float.Parse(coordinates[9]), float.Parse(coordinates[10])), 0f));
            moonCoordinates.Add(new Coordinate(new UnityEngine.Vector3(float.Parse(coordinates[11]), float.Parse(coordinates[12]), float.Parse(coordinates[13])), 0f));
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(tracker < 20000)
        {
            // Creates vectors out of information from the Lists, uses this to move around objects/trails
            UnityEngine.Vector3 refuelerVector = refuelerCoordinates[tracker].Coords;
            trail1.AddPosition(refuelerVector);
            trail1.transform.position = refuelerVector;
            obj1.transform.position = refuelerVector;

            // Uses the current and previous points to make a "thrust" trail that's tangent to the actual orbit trail
            trailT.Clear();
            trailT.AddPosition(refuelerVector);
            trailT.transform.position = refuelerVector;
            UnityEngine.Vector3 previousRefuelerVector;

            if (tracker > 1)
            {
                previousRefuelerVector = refuelerCoordinates[tracker - 1].Coords;

                //Finds the difference in each plane between new and old coordinate
                float xDiff = refuelerVector.x - previousRefuelerVector.x;
                float yDiff = refuelerVector.y - previousRefuelerVector.y;
                float zDiff = refuelerVector.z - previousRefuelerVector.z;

                trailT.AddPosition(previousRefuelerVector);

                //Proportionally subtracts the differences to create a third point in line with the current and previous points
                trailT.AddPosition(new UnityEngine.Vector3(previousRefuelerVector.x - xDiff * 20, previousRefuelerVector.y - yDiff * 20, previousRefuelerVector.z - zDiff * 20));
            }



            // Checks if thrust is active
            if (refuelerCoordinates[tracker].Thrust == 1f)
            {
                trailT.startColor = new Color(1, 0, 0, 1);
                trailT.endColor = new Color(1, 0, 0, 1);
            } else
            {
                trailT.startColor = new Color(1, 0, 0, 0);
                trailT.endColor = new Color(1, 0, 0, 0);
            }

            UnityEngine.Vector3 jwstVector = jwstCoordinates[tracker].Coords;
            trail2.AddPosition(jwstVector);
            trail2.transform.position = jwstVector;
            obj2.transform.position = jwstVector;

            UnityEngine.Vector3 earthVector = earthCoordinates[tracker].Coords;
            trail3.AddPosition(earthVector);
            trail3.transform.position = earthVector;
            obj3.transform.position = earthVector;

            UnityEngine.Vector3 moonVector = moonCoordinates[tracker].Coords;
            trail4.AddPosition(moonVector);
            trail4.transform.position = moonVector;
            obj4.transform.position = moonVector;
            tracker++;

        }
    }
}
