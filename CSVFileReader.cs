using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using PathCreation;


public class CSVFileReader : MonoBehaviour
{
    // Start is called before the first frame update
    public TextAsset csvFile;
    private int canvasIndex = 0;
    public Shader shader;
    public List<Vector3> coordinates = new List<Vector3>();
    public float xMax = 0;
    public float xMin = 0;
    public float yMax = 0;
    public float yMin = 0;
    public float zMax = 0;
    public float zMin = 0;
    public float newY = 0;

    // Update is called once per frame
    void Start()
    {
        readCSV();

    }

    void Update()
    {

    }

    void readCSV()
    {
        string[] records = csvFile.text.Split('\n');


        for (int i = 1; i < records.Length; i++)
        {
            string[] fields = records[i].Split(',');

            // Check for data set max/min in each direction

            if (float.Parse(fields[0]) > xMax)
            {
                xMax = float.Parse(fields[0]);
            } 
            else if (float.Parse(fields[0]) < xMin)
            {
                xMin = float.Parse(fields[0]);
            }


            if (float.Parse(fields[1]) > yMax)
            {
                yMax = float.Parse(fields[1]);
            }
            else if (float.Parse(fields[1]) < yMin)
            {
                yMin = float.Parse(fields[1]);
            }


            if (float.Parse(fields[2]) > zMax)
            {
                zMax = float.Parse(fields[2]);
            }
            else if (float.Parse(fields[2]) < zMin)
            {
                zMin = float.Parse(fields[2]);
            }
        }


        for (int i = 1; i < records.Length; i++)
        {
            string[] fields = records[i].Split(',');

            // Adjusts all Y values above the plane
            newY = float.Parse(fields[1]) + Mathf.Abs(yMin) + 20;

            coordinates.Add(new Vector3(float.Parse(fields[0]), newY, float.Parse(fields[2])));
            //cubeTest.transform.position = new Vector3(float.Parse(fields[0]), float.Parse(fields[1]), float.Parse(fields[2]));       (direct transformation)
        }

        Debug.Log($"X range: {xMin} to {xMax} \nY range: {yMin} to {yMax} \nZ range: {zMin} to {zMax}\nNew Y: {newY}");
        var coordinatesArray = coordinates.ToArray();

        for (int i = 1; i < (coordinatesArray.Length); i++)
        {
            Vector3 startv = coordinatesArray[i - 1];
            Vector3 endv = coordinatesArray[i];

            // Can change color of line to represent thrust

            createLine(startv, endv, 0.25f, Color.blue);
        }

    }


    private void createLine(Vector3 start, Vector3 end, float lineSize, Color c)
    {
        GameObject canvas = new GameObject("canvas" + canvasIndex);
        canvas.transform.parent = transform;
        canvas.transform.rotation = transform.rotation;
        LineRenderer lines = (LineRenderer)canvas.AddComponent<LineRenderer>();
        lines.material = new Material(shader);
        lines.material.color = c;
        lines.useWorldSpace = false;
        lines.SetWidth(lineSize, lineSize);
        lines.SetVertexCount(2);
        lines.SetPosition(0, start);
        lines.SetPosition(1, end);
        canvasIndex++;
    }

    void OnDrawGizmos()
    {

        var coordinatesArray = coordinates.ToArray();
        for (int i = 1; i < (coordinatesArray.Length); i++)
        {   
            Vector3 startv = coordinatesArray[i - 1];
            Vector3 endv = coordinatesArray[i];
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(startv, endv);
        }
    }

}
