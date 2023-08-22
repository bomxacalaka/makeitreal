using UnityEngine;

public class GameTrigger : MonoBehaviour
{
    public TimingGame timingGame; // Reference to your TimingGame script
    public GameObject gameElements; // Reference to the GameObject containing all game elements

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision Detected with: " + other.gameObject.name);

        // Check if the character has collided with the GameStarter object and the game is not already started
        if (other.CompareTag("GameStarter") && !timingGame.isActive)
        {
            StartGame();
        }
    }



private void StartGame()
    {
        // Activate the game elements
        gameElements.SetActive(true);

        // Trigger the game
        timingGame.isActive = true;
        // Add any other logic here if needed to start the game
    }
}
