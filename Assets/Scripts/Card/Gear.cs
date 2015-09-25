using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Gear : MonoBehaviour
{
    public int value;
    public int strength;
    public static Sprite sprite;

    void Start()
    {
        value = strength * 10;
    }

    public static Gear buildGear(int strength)
    {
        GameObject go = new GameObject();
        Gear gear = go.AddComponent<Gear>();
        gear.strength = strength;
        go.AddComponent<GearDrag>();
        CanvasGroup cg = go.AddComponent<CanvasGroup>();
        LayoutElement le = go.AddComponent<LayoutElement>();
        le.preferredWidth = 90;
        Image pic = go.AddComponent<Image>();
        pic.sprite = sprite;
        return gear;
    }
}
