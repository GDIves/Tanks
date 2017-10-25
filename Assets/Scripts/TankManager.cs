using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TankManager
{
    public Color playerColor;
    public Transform spawnPoint;
    [HideInInspector] public int playerNum;
    [HideInInspector] public string coloredPlayerText;
    [HideInInspector] public GameObject instance;
    [HideInInspector] public int wins;

    private TankMovement tankMovement;
    private TankShooting tankShooting;
    private GameObject canvasGameObject;

    public void Setup()
    {
        tankMovement = instance.GetComponent<TankMovement>();
        tankShooting = instance.GetComponent<TankShooting>();
        canvasGameObject = instance.GetComponentInChildren<Canvas>().gameObject;

        tankMovement.playerNum = playerNum;
        tankShooting.playerNum = playerNum;

        coloredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(playerColor) + ">PLAYER " + playerNum + "</color>";

        MeshRenderer[] tankRenderers = instance.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < tankRenderers.Length; i++)
        {
            tankRenderers[i].material.color = playerColor;
        }
    }

    public void DisableControl()
    {
        tankMovement.enabled = false;
        tankShooting.enabled = false;
        canvasGameObject.SetActive(false);
    }

    public void EnableControl()
    {
        tankMovement.enabled = true;
        tankShooting.enabled = true;
        canvasGameObject.SetActive(true);
    }

    public void Reset()
    {
        instance.transform.position = spawnPoint.position;
        instance.transform.rotation = spawnPoint.rotation;
        instance.SetActive(false);
        instance.SetActive(true);
    }
}
