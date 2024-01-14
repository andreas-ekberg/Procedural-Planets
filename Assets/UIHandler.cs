using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    PlanetCreator planetCreator;
    planetSettings planetSettings;

    public TextMeshProUGUI amplitudeText;

    public TextMeshProUGUI frequencyText;
    public TextMeshProUGUI lacunarityText;
    public TextMeshProUGUI presistenceText;
    public TextMeshProUGUI octavesText;

    [SerializeField] UnityEngine.UI.Slider amplitudeSlider;

    [SerializeField] UnityEngine.UI.Slider frequencySlider;

    [SerializeField] UnityEngine.UI.Slider lacunaritySlider;

    [SerializeField] UnityEngine.UI.Slider presistenceSlider;

    [SerializeField] UnityEngine.UI.Slider octavesSlider; 

    void Start(){
        planetCreator = GameObject.Find("Mesh Generator").GetComponent<PlanetCreator>();
        planetSettings = GameObject.Find("Mesh Generator").GetComponent<planetSettings>();   
        
        /* frequencySlider
        lacunaritySlider
        presistanceSlider */

        amplitudeSlider.onValueChanged.AddListener((v) => {
            amplitudeText.text = v.ToString("0.0");
        });

        frequencySlider.onValueChanged.AddListener((v) => {
            frequencyText.text = v.ToString("0.0");
        });

        lacunaritySlider.onValueChanged.AddListener((v) => {
            lacunarityText.text = v.ToString("0.0");
        });

        presistenceSlider.onValueChanged.AddListener((v) => {
            presistenceText.text = v.ToString("0.0");
        });

        octavesSlider.onValueChanged.AddListener((v) => {
            octavesText.text = v.ToString("0");
        });
    }

    //GameObject.Find("amplitudeSlider").GetComponent<UnityEngine.UI.Slider>()

    public void GenerateNewPlanet(){

        planetSettings.updateAmplitude(amplitudeSlider.value);
        planetSettings.updateFrequency(frequencySlider.value);
        planetSettings.updateLacunarity(lacunaritySlider.value);
        planetSettings.updatePresistence(presistenceSlider.value);
        planetSettings.updateOctaves(Mathf.RoundToInt(octavesSlider.value));
        planetCreator.updateSphere();
    }

    
}
