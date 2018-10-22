
public class StructWrapper<T> where T : struct
{

    private readonly object syncLock = new object();

    private T value;

    public T Value
    {
        get
        {
            lock (syncLock)
            {
                return value;
            }
        }
        set
        {
            lock (syncLock)
            {
                this.value = value;
            }
        }
    }

    public StructWrapper(T value)
    {
        this.Value = value;
    }

    public static implicit operator T(StructWrapper<T> w)
    {
        return w.Value;
    }

    public static implicit operator StructWrapper<T>(T s)
    {
        return new StructWrapper<T>(s);
    }
}
