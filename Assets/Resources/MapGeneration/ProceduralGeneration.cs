using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] int width = 1000, height = 50;
    [SerializeField] Sprite grassSprite, dirtSprite, rockSprite, treeSprite;

    [Header("Noise Settings")]
    [SerializeField] float scale = 0.01f;
    [SerializeField] float heightMultiplier = 50;
    [SerializeField] float secondaryScale = 0.02f; // New secondary noise scale
    [SerializeField] float secondaryWeight = 0.5f; // Weighting for secondary noise

    private float xOffset, yOffset;

    void Start()
    {
        GenerateTerrain();
    }


    public void GenerateTerrain()
    {

        // Load sprites from Resources folder
        if (grassSprite == null || dirtSprite == null || rockSprite == null || treeSprite == null)
        {
            grassSprite = Resources.Load<Sprite>("Sprites/grass");
            dirtSprite = Resources.Load<Sprite>("Sprites/dirt");
            rockSprite = Resources.Load<Sprite>("Sprites/rock");
            treeSprite = Resources.Load<Sprite>("Sprites/tree");
        }

        xOffset = Random.Range(0, 9999);
        yOffset = Random.Range(0, 9999);
        for (int x = 0; x < width; x++)
        {
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

                    if (y == terrainHeight && primaryNoise > 0.5f && secondaryNoise > 0.75f)
                    {
                        SpawnSprite(rockSprite, spawnPosition + Vector2.up, 1);

                    }
                }
                else if (y == terrainHeight + 1)
                {
                    SpawnSprite(grassSprite, spawnPosition);
                }
            }
        }

    }

    void SpawnSprite(Sprite sprite, Vector2 position, int sortingOrder = 0)
    {
        GameObject newObj = new GameObject("SpriteObject");
        newObj.transform.position = position;
        newObj.transform.parent = transform;

        SpriteRenderer spriteRenderer = newObj.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingOrder = sortingOrder;
    }

    // New method to clear existing terrain
    public void ClearTerrain()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
