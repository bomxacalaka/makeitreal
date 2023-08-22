using UnityEngine;

public class TimingGame : MonoBehaviour
{
    public Transform movingLine; // Reference to the moving line
    public Transform greenBar;   // Reference to the green bar
    public Transform redBar;     // Reference to the red bar

    private bool isMovingRight = true;
    public float moveSpeed = 5f;
    private const float speedIncreaseFactor = 1.1f;
    public bool isActive = false;

    private int difficultyLevel = 1;
    private const float minGreenBarWidth = 0.1f;
    private const float decreaseAmount = 0.2f;
    private float initialGreenBarWidth;

    private Vector2 greenLeftEdge;
    private Vector2 greenRightEdge;
    private Vector2 redLeftEdge;
    private Vector2 redRightEdge;

    private void Start()
    {
        initialGreenBarWidth = greenBar.transform.lossyScale.x;


        UpdateEdges();
    }

    public void UpdateEdges()
    {
        // Green Bar Edges
        SpriteRenderer greenRenderer = greenBar.GetComponent<SpriteRenderer>();
        float greenSpriteWidth = greenRenderer.sprite.bounds.size.x * greenBar.transform.lossyScale.x;

        greenLeftEdge = new Vector2(greenBar.position.x - (greenSpriteWidth / 2), greenBar.position.y);
        greenRightEdge = new Vector2(greenBar.position.x + (greenSpriteWidth / 2), greenBar.position.y);

        // Red Bar Edges
        SpriteRenderer redRenderer = redBar.GetComponent<SpriteRenderer>();
        float redSpriteWidth = redRenderer.sprite.bounds.size.x * redBar.transform.lossyScale.x;

        redLeftEdge = new Vector2(redBar.position.x - (redSpriteWidth / 2), redBar.position.y);
        redRightEdge = new Vector2(redBar.position.x + (redSpriteWidth / 2), redBar.position.y);
    }

    void Update()
    {
        if (isActive)
        {
            // Moving the line left and right
            if (isMovingRight)
            {
                movingLine.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                if (movingLine.position.x > redRightEdge.x)
                {
                    isMovingRight = false;
                }
            }
            else
            {
                movingLine.Translate(Vector3.left * moveSpeed * Time.deltaTime);
                if (movingLine.position.x < redLeftEdge.x)
                {
                    isMovingRight = true;
                }
            }

            // Draw debug lines for Green Bar boundaries
            Debug.DrawLine(new Vector3(greenLeftEdge.x, greenBar.position.y - 1, 0), new Vector3(greenLeftEdge.x, greenBar.position.y + 1, 0), Color.green);
            Debug.DrawLine(new Vector3(greenRightEdge.x, greenBar.position.y - 1, 0), new Vector3(greenRightEdge.x, greenBar.position.y + 1, 0), Color.green);

            // Draw debug lines for Red Bar boundaries
            Debug.DrawLine(new Vector3(redLeftEdge.x, redBar.position.y - 1, 0), new Vector3(redLeftEdge.x, redBar.position.y + 1, 0), Color.red);
            Debug.DrawLine(new Vector3(redRightEdge.x, redBar.position.y - 1, 0), new Vector3(redRightEdge.x, redBar.position.y + 1, 0), Color.red);

            // Check for user input
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CheckSuccess();
            }
        }
    }


    private void CheckSuccess()
    {
        if (movingLine.position.x > greenLeftEdge.x && movingLine.position.x < greenRightEdge.x)
        {
            Debug.Log("You Win!");
            IncreaseDifficulty();
        }
        else
        {
            Debug.Log("You Lose!");
        }
    }

    private void IncreaseDifficulty()
    {
        difficultyLevel++;

        float newWidth = greenBar.localScale.x - decreaseAmount;
        newWidth = Mathf.Max(newWidth, minGreenBarWidth);

        greenBar.localScale = new Vector3(newWidth, greenBar.localScale.y, greenBar.localScale.z);

        // Debug the x-scale of the green bar
        Debug.Log("Green Bar X Scale: " + greenBar.localScale.x);

        UpdateEdges(); // Update the edges after changing the green bar's size

        // Increase the moving line's speed
        moveSpeed *= speedIncreaseFactor;
    }

}
