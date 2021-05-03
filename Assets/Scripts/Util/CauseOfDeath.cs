using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CauseOfDeath
{
    private static string cause = "You died of:\nDEFAULT_VALUE";

    public static void UpdateCauseOfDeath(string reason)
    {
        string newCause = "You died of:\n" + reason;
        cause = newCause;
    }

    public static string GetCauseOfDeath()
    {
        return cause;
    }
}
