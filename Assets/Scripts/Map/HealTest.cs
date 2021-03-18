using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealTest : MonoBehaviour
{
    public int playerHP = 1;
    public int maxHP = 2;

    public void Heal()
    {
         playerHP += 1;
    }
}
