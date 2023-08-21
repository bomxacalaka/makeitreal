using UnityEngine;
using UnityEditor; // This is needed for the [CustomEditor] attribute

[CustomEditor(typeof(ProceduralGeneration))]
public class GeneralEditor : Editor
{
    int mainOffset = 0;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ProceduralGeneration generationScript = (ProceduralGeneration)target;

        if (GUILayout.Button("Generate Terrain"))
        {
            generationScript.ClearTerrain();
            generationScript.GenerateTerrain();
        }
        if (GUILayout.Button("Clear Terrain"))
        {
            generationScript.ClearTerrain();
        }
        if (GUILayout.Button("Add to mainOffset"))
        {
            mainOffset += 10;
            generationScript.ClearTerrain();
            generationScript.GenerateTerrain(mainOffset);
        }
    }
}
