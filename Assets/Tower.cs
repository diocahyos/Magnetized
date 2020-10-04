using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

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
