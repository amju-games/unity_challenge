// ----------------------------------------------------------------------------------------------------------
// Unity test for ustwo - Jason Colman 2016 jason@amju.com
// ----------------------------------------------------------------------------------------------------------

using System.Collections;
using UnityEngine;

// ----------------------------------------------------------------------------------------------------------
// Various utils - TODO break up?
// ----------------------------------------------------------------------------------------------------------
public class Utils
{
    // ------------------------------------------------------------------------------------------------------

    // Returns trus if two floats are equal within the given tolerance
    public static bool VeryClose(float f1, float f2, float eps = 0.001f)
    {
        return (Mathf.Abs(f1 - f2) < eps);
    }

    // ------------------------------------------------------------------------------------------------------

    // Returns trus if two vector3s are equal within the given tolerance
    public static bool VeryClose(Vector3 v1, Vector3 v2, float eps = 0.001f)
    {
        // Can we use Mathf.Approximately here?
        return (   VeryClose(v1.x, v2.x, eps) 
                && VeryClose(v1.y, v2.y, eps)
                && VeryClose(v1.z, v2.z, eps));

        // (Using "table layout", and watch out for "last line effect"!)
    }

    // ------------------------------------------------------------------------------------------------------

    public static float CalcDesiredYAxisRot(Vector3 direction)
    {
        // Ignore y component of direction
        Vector3 dir = new Vector3(direction.x, 0, direction.z);
        dir.Normalize();
        // Dot with "zero direction" - this direction gives rotation of 0 degs.
        Vector3 ZERO_DIR = new Vector3(0, 0, 1);
        float c = Vector3.Dot(dir, ZERO_DIR);
        // Convert from rads to degs
        float degs = Mathf.Rad2Deg * (Mathf.Acos(c));
        // Calc sign of angle
        if (Vector3.Cross(dir, ZERO_DIR).y > 0)
        {
            degs = -degs;
        }
        return degs;
    }

    // ------------------------------------------------------------------------------------------------------
}

