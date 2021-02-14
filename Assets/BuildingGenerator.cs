using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.ProBuilder;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

public class BuildingGenerator : MonoBehaviour
{
    [SerializeField] private Vector3 genCoords = Vector3.zero;
    [SerializeField] private float height = 30f;
    [SerializeField] private float width = 20f;
    [SerializeField] private Material wallMaterial;
    [SerializeField] private Material windowMaterial;
    [SerializeField] private Material doorMaterial;
    [SerializeField] private float windowHeight = 2f;
    [SerializeField] private float windowWidth = 2f;
    [SerializeField] [Tooltip("Két ablak közötti távolság fele vízszintesen")] private float windowWidthSpacing = 1.5f;
    [SerializeField] [Tooltip("Két ablak közötti távolság fele függőlegesen")] private float windowHeightSpacing = .5f;
    [SerializeField] private float doorSectionHeight = 4f;

    // Start is called before the first frame update
    void Start()
    {
        GenerateBuilding();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GenerateBuilding();
    }

    private void GenerateBuilding()
    {
        var mainVertices = new Vector3[]
        {
            new Vector3(0f, 0f, 0f),
            new Vector3(width, 0f, 0f),
            new Vector3(0f, height, 0f),
            new Vector3(width, height, 0f)
        };
        var mainFace = new Face(new int[] {1, 0, 3, 0, 2, 3});
        var faces = new List<Face>{mainFace};

        var windowFaces = CreateWindows(ref mainVertices);
        faces.AddRange(windowFaces);
        
        var mesh = ProBuilderMesh.Create(mainVertices, faces);
        
        mesh.GetComponent<MeshRenderer>().sharedMaterials = new[] {wallMaterial, windowMaterial};
        mainFace.submeshIndex = 0;
        
        var uvs = new List<Vector4>();
        mesh.GetUVs(0, uvs);

        //Set UV for main face
        //mainFace.manualUV = true;
        //var mainFaceIndexes = mainFace.distinctIndexes;
        // uvs[mainFaceIndexes[0]] = new Vector4(1.18f, 0f);
        // uvs[mainFaceIndexes[1]] = new Vector4(0.82f, 0f);
        // uvs[mainFaceIndexes[2]] = new Vector4(1.18f, 1f);
        // uvs[mainFaceIndexes[3]] = new Vector4(0.82f, 1f);
        mainFace.uv = new AutoUnwrapSettings
        {
            scale = new Vector2(15f, 15f)
        };

        //Set UV for all window faces
        foreach (var face in windowFaces)
        {
            face.submeshIndex = 1;    //Window material
            face.manualUV = true;
        
            //Set UV coords
            var wIndexes = face.distinctIndexes;
            uvs[wIndexes[0]] = new Vector4(0.78f, 0.18f);
            uvs[wIndexes[1]] = new Vector4(0.22f, 0.18f);
            uvs[wIndexes[2]] = new Vector4(0.78f, 0.82f);
            uvs[wIndexes[3]] = new Vector4(0.22f, 0.82f);
        }
        mesh.SetUVs(0, uvs);
        
        //Materials overlap, extrude the windows a bit
        mesh.Extrude(windowFaces, ExtrudeMethod.FaceNormal, 0.01f);
        
        mesh.ToMesh();
        mesh.Refresh();
    }

    /// <summary>
    /// Creates the windows of the facade
    /// </summary>
    /// <param name="vertices">The array containing the mesh's vertices, ref type</param>
    /// <returns></returns>
    private List<Face> CreateWindows(ref Vector3[] vertices)
    {
        var createdFaces = new List<Face>();
        
        //Get the height parts of all the lower-left coordinates, goes from the height of the door section to the top
        for (float heightLowerStart = doorSectionHeight; heightLowerStart + windowHeight < height; heightLowerStart += windowHeight + windowHeightSpacing * 2)
        {
            //Get the width parts of all the lower-left coordinates, goes from left edge + spacing to the other edge
            for (float widthLeftStart = genCoords.x + windowWidthSpacing * 2; widthLeftStart + windowWidth < width; widthLeftStart += windowWidth + windowWidthSpacing * 2)
            {
                createdFaces.Add(CreateWindowAt(ref vertices, new Vector3(widthLeftStart, heightLowerStart)));
            }
        }

        return createdFaces;
    }

    /// <summary>
    /// Creates a window at the given coordinates, adding the vertices to the array
    /// </summary>
    /// <param name="vertices">Array of the vertices of the whole facade, ref type</param>
    /// <param name="lowerLeft">The lower left coordinates of the window</param>
    /// <returns>The created face, the vertices array is expanded with the new vertices</returns>
    private Face CreateWindowAt(ref Vector3[] vertices, Vector3 lowerLeft)
    {
        var vertCount = vertices.Length;
        var newVertices = new Vector3[]
        {
            lowerLeft,
            lowerLeft + new Vector3(windowWidth, 0, 0),
            lowerLeft + new Vector3(0, windowHeight, 0),
            lowerLeft + new Vector3(windowWidth, windowHeight, 0)
        };
        vertices = vertices.Concat(newVertices).ToArray();
        return new Face(new int[]{vertCount + 1, vertCount, vertCount + 3, vertCount, vertCount + 2, vertCount + 3});
    }
}
