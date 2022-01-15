using System;

[Serializable]
public class Pair<T, U>
{
    public T first;
    public U second;

    public Pair()
    {
    }

    public Pair(T first, U second)
    {
        this.first = first;
        this.second = second;
    }

    public override string ToString()
    {
        string str = "(" + this.first + ", " + this.second + ")";
        return str;
    }
}