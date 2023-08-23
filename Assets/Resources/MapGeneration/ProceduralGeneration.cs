using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] int width = 1000, height = 50;
    [SerializeField] Sprite grassSprite, dirtSprite, rockSprite, treeSprite;
    [SerializeField] Transform target;

    [Header("Noise Settings")]
    [SerializeField] float scale = 0.01f;
    [SerializeField] float heightMultiplier = 50;
    [SerializeField] float secondaryScale = 0.02f; // New secondary noise scale
    [SerializeField] float secondaryWeight = 0.5f; // Weighting for secondary noise

    private float xOffset, yOffset;
    private int[,] map;

    void Start()
    {
        // Load sprites from Resources folder
        if (grassSprite == null || dirtSprite == null || rockSprite == null || treeSprite == null)
        {
            grassSprite = Resources.Load<Sprite>("Sprites/grass");
            dirtSprite = Resources.Load<Sprite>("Sprites/dirt");
            rockSprite = Resources.Load<Sprite>("Sprites/rock");
            treeSprite = Resources.Load<Sprite>("Sprites/tree");
        }
    }

    private void OnValidate()
    {
        xOffset = Random.Range(0, 9999);
        yOffset = Random.Range(0, 9999);
    }

    private void Update()
    {
        Generation();
    }

    public void Generation()
    {
        ClearTerrain();
        GenerateTerrain();
    }


    public void GenerateTerrain(int mainOffset = 0)
    {

        int playerX = (int) target.position.x;

        playerX -= 50;


        for (int x = 0 + playerX; x < width + playerX; x++)
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
                        SpawnSprite(rockSprite, spawnPosition + Vector2.up * Random.Range(-y, 0), 1);

                    }
                }
                else if (y == terrainHeight + 1)
                {
                    SpawnSprite(grassSprite, spawnPosition);
                }
            }
        }

    }

    void LoadChunk(int x1, int x2)
    {
        for (int x = 0 + x1; x < x2 + x1; x++)
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
                        SpawnSprite(rockSprite, spawnPosition + Vector2.up * Random.Range(-y, 0), 1);

                    }
                }
                else if (y == terrainHeight + 1)
                {   
                    SpawnSprite(grassSprite, spawnPosition);
                }
            }
        }
    }

    void UnloadChunk(int x1, int x2)
    {
        // Get transform from location x1 to x1

    }



    void SpawnSprite(Sprite sprite, Vector2 position, int sortingOrder = 0)
    {
        GameObject newObj = new GameObject("SpriteObject");
        newObj.transform.position = position;
        newObj.transform.parent = transform;

        SpriteRenderer spriteRenderer = newObj.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingOrder = sortingOrder;

        // Add a box collider to the sprite
        newObj.AddComponent<BoxCollider2D>();
        newObj.transform.localPosition = position;

        // Assign layer ground to the sprite
        newObj.layer = LayerMask.NameToLayer("Ground");
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
