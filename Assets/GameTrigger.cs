using UnityEngine;
using System.Collections;

public class GameTrigger : MonoBehaviour
{
    public TimingGame timingGame;
    public GameObject gameElements;
    private CharacterController2D playerController;

    private bool gameAlreadyTriggered = false;

    private void Awake()
    {
        playerController = FindObjectOfType<CharacterController2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GameStarter") && !timingGame.isActive && !gameAlreadyTriggered)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        gameAlreadyTriggered = true;

        // Set the gameElements' x position to be the same as the player's x position
        Vector3 playerPosition = playerController.transform.position;
        gameElements.transform.position = new Vector3(playerPosition.x, gameElements.transform.position.y, gameElements.transform.position.z);

        if (playerController)
            playerController.FreezePlayer();

        gameElements.SetActive(true);
        timingGame.isActive = true;
    }

    public void EndGame()
    {
        gameElements.SetActive(false);
        timingGame.isActive = false;

        // Call the coroutine to handle the unfreezing after a delay
        StartCoroutine(DelayedUnfreezePlayer());
    }

    private IEnumerator DelayedUnfreezePlayer()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(.1f);

        if (playerController)
            playerController.UnfreezePlayer();
    }
}
