using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ProceduralGeneration2 : MonoBehaviour
{
    [SerializeField] GameObject floor;
    [SerializeField] Transform target;

    [Header("Settings")]
    [SerializeField] int viewDistanceRender = 10;
    [SerializeField] int spaceBetweenChunk = 10;
    [SerializeField] int deleteChunkBehindThreshold = 10;


    private int[] chunks;

   void Start()
    {
        chunks = new int[1000];
        for (int i = 0; i < 1000; i++)
        {
            chunks[i] = 0;
        }
    }


    void SpawnGameObject(GameObject gameObject, Vector3 position)
    {
        Instantiate(gameObject, position, Quaternion.identity);
    }

    void ChunkChecker(GameObject gameObject, float distance = 5f)
    {
        float playerX = target.position.x;
        float goX = gameObject.transform.position.x;

        if (goX - playerX >= -viewDistanceRender && goX - playerX <= viewDistanceRender && chunks[(int)goX] == 0)
        {
            SpawnGameObject(floor, new Vector3(goX + spaceBetweenChunk, 0, 0));
            chunks[(int)goX] = 1;
        }
        else
        {
            if (goX - playerX <= -deleteChunkBehindThreshold && chunks[(int)goX] == 1)
            {
                Destroy(gameObject);
                chunks[(int)goX] = 0;
            }
        }
    }

    public void Gen()
    {
        // Check all chunks
        foreach (GameObject chunk in GameObject.FindGameObjectsWithTag("Chunk"))
        {
            ChunkChecker(chunk);
        }
    }

    void Update()
    {
        Gen();
    }
}