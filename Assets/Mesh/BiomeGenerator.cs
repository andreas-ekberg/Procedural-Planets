using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeGenerator : MonoBehaviour
{

    int face = 1;
    private float lowestTemperature = 0;

    [SerializeField]
    private float amplidtude = 0;
    private List<float> vertexTemperatures = new List<float>();

    private List<float> vertexRain = new List<float>();
    private multiOctaveNoise noise = new multiOctaveNoise();

    public Gradient temperatureGradient = new Gradient();

    public PlanetCreator planetCreator;



    


    public void clearRain()
    {
        vertexRain.Clear();
    }
    public float generateRain(Vector3 p, Vector3 seed)
    {
        Vector3 center = seed;
        float noiseValue = noise.octaveNoise(p, 1f, 1f, 0.3f, 0.3f, 10, center);
        return noiseValue;

    }

    public Color[] getAllRain()
    {
        Color[] allRain = new Color[vertexRain.Count];
        for (int i = 0; i < vertexRain.Count; i++)
        {
            if (vertexRain[i] > 0.8)
            {
                allRain[i] = new Color(0, 0, 1);
            }
            else if (vertexRain[i] > 0.4)
            {
                allRain[i] = new Color(0, 1f, 0);
            }
            else
            {
                allRain[i] = new Color(1, 1, 1);
            }
            //allRain[i] = temperatureGradient.Evaluate(vertexRain[i]);
        }
        return allRain;
    }

    public void clearTemperatures()
    {
        vertexTemperatures.Clear();
    }
    public void addVertexTemperature(float temperature)
    {
        vertexTemperatures.Add(temperature);
        if (temperature < lowestTemperature)
        {
            lowestTemperature = temperature;
        }
    }

    public Color[] getAllTemperatures()
    {
        //Debug.Log(lowestTemperature);
        Color[] allTemperatures = new Color[vertexTemperatures.Count];
        for (int i = 0; i < vertexTemperatures.Count; i++)
        {
            float temperature = Mathf.InverseLerp(lowestTemperature, 0, vertexTemperatures[i]);
            allTemperatures[i] = temperatureGradient.Evaluate(temperature);
        }
        return allTemperatures;
    }

    public Color[] generateBiome()
    {
        Color[] biomeColor = new Color[vertexTemperatures.Count];
        for (int i = 0; i < vertexTemperatures.Count; i++)
        {
            biomeColor[i] = decideBiome(vertexRain[i], vertexTemperatures[i]);
        }
        return biomeColor;
    }

    public int getSizeOfBiome(){

        return vertexTemperatures.Count;
    }

    public Color decideBiome(float rain, float temperature)
    {

        if (temperature < -12)
        {
            return new Color(0.82f, 0.92f, 1f);
            //return planetMaterial.SetFloat("_biomeTexture", biomeTexture);
        }
        else if (temperature < -5 && rain < 0.5f)
        {
            return new Color(0.651f, 0.53f, 0.2f);
        }
        else if (temperature < -5 && rain > 0.5f)
        {
            return new Color(0.23f, 0.65f, 0.13f);
        }
        else if (temperature > -5 && rain > 0.5f)
        {
            return new Color(0.9f, 0.81f, 0.53f);
        }
        else
        {
            return new Color(0.19f, 0.369f, 0.255f);
        }

    }

    public Color decideBiomeGradient(float rain, float temperature){

        rain *= 30;
        temperature = -temperature;
        temperature /= 14;
        temperature *= 100;

        

        return planetCreator.biomeGradient.Evaluate((rain+temperature)/130);

    }
    

}
