using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    int[] triangles;
    private List<Polygon> polygon = new List<Polygon>();
    private List<Vector3> vertices = new List<Vector3>();
    private Color[] temperatures;
    public Gradient temperaturGradient;
    float minTerrainHeight;
    float maxTerrainHeight;
    private float[] vertexTemperatures;



    [Range(1.0f, 100.0f)]
    [SerializeField]
    float radius = 1.0f;
    float lastRadius;


    [Range(0, 10)]
    [SerializeField]
    int subdivisions = 0;
    int lastSubdivisions;

    [Range(0f, 10)]
    [SerializeField]
    float amplidtude = 5;
    float lastAmplitude;

    [Range(0f, 10)]
    [SerializeField]
    float frequency = 0.2f;
    float lastfrequency;

    [Range(0f, 5)]
    [SerializeField]
    float presistence = 0.5f;
    float lastPresitence;

    [Range(0f, 5)]
    [SerializeField]
    float lacunarity = 2.0f;
    float lastLacunarity;

    [SerializeField]
    Vector3 noiseCenter;
    Vector3 lastNoiseCenter;
    [SerializeField]
    bool rotate = false;
    public multiOctaveNoise noise = new multiOctaveNoise();

    [SerializeField]
    private BiomeGenerator biomeGenerator;

    private SphereGenerator sphereGenerator;
    void Start()
    {
        mesh = new Mesh();
        //biomeGenerator = new BiomeGenerator(temperaturGradient);
        GetComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<BiomeGenerator>();
        sphereGenerator = GetComponent<SphereGenerator>();


        lastRadius = radius;
        lastSubdivisions = subdivisions;
        sphereGenerator.createShape();
        mesh.vertices = sphereGenerator.vertices.ToArray();
        mesh.triangles = sphereGenerator.triangles;
        //mesh.colors = temperatures;
        mesh.RecalculateNormals();
        // createSphere();
        //subdivideSphere(subdivisions);
        //updateMesh();

    }
    /* void Update()
    {
        if (subdivisions != lastSubdivisions)
        {
            subdivideSphere(subdivisions);
            updateMesh();
            lastSubdivisions = subdivisions;
        }
        if (radius != lastRadius)
        {
            updateMesh();
            lastRadius = radius;
        }
        if (amplidtude != lastAmplitude)
        {
            updateMesh();
            lastAmplitude = amplidtude;
        }

        if (noiseCenter != lastNoiseCenter)
        {
            updateMesh();
            lastNoiseCenter = noiseCenter;
        }

        if (frequency != lastfrequency)
        {
            updateMesh();
            lastfrequency = frequency;
        }
        if (lacunarity != lastLacunarity)
        {
            updateMesh();
            lastLacunarity = lacunarity;
        }
        if (presistence != lastPresitence)
        {
            updateMesh();
            lastPresitence = presistence;
        }
        if (rotate)
        {
            float rotationSpeed = 15.0f;
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }


    } */

    void createSphere()
    {
        float phi = (1.0f + Mathf.Sqrt(5.0f)) * 0.5f;
        float a = 1.0f;
        float b = 1.0f / phi;


        vertices.Add(new Vector3(0, b, -a).normalized * radius);
        vertices.Add(new Vector3(b, a, 0).normalized * radius);
        vertices.Add(new Vector3(-b, a, 0).normalized * radius);
        vertices.Add(new Vector3(0, b, a).normalized * radius);
        vertices.Add(new Vector3(0, -b, a).normalized * radius);
        vertices.Add(new Vector3(-a, 0, b).normalized * radius);
        vertices.Add(new Vector3(0, -b, -a).normalized * radius);
        vertices.Add(new Vector3(a, 0, -b).normalized * radius);
        vertices.Add(new Vector3(a, 0, b).normalized * radius);
        vertices.Add(new Vector3(-a, 0, -b).normalized * radius);
        vertices.Add(new Vector3(b, -a, 0).normalized * radius);
        vertices.Add(new Vector3(-b, -a, 0).normalized * radius);

        polygon.Add(new Polygon(2, 1, 0));
        polygon.Add(new Polygon(1, 2, 3));
        polygon.Add(new Polygon(5, 4, 3));
        polygon.Add(new Polygon(4, 8, 3));
        polygon.Add(new Polygon(7, 6, 0));
        polygon.Add(new Polygon(6, 9, 0));
        polygon.Add(new Polygon(11, 10, 4));
        polygon.Add(new Polygon(10, 11, 6));
        polygon.Add(new Polygon(9, 5, 2));
        polygon.Add(new Polygon(5, 9, 11));
        polygon.Add(new Polygon(8, 7, 1));
        polygon.Add(new Polygon(7, 8, 10));
        polygon.Add(new Polygon(2, 5, 3));
        polygon.Add(new Polygon(8, 1, 3));
        polygon.Add(new Polygon(9, 2, 0));
        polygon.Add(new Polygon(1, 7, 0));
        polygon.Add(new Polygon(11, 9, 6));
        polygon.Add(new Polygon(7, 10, 6));
        polygon.Add(new Polygon(5, 11, 4));
        polygon.Add(new Polygon(10, 8, 4));



        //Adds triangles from the list of polygon - Turns List<Polygon> -> int[]
        updateTriangle();
    }
    void updateTriangle()
    {
        triangles = new int[polygon.Count * 3];
        int triangleCount = 0;
        foreach (Polygon pol in polygon)
        {
            for (int i = 0; i < pol.triangle.Count; i++)
            {
                triangles[triangleCount * 3 + i] = pol.triangle[i];
            }
            triangleCount++;
        }
    }

    void updateMesh()
    {
        mesh.Clear();
        biomeGenerator.clearTemperatures();
        biomeGenerator.clearRain();
        vertices.Clear();
        polygon.Clear();
        createSphere();
        subdivideSphere(subdivisions);
        //updateTriangle();


        //Debug.Log(vertices.Count);
        float perlinNoise;
        temperatures = new Color[vertices.Count];
        vertexTemperatures = new float[vertices.Count];

        for (int i = 0; i < vertices.Count; i++)
        {
            perlinNoise = noise.octaveNoise(vertices[i].normalized, amplidtude, frequency, lacunarity, presistence, 10, noiseCenter);
            vertices[i] = vertices[i].normalized * radius + Vector3.Scale(new Vector3(perlinNoise, perlinNoise, perlinNoise), vertices[i].normalized);



            // --- Calculate the distance from the equator to get the temperature --- // 
            Vector3 yVector = new Vector3(0, vertices[i].y, 0);
            Vector3 equator = new Vector3(0, 0, 0);
            float temperature = -Vector3.Distance(yVector, equator);
            biomeGenerator.addVertexTemperature(temperature);
            //biomeGenerator.generateRain(vertices[i].normalized);
        }
        temperatures = biomeGenerator.getAllTemperatures();
        temperatures = biomeGenerator.getAllRain();
        temperatures = biomeGenerator.generateBiome();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;
        mesh.colors = temperatures;
        mesh.RecalculateNormals();
    }


    void subdivideSphere(int subdivisions)
    {
        var midPointCache = new Dictionary<int, int>();
        List<cacheMemory> indexCache = new List<cacheMemory>();

        for (int i = 0; i < subdivisions; i++)
        {
            List<Polygon> newPolygons = new List<Polygon>();
            foreach (Polygon poly in polygon)
            {
                int a = poly.triangle[0];
                int b = poly.triangle[1];
                int c = poly.triangle[2];

                int ab = getMidpointIndex(indexCache, a, b);
                int bc = getMidpointIndex(indexCache, b, c);
                int ca = getMidpointIndex(indexCache, c, a);

                newPolygons.Add(new Polygon(a, ab, ca));
                newPolygons.Add(new Polygon(ab, b, bc));
                newPolygons.Add(new Polygon(ab, bc, ca));
                newPolygons.Add(new Polygon(ca, bc, c));
            }
            polygon = newPolygons;
        }
        updateTriangle();

    }

    public int getMidpointIndex(List<cacheMemory> cache, int indexA, int indexB)
    {
        int smallerIndex = Mathf.Min(indexA, indexB);
        int biggerIndex = Mathf.Max(indexA, indexB);
        int key = (smallerIndex << 16) + biggerIndex;

        int ret;

        //if (cache.TryGetValue(key, out ret))
        // return ret;
        foreach (cacheMemory c in cache)
        {
            if (c.smallerIndex == smallerIndex && c.biggerIndex == biggerIndex)
            {
                //Debug.Log("same");

                return c.index;
            }
        }

        Vector3 p1 = vertices[indexA];
        Vector3 p2 = vertices[indexB];
        Vector3 middle = Vector3.Lerp(p1, p2, 0.5f).normalized;
        middle = middle.normalized * radius;

        ret = vertices.Count;
        cacheMemory memory = new cacheMemory(ret, smallerIndex, biggerIndex);

        vertices.Add(middle);
        cache.Add(memory);
        return ret;
    }


}
