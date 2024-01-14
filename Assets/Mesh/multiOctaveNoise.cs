using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class multiOctaveNoise
{
    Noise noise = new Noise();
    NoiseSetting settings = new NoiseSetting();
    public float octaveNoise(Vector3 point, float amplitude, float frequency, float lacunarity, float presistence, int octave, Vector3 center)
    {

        float noiseValue = 0.0f;
        for (int i = 0; i < octave; i++)
        {
            float v = noise.Evaluate(point * frequency + center);
            noiseValue += v * amplitude;
            amplitude *= presistence;
            frequency *= lacunarity;

        }
        //ADDS A SMOOTH THING AT MIN VAL 
        //noiseValue = Mathf.Max(0, noiseValue - 1);
        return noiseValue;
    }
}
