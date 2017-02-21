using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {
    public override bool GetConditionButton(string input)
    {
        return Input.GetButton(input);
    }

    public override bool GetConditionButtonDown(string input)
    {
        return Input.GetButtonDown(input);
    }

    public override bool GetConditionButtonUp(string input)
    {
        return Input.GetButtonUp(input);
    }

}
