using UnityEngine;
using System.Collections;

public class LeverScript : MonoBehaviour
{
    public int leverNumber;

    public LeverController leverController;

    public void LeverNumber()
    {
        if (leverController.pulls <= leverController.pullLimit - 1)
        {
            leverController.playerOrder = leverController.playerOrder + leverNumber;
            leverController.pulls++;
        }

        leverController.PullSound();
    }
}