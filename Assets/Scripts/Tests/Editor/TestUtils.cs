// ----------------------------------------------------------------------------------------------------------
// Unity test for ustwo - Jason Colman 2016 jason@amju.com
// ----------------------------------------------------------------------------------------------------------

using NUnit.Framework;
using System.Collections;
using UnityEngine;

// ----------------------------------------------------------------------------------------------------------
// TestUtils class
// Test the Utils class, which provides some useful functions.
// ----------------------------------------------------------------------------------------------------------
public class TestUtils 
{
    // ------------------------------------------------------------------------------------------------------

    // Test the function Utils.CalcDesiredYAxisRot
    [Test]
    public void DesiredAngleTest()
    {
        float d = Utils.CalcDesiredYAxisRot(new Vector3(0, 0, 1));
        Assert.IsTrue(Utils.VeryClose(d, 0.0f));

        d = Utils.CalcDesiredYAxisRot(new Vector3(1, 0, 0));
        Assert.AreEqual(90.0f, d);
        Assert.IsTrue(Utils.VeryClose(d, 90.0f));

        d = Utils.CalcDesiredYAxisRot(new Vector3(-1, 0, 0));
        Assert.AreEqual(-90.0f, d);
        Assert.IsTrue(Utils.VeryClose(d, -90.0f));

    }

    // ------------------------------------------------------------------------------------------------------
}

