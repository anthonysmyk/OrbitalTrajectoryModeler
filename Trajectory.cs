using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Numerics;
using System.IO;


public class Trajectory : MonoBehaviour
{
    public List<List<UnityEngine.Vector3>> vectors;
    public List<List<BigInteger>> times;
    public List<Boolean> thrusts;
    public List<TextAsset> files;
    public List<TrailRenderer> trails;
    public List<int> trackers;
    public List<GameObject> objects;

    public TextAsset file1;
    public TextAsset file2;
    public TextAsset file3;
    public TextAsset file4;
    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;
    public GameObject obj4;
    public TrailRenderer trail1;
    public TrailRenderer trail2;
    public TrailRenderer trail3;
    public TrailRenderer trail4;
    public TrailRenderer trailT;

    float oldTime = -1;
    float newTime = 0;
    BigInteger minTime = 6493275631;
    BigInteger cycles = 200000;
    BigInteger prevCycleEnd = 0;


    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 4.0f;
        //Time.fixedDeltaTime = 0.1f;
        List<UnityEngine.Vector3> vectors1 = new List<UnityEngine.Vector3>();
        List<UnityEngine.Vector3> vectors2 = new List<UnityEngine.Vector3>();
        List<UnityEngine.Vector3> vectors3 = new List<UnityEngine.Vector3>();
        List<UnityEngine.Vector3> vectors4 = new List<UnityEngine.Vector3>();
        vectors = new List<List<UnityEngine.Vector3>>() { vectors1, vectors2, vectors3, vectors4 };

        List<BigInteger> times1 = new List<BigInteger>();
        List<BigInteger> times2 = new List<BigInteger>();
        List<BigInteger> times3 = new List<BigInteger>();
        List<BigInteger> times4 = new List<BigInteger>();
        times = new List<List<BigInteger>>() { times1, times2, times3, times4 };

        objects = new List<GameObject> { obj1, obj2, obj3, obj4 };
        trackers = new List<int> { 0, 0, 0, 0 };
        thrusts = new List<Boolean>();
        files = new List<TextAsset> { file1, file2, file3, file4 };
        trails = new List<TrailRenderer> { trail1, trail2, trail3, trail4 };


        initialize();
        thrustChecker();
        normalizeTime();

        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        newTime += Time.deltaTime;
        //Debug.Log(newTime);
        if (newTime > oldTime + 0.01)
        {
            for (BigInteger i = prevCycleEnd; i < prevCycleEnd + cycles; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (trackers[j] < vectors[j].Count)
                    {
                        if (times[j][trackers[j]] == i)
                        {

                            trails[j].AddPosition(vectors[j][trackers[j]]);
                            objects[j].transform.position = vectors[j][trackers[j]];
                            trails[j].transform.position = vectors[j][trackers[j]];


                            if (j==0 && trackers[j] < thrusts.Count-1)
                            {

                                trailT.AddPosition(vectors[j][trackers[j]]);
                                trailT.transform.position = vectors[j][trackers[j]];
                                if (thrusts[trackers[j]])
                                {
                                    trailT.startColor = new Color(1, 0, 0, 1);
                                    trailT.endColor = new Color(1, 0, 0, 1);
                                }
                                else
                                {
                                    trailT.startColor = new Color(1, 0, 0, 0);
                                    trailT.endColor = new Color(1, 0, 0, 0);
                                }
                            }

                            
                            trackers[j]++;
                            if(trackers[0]+1 > vectors[0].Count)
                            {
                                obj1.transform.localScale = new UnityEngine.Vector3(0, 0, 0);
                            }
                        }
                    }
                }
            }
            prevCycleEnd += cycles;
            oldTime = newTime;
        }
        
    }

    void initialize()
    {
        for (int i = 0; i < 4; i++)
        {
            string[] csvLines = files[i].text.Split('\n');
            for (int j = 1; j < csvLines.Length; j++)
            {
                string[] lineData = csvLines[j].Split(',');
                vectors[i].Add(new UnityEngine.Vector3(float.Parse(lineData[0])/10 - 10, float.Parse(lineData[2]) / 10 + 20, float.Parse(lineData[1]) / 10 + 20)); 
            }
        }
    }

    void thrustChecker()
    {
        for (int i = 0; i < 4; i++)
        {
            string[] csvLines = files[i].text.Split('\n');
            for (int j = 1; j < csvLines.Length; j++)
            {
                string[] lineData = csvLines[j].Split(',');
                if (i == 0)
                {
                    if (lineData[4].Length > 5)
                    {
                        thrusts.Add(false);
                    }
                    else
                    {
                        thrusts.Add(true);
                    }
                    
                }
            }
        }
    }

    void normalizeTime()
    {
        for (int i = 0; i < 4; i++)
        {
            string[] csvLines = files[i].text.Split('\n');
            for (int j = 1; j < csvLines.Length; j++)
            {
                string[] lineData = csvLines[j].Split(',');

                if (lineData[3].Contains("."))
                {
                    times[i].Add(BigInteger.Parse(lineData[3].Replace(".", "")) - minTime);
                }
                else
                {
                    times[i].Add(BigInteger.Parse(lineData[3]) * 10 - minTime);
                }
            }
        }
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < times[i].Count - 2; j++)
            {
                if (times[i][j] == times[i][j + 1])
                {
                    times[i].RemoveAt(j + 1);
                    vectors[i].RemoveAt(j + 1);
                    thrusts.RemoveAt(j + 1);
                    j--;
                }
            }
        }
    }
}
