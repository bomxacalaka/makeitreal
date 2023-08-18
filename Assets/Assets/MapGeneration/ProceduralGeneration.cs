using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] int width, height;
    [SerializeField] Sprite dirtSprite, grassSprite, rockSprite, treeSprite;

    [Header("Noise Settings")]
    [SerializeField] float scale = 0.1f;
    [SerializeField] float heightMultiplier = 5;
    [SerializeField] float secondaryScale = 0.05f; // New secondary noise scale
    [SerializeField] float secondaryWeight = 0.5f; // Weighting for secondary noise

    private float xOffset, yOffset;

    void Start()
    {
        GenerateTerrain();
    }


    public void GenerateTerrain()
    {
        ClearTerrain();

        xOffset = Random.Range(0, 9999);
        yOffset = Random.Range(0, 9999);
        for (int x = 0; x < width; x++)
        {
            // Combining two layers of Perlin noise
            float primaryNoise = Mathf.PerlinNoise(x * scale + xOffset, yOffset);
            float secondaryNoise = Mathf.PerlinNoise(x * secondaryScale + xOffset, yOffset * secondaryScale);

            float combinedHeight = (primaryNoise + secondaryNoise * secondaryWeight) / (1 + secondaryWeight);
            int terrainHeight = Mathf.FloorToInt(combinedHeight * heightMultiplier);

            for (int y = 0; y < height; y++)
            {
                Vector2 spawnPosition = new Vector2(x, y);

                if (y <= terrainHeight)
                {
                    SpawnSprite(dirtSprite, spawnPosition);

                    // Place rocks or trees based on secondary noise
                    if (y == terrainHeight)
                    {
                        if (secondaryNoise > 0.75f) SpawnSprite(rockSprite, spawnPosition + Vector2.up);
                        else if (secondaryNoise < 0.25f) SpawnSprite(treeSprite, spawnPosition + Vector2.up);
                    }
                }
                else if (y == terrainHeight + 1)
                {
                    SpawnSprite(grassSprite, spawnPosition);
                }
            }
        }
    }

    void SpawnSprite(Sprite sprite, Vector2 position)
    {
        GameObject newObj = new GameObject("SpriteObject");
        newObj.transform.position = position;
        newObj.transform.parent = transform;

        SpriteRenderer spriteRenderer = newObj.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
    }

    // New method to clear existing terrain
    public void ClearTerrain()
    {
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }
    }
}
