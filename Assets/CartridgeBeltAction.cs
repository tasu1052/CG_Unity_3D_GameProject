using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartridgeBeltAction : MonoBehaviour
{
   public void SetAllDamage(float extra)
    {
        Managers.Game.extraDamage = extra;
    }
}
