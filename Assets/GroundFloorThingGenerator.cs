using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFloorThingGenerator : MonoBehaviour
{

    [SerializeField] private float GroundFloorThingWidth = 2f;
    [SerializeField] private List<GameObject> groundFloorPrefabs = new List<GameObject>();

    private GameObject parent;

    private readonly List<Vector2> vectorList = new List<Vector2>
    {
        new Vector2(0, 0),
        new Vector2(20, 0),
        new Vector2(20, 20),
        new Vector2(0, 20),
    };
    
    // Start is called before the first frame update
    void Start()
    {
        parent = new GameObject("GroundFloorThings");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            GenerateGroundFloorThings(vectorList);
        }
    }

    public void GenerateGroundFloorThings(List<Vector2> vectors)
    {
        for(int i = 0; i < vectors.Count; i++)
        {
            var nextVector = i + 1 >= vectors.Count ? 0 : i + 1;
            CreateGroundThingsBetween(new Vector3(vectors[i].x, 0, vectors[i].y), new Vector3(vectors[nextVector].x, 0, vectors[nextVector].y));
        }
    }

    private void CreateGroundThingsBetween(Vector3 v1, Vector3 v2)
    {
        var baseVector = v2 - v1;
        var thingCoords = CreateGroundFloor(baseVector, v1);
        var facing = Vector3.Cross(baseVector, Vector3.up);

        foreach (var coord in thingCoords)
        {
            Instantiate(groundFloorPrefabs[Random.Range(0, groundFloorPrefabs.Count)], coord + (facing.normalized * 0.001f), Quaternion.LookRotation(facing), parent.transform);
        }
    }
    
    private List<Vector3> CreateGroundFloor(Vector3 baseVector, Vector3 startVector)
    {
        var doorStartWidth = baseVector.magnitude / 2 - 2;
        var numberOfThings = (int)(doorStartWidth / (GroundFloorThingWidth + 1.5));    //thing, .75+.75mes sides
        var separatorWidth = (doorStartWidth - numberOfThings * GroundFloorThingWidth) / (numberOfThings + 1);
        var normalizedBase = baseVector.normalized;

        var thingCoords = new List<Vector3>();
        var currentPos = 0f;

        for (int i = 0; i < numberOfThings; i++)    //Before door
        {
            currentPos += separatorWidth;
            thingCoords.Add(startVector + normalizedBase * currentPos + new Vector3(0, 1.5f, 0));
            currentPos += GroundFloorThingWidth;
        }

        currentPos += separatorWidth;  //Last Separator

        currentPos += 4;   //Door - doorwidth = 2 -> 4 space with door separators
            
        for (int i = 0; i < numberOfThings; i++)    //After door
        {
            currentPos += separatorWidth;
            thingCoords.Add(startVector + normalizedBase * currentPos + new Vector3(0, 1.5f, 0));
            currentPos += GroundFloorThingWidth;
        }

        currentPos += separatorWidth;  //Last Separator
        return thingCoords;
    }
}
