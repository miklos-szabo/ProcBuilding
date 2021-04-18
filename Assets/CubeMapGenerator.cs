using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CubeMapGenerator : MonoBehaviour
{
    private Camera camera;
    
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            var cubeMap = new Cubemap(512, TextureFormat.ARGB32, false);
            camera.RenderToCubemap(cubeMap);
            AssetDatabase.CreateAsset(cubeMap, $"Assets/CubeMaps/{camera.name}.cubemap");
        }
    }
}
