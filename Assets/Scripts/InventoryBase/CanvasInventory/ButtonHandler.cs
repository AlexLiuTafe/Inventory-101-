using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    //* THIS SCRIPT IS FOR THE BUTTON PREFAB TO SEND VALUE TO THE LISTENER
    public int i;
    public GameObject target;
   public void ButtonEvent()
    {
        //Send this Message to the target which is the canvas 
        //*We set the target via script in canvas script.
        //            Name of the function that we want to call
        target.SendMessage("SelectedItem", i);

    }
}
