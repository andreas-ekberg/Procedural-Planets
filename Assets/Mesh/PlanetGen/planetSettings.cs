using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planetSettings : MonoBehaviour
{
    public float radius = 1f;
    public float amplitude = 0.8f;
    public float frequency = 1.3f;
    public float lacunarity = 2f;
    public float presistence = 0.5f;
    public int octaves = 10;

    public void updateAmplitude(float _amplitude){
        amplitude = _amplitude;
    }

    public void updateFrequency(float _frequency){
        frequency = _frequency;
    }

    public void updateLacunarity(float _lacunarity){
        lacunarity = _lacunarity;
    }

    public void updatePresistence(float _presistence){
        presistence = _presistence;
    }

    public void updateOctaves(int _octaves){
        octaves = _octaves;
    }


}
