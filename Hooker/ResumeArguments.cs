namespace Hooker.Core;

public sealed class ResumeArguments
{
    public short MouseX
    {
        get;
        set
        {
            field = value;
            OnChanged();
        }
    }

    public short MouseY
    {
        get;
        set
        {
            field = value;
            OnChanged();
        }
    }

    public bool KeyboardOnly
    {
        get;
        set
        {
            field = value;
            OnChanged();
        }
    }

    public bool CycleMovement
    {
        get;
        set
        {
            field = value;
            OnChanged();
        }
    }

    public override string ToString() => $"{new { MouseX, MouseY, KeyboardOnly, CycleMovement }}";

    void OnChanged()
    {
        Console.Title = $"args: {this}";
    }
}
