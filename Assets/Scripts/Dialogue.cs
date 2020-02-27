using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public enum Animal {Monkey, Dog, Eel};
    public Animal[] animals;
    [TextArea(3, 10)]
    public string[] sentences;
}
