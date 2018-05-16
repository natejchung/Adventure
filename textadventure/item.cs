using System;

class item
{
    public string name;
    public bool infinite;
    public bool invisible;

    public item(string n, bool i, bool iv)
    {
        name = n;
        infinite = i;
        invisible = iv;
    }
    public virtual void use()
    {

    }
}
