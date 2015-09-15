// This is the master class for special abilities

using UnityEngine;
using System.Collections;

[System.Serializable]
public class Special
{

    [SerializeField]
    string _name;
    [SerializeField]
    string _3CharDesig;
    [SerializeField]
    Sprite _icon;
    [SerializeField]
    string _description;
    [SerializeField]
    enhance enhancedAttribute;
    [SerializeField]

    public enum enhance
    {
        none,
        attack,
        defense,
        other
    }

    public Special()
    {
        _name = "";
        _icon = new Sprite();
        _description = "Description goes here";
        enhancedAttribute = enhance.none;
    }

    public Special(string name, string threeChar, string description, enhance enhanced, Sprite icon = null)
    {
        this._name = name;
        this._3CharDesig = threeChar;
        this._description = description;
        this.enhancedAttribute = enhanced;
        this.Icon = icon;
    }


    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public string CharDesig
    {
        get { return _3CharDesig; }
        set
        {
            if (value == null || value.Length > 3)
            { return; }

            _3CharDesig = value;
        }
    }


    public Sprite Icon
    {
        get { return _icon; }
        set { _icon = value; }
    }


    public string Description
    {
        get { return _description; }
        set { _description = value; }
    }

    public enhance EnhancedAttribute
    {
        get { return enhancedAttribute; }
        set { enhancedAttribute = value; }
    }
}
