using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    public PlayerController player;

    private void OnMouseDown()
    {
        if (player)
        {
            player.Pull();
        }
    }

    private void OnMouseUp()
    {
        if (player)
        {
            player.Release();
            player = null;
        }
    }

    public void SetPlayer(PlayerController player)
    {
        this.player = player;
    }
}
