using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeathPlaneController : MonoBehaviour
{

    public Transform playerSpawnPoint;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Death");
        if(collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<PlayerBehavior>().lifeCounter.LoseLife();
            collision.gameObject.GetComponent<PlayerBehavior>().health.ResetHealth();

            if (collision.gameObject.GetComponent<PlayerBehavior>().lifeCounter.value > 0)
            {
                Respawn(collision.gameObject);

                FindObjectOfType<SoundManager>().PlaySoundFX(SoundFX.DEATH, Channel.DEATH);
                // todo: play the death sound
            }
            
        }
    }

    public void Respawn(GameObject go)
    {
        go.transform.position = playerSpawnPoint.position;
    }
}
