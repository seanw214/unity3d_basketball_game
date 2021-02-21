using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public enum PlayerState
    {
        BBInPossesion,
        NoBB
    }

    public PlayerState playerState;

    private void Start()
    {
        playerState = PlayerState.NoBB;
    }

    private void Update()
    {
        print(this.gameObject.name + " " + playerState);
    }
}
