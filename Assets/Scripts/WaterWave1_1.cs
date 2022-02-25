using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWave1_1 : MonoBehaviour
{
    public static WaterWave1_1 instance;

    public SpriteRenderer WaterWaveSecondBottle;

    void Awake()
    {
        instance = GetComponent<WaterWave1_1>();
        WaterWaveSecondBottle = GetComponent<SpriteRenderer>();
    }
}
