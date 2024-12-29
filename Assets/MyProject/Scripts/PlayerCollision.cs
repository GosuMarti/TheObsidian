using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField]
    private string gameOverSceneName = "SampleScene";

    [SerializeField]
    private string monsterTag = "Enemy";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(monsterTag))
        {
            PlayerDies();
        }

        if (collision.gameObject.CompareTag("Death")){
            {
                PlayerDies();
            }
        }

        if (collision.gameObject.CompareTag("Finish"))
        {
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene("EndScreen");
            }
        }
    }

    private void PlayerDies()
    {
        Debug.Log("Player has died! Loading Game Over scene...");

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene(gameOverSceneName);
    }
}
