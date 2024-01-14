using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCreator : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 10;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    multiOctaveNoise noise = new multiOctaveNoise();

    private float[] vertexTemperatures;

    //---- PLANET SETTINGS ----///
    public planetSettings settings;
    public BiomeGenerator biomeGenerator;

    public Material planetMaterial;

    public MinMaxCalc elevationMinMax;

//TEXTURE GRADIENT VARIABLES
    public Gradient elevationGradient = new Gradient();

    public Gradient biomeGradient = new Gradient();

    Texture2D texture;
    Texture2D blueTexture;

    Texture2D biomeTexture;

    

    const int textureResolution = 50;

    public void updateElevation(MinMaxCalc _elevationMinMax){
        planetMaterial.SetVector("_elevationMinMax", new Vector4(_elevationMinMax.Min, _elevationMinMax.Max));
    }

    public void updateColours(){
        Color[] Colours = new Color[textureResolution];
        Color[] blueColours = new Color[textureResolution];
        //Color[] biomeColors = biomeGenerator.generateBiome();
        Color[] biomeColors = new Color[textureResolution];


        for(int i = 0;  i<textureResolution; i++){
            Colours[i] = elevationGradient.Evaluate(i/(textureResolution - 1f));
            biomeColors[i] = biomeGradient.Evaluate(i/(textureResolution - 1f));
            blueColours[i] = new Color(0f,0f,0.5f);
        }
        texture.SetPixels(Colours);
        texture.Apply();
        planetMaterial.SetTexture("_texture", texture);  

        biomeTexture.SetPixels(biomeColors);
        biomeTexture.Apply();
        planetMaterial.SetTexture("_biomeTexture", biomeTexture);

        blueTexture.SetPixels(blueColours);
        blueTexture.Apply();
        planetMaterial.SetTexture("_blueTexture", blueTexture);


    }

    void Start(){
        
        Initialize();
        GenerateMesh();
        
    }


/*     void OnValidate()
    {
        Initialize();
        GenerateMesh();
    }  */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Generating new planet!");
            updateSphere();
        }
        transform.Rotate(Vector3.up, Time.deltaTime * 3f);
 
        
    }
    void Initialize()
    {
        ///--- Generate random seed ---///
        Vector3 seed = randomSeed();

        elevationMinMax = new MinMaxCalc();
        
        texture = new Texture2D(textureResolution, 1);

        blueTexture = new Texture2D(textureResolution, 1);

        biomeTexture = new Texture2D(textureResolution, 1);
        
        updateColours();

        biomeGenerator.clearTemperatures();
        biomeGenerator.clearRain();

        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];

        }
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObject = new GameObject("mesh");
                meshObject.transform.parent = transform;

                meshObject.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObject.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();

            }
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = planetMaterial;


            terrainFaces[i] = new TerrainFace(settings, biomeGenerator, meshFilters[i].sharedMesh, resolution, directions[i], noise, seed, elevationMinMax);
        }
    }

    void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
        updateElevation(elevationMinMax);
    }

    /* void GenerateColours(){
        updateColours();
    } */



    public void updateSphere()
    {

        Initialize();
        GenerateMesh();
    }
    private Vector3 randomSeed()
    {
        return new Vector3(Random.Range(0, 5f), Random.Range(0, 5f), Random.Range(0, 5f));
    }

}
