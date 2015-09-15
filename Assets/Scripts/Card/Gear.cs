using UnityEngine;
using System.Collections;

public class Gear : MonoBehaviour
{
    public int value;
    public int strength;

    void Start()
    {
        value = strength * 10;
    }
}
