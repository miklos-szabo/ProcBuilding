using System.Linq;
using UnityEngine;

namespace ProceduralToolkit.Samples.Buildings
{
    public enum BuildingFacadeType
    {
        MissingSomeWindows,        // 20% chance of each window not appearing
        EmptyMiddleVertical,       // The middle column of windows doesn't appear
        EmptyThickMiddleVertical,  // The 3 middle columns of windows don't appear
        LeftSideEmpty,             // Windows don't appear on the left side
        RightSideEmpty,            // Windows don't appear on the right side
        HorizontalLineMiddle,      // Middle row of windows doesn't appear
        HorizontalThickLineMiddle, // Middle 3 rows of windows don't appear
        Cross,                     // Middle row and column doesn't appear
        ThickCross,                // 3 middle rows and columns don't appear
        MiddleColumnFullGlass,     // Middle column is full glass
        SWithContGlass,            // An S shape with continuous windows
    }

    public class CurrentBuilding
    {
        public static BuildingFacadeType FacadeType = BuildingFacadeType.MissingSomeWindows;
        public static string GroundFloorType;

        private static float[] weights = {
            2f,
            2f,
            1f,
            1f,
            1f,
            1f,
            1f,
            2f,
            1f,
            2f,
            1f
        };

        public static void SetRandomFacadeType()
        {
            float total = weights.Sum();

            float randomPoint = Random.value * total;
            
            for (int i= 0; i < weights.Length; i++)
            {
                if (randomPoint < weights[i]) {
                    FacadeType = (BuildingFacadeType)i;
                    return;
                }

                randomPoint -= weights[i];
            }
            
            FacadeType = (BuildingFacadeType)weights.Length - 1 ;
        }

        public static void SetRandomGroundFloorType()
        {
            GroundFloorType = "GroundFloorThing-" + Random.Range(0, 3);    // Itt kell átállítani, ha többféle kirakatot szeretnénk
        }
    }
}