using System.Collections;

public class IEnumeratorHolder {

    private IEnumerator iEnumerator;

    public IEnumeratorHolder(IEnumerator iEnumerator)
    {
        this.iEnumerator = iEnumerator;
    }

    public IEnumerator Get()
    {
        return iEnumerator;
    }

}
