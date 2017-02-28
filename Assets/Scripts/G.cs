using UnityEngine;
using System.Collections;

public sealed class G
{
    private static volatile G instance;

    public static G Sys
    {
        get
        {
            if (G.instance == null)
                G.instance = new G();
            return G.instance;
        }
    }
}
