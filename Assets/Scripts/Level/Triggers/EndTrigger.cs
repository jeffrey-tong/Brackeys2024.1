using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : BaseTrigger
{ 
    public override void Trigger(PlayerController controller)
    {
        EndGame();
    }

    private void EndGame()
    {
         TransitionManager.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
