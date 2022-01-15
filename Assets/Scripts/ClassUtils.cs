using System.Collections.Generic;
using UnityEngine;

class StringVector3Dictionary : Dictionary<string, Vector3>
{
    public override string ToString()
    {
        string str = "{\n";
        foreach (string key in this.Keys)
        {
            str += key + " : " + this[key] + ",\n";
        }
        str += "}";

        return str;
    }
}