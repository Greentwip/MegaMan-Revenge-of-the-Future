using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour {
    public abstract bool GetConditionButton(string input);
    public abstract bool GetConditionButtonDown(string input);
    public abstract bool GetConditionButtonUp(string input);

}
