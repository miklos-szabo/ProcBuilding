using System;
using System.Collections;
using System.Collections.Generic;
using ProceduralToolkit.Samples.Buildings;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainBuildingGen : MonoBehaviour
{
    [SerializeField] private BuildingGeneratorComponent _generator;
    public static BuildingFacadeType BuildingFacadeType = BuildingFacadeType.MissingSomeWindows;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var existingBuilding = GameObject.Find("Building");
            Destroy(existingBuilding);
            
            _generator.Generate();
        }
    }
}


