using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] int width, height;
    [SerializeField] Sprite dirtSprite, grassSprite;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTerrain();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateTerrain()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 spawnPosition = new Vector2(x, y);
                SpawnSprite(dirtSprite, spawnPosition);
            }

            Vector2 grassSpawnPosition = new Vector2(x, height);
            SpawnSprite(grassSprite, grassSpawnPosition);
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
}
