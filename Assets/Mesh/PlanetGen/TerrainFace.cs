using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TerrainFace
{
    Mesh mesh;
    int resolution;
    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;

    multiOctaveNoise noise;
    planetSettings settings;

    Vector3 seed;
    BiomeGenerator biomeGenerator;
    private List<float> vertexTemperatures = new List<float>();

    private List<float> vertexRain = new List<float>();

    public MinMaxCalc elevationMinMax;



    public TerrainFace(planetSettings settings, BiomeGenerator biomeGenerator, Mesh mesh, int resolution, Vector3 localUp, multiOctaveNoise noise, Vector3 seed, MinMaxCalc elevationMinMax)
    {
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;
        this.noise = noise;
        this.settings = settings;
        this.biomeGenerator = biomeGenerator;
        this.seed = seed;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
        this.elevationMinMax = elevationMinMax;
    }

    public void ConstructMesh()
    {



        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;
        Color[] biomeColor = new Color[resolution * resolution];

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int index = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;



                float noiseValue = noise.octaveNoise(pointOnUnitSphere.normalized, settings.amplitude, settings.frequency, settings.lacunarity, settings.presistence, settings.octaves, seed);
                //noiseValue = 0;
                vertices[index] = pointOnUnitSphere.normalized * settings.radius + Vector3.Scale(new Vector3(noiseValue, noiseValue, noiseValue), pointOnUnitSphere.normalized);
                elevationMinMax.AddValue((pointOnUnitSphere.normalized * settings.radius + Vector3.Scale(new Vector3(noiseValue, noiseValue, noiseValue), pointOnUnitSphere.normalized)).magnitude);
                ///--- BIOME ---///

                Vector3 yVector = new Vector3(0, vertices[index].y, 0);
                Vector3 equator = new Vector3(0, 0, 0);
                float temperature = -Vector3.Distance(yVector, equator) + noise.octaveNoise(pointOnUnitSphere.normalized, settings.amplitude, settings.frequency, settings.lacunarity, settings.presistence, settings.octaves, new Vector3(0, 0, 0));
                vertexTemperatures.Add(temperature);
                vertexRain.Add(biomeGenerator.generateRain(vertices[index].normalized, seed));

                biomeColor[index] = biomeGenerator.decideBiomeGradient(vertexRain[index], vertexTemperatures[index]);




                if (x != resolution - 1 && y != resolution - 1)
                {
                    // First triangle
                    triangles[triIndex] = index;
                    triangles[triIndex + 1] = index + resolution + 1;
                    triangles[triIndex + 2] = index + resolution;

                    // Second triangle
                    triangles[triIndex + 3] = index;
                    triangles[triIndex + 4] = index + 1;
                    triangles[triIndex + 5] = index + resolution + 1;

                    triIndex += 6;
                }
            }
        }





        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = biomeColor;
        mesh.RecalculateNormals();
    }




}
