
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class targetItem : MonoBehaviour
{
    private GameObject pickUpText;
    private GameObject blockOnPlayer;

    void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pickUpText = FindPickUpText(other.gameObject);
            pickUpText.SetActive(true);

            // Check if 'E' key is held down to pick up the Block
            if (Input.GetKey(KeyCode.E))
            {
                gameObject.SetActive(false);

                // Find the BlockOnPlayer GameObject in the player's hierarchy
                if (blockOnPlayer == null)
                {
                    blockOnPlayer = FindBlockOnPlayer(other.gameObject);
                }

                if (blockOnPlayer != null)
                {
                    blockOnPlayer.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("BlockOnPlayer GameObject not found in the player's hierarchy.");
                }

                pickUpText.SetActive(false);
            }
        }
    }
    //for the hidden item
    private GameObject FindBlockOnPlayer(GameObject player)
    {
        Transform[] children = player.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {
            if (child.name == "BreadPlayerHands")
            {
                return child.gameObject;
            }
        }
        return null;
    }
    //for the hidden text
    private GameObject FindPickUpText(GameObject player)
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Text");
        if (canvas != null)
        {
            Transform[] children = canvas.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                if (child.name == "pickupText")
                {
                    return child.gameObject;
                }
            }
        }
        else
        {
            Debug.LogWarning("Canvas with tag 'Text' not found.");
        }
        return null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pickUpText.SetActive(false);
        }
    }
}
