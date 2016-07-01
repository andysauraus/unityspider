using UnityEngine;
using System.Collections;

public static class Logging
{
    private static System.Guid id;
    public static System.Guid instanceID
    {
        get
        {
            if (null != id)
            {
                return id;
            }
            else
            {
                id = new System.Guid();
            }
            return id;
        }
    }

    public static void Log(object message)
    {
        Debug.Log(instanceID + " : " + message);
    }
}
