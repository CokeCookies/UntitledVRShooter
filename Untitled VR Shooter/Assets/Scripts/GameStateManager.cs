using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  //for restarting the scene

//Game State Manager rovides high level coordination of game states (including transitions, timings, UI events)
public class GameStateManager : MonoBehaviour
{
    //Create states as an enum
    private enum State { Initial, Playing, Pause, Win };
    private State state;

    //The Initial state handles the game's opening state (prior to the player choosing to begin the game)
    IEnumerator InitialState()
    {
        Debug.Log("Enter: initial state");
        //These actions will be executed upon entering the state (e.g. enable a UI element, setting spawning variables)

        while (state == State.Initial)
        {
            //Actions in here are run every update (e.g. enemy pathfinding to a player)
            //Chances are we won't put much in here.
            //Checking for things like win conditions can be added as transition conditions in the switch statement
            yield return 0;
        }

        Debug.Log("Exit: initial state");
        //These actions will be executed upon exiting the state (e.g. disable a UI element)
    }

    //The playing state covers main gameplay
    IEnumerator PlayingState()
    {
        Debug.Log("Enter: playing state");

        while (state == State.Playing)
        {
            yield return 0;
        }

        Debug.Log("Exit: playing state");
    }

    //The Pause state handles the game during pause (UI, timescale, etc.)
    IEnumerator PauseState()
    {
        Debug.Log("Enter: pause state");

        while (state == State.Pause)
        {
            yield return 0;
        }

        Debug.Log("Exit: pause state");
    }

    //The Win state handles the things that happen when the player completes the game (restart, quit, etc.)
    IEnumerator WinState()
    {
        Debug.Log("Enter: win state");

        while (state == State.Win)
        {
            yield return 0;
        }

        Debug.Log("Exit: win state");
    }

    private void Awake()
    {      
    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.Initial;
        StartCoroutine(InitialState());
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Initial:

                //Conditions for transitioning to another state go here!
                //E.G.             
/*                if (Input.GetButtonDown("Cancel") && !gameFinished)
                {
                    state = State.Playing;
                    StartCoroutine(PlayingState());
                }*/

                break;

            case State.Playing:

                break;

            case State.Pause:

                break;

            case State.Win:

                break;

            default:
                Debug.Log("Default case: you shouldn't be here.");
                break;
        }
    }
}