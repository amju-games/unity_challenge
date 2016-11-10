// ----------------------------------------------------------------------------------------------------------
// Unity test for ustwo - Jason Colman 2016 jason@amju.com
// ----------------------------------------------------------------------------------------------------------

using NUnit.Framework;
using System.Collections;
using UnityEngine;

// ----------------------------------------------------------------------------------------------------------
// TestFloor class
// Unit tests for FloorTile class.
// ----------------------------------------------------------------------------------------------------------
[TestFixture] // Not required for Unity Editor Test Runner..?
public class TestFloor  
{
    // ------------------------------------------------------------------------------------------------------

    [Test]
    public void FloorStaticIsoCoordTest()
    {
        // Test calculation of iso coord from position
        // - static function
        Vector3 pos = new Vector3(1, 0, 3);
        Vector2 iso = FloorTile.CalcIsoCoord(pos);
        Assert.AreEqual(iso, new Vector2(1, 3));
    }

    // ------------------------------------------------------------------------------------------------------

    [Test]
    public void FloorIsoCoordTest()
    {
        // Test calculation of iso coord from position
        FloorTile floor = new FloorTile(0);
        // transform.position can't be used in this test.. because null, set later by Unity magic I guess..
        floor.Pos = new Vector3(3, 2, 4);
        floor.CalcIsoCoord();
        Assert.AreEqual(new Vector2(1, 2), floor.IsoCoord); // expected, actual
    }

    // ------------------------------------------------------------------------------------------------------

    [Test]
    public void FloorIsoCoordsAreAdjacent()
    {
        // Test adjacency test for iso coords - static function
        for (int i = -10; i < 10; i++) 
        {
            for (int j = -10; j < -10; j++)
            {
                // Two iso coords are adjacent if they differ by 1 in 1 axis.
                Assert.True(FloorTile.AreIsoCoordsAdjacent(new Vector2(i, j), new Vector2(i, j + 1)));
                Assert.True(FloorTile.AreIsoCoordsAdjacent(new Vector2(i, j), new Vector2(i, j - 1)));
                Assert.True(FloorTile.AreIsoCoordsAdjacent(new Vector2(i, j), new Vector2(i + 1, j)));
                Assert.True(FloorTile.AreIsoCoordsAdjacent(new Vector2(i, j), new Vector2(i - 1, j)));

                // Counter examples
                Assert.False(FloorTile.AreIsoCoordsAdjacent(new Vector2(i, j), new Vector2(i, j)));

                for (int k = 2; k < 10; k++)
                {
                    Assert.False(FloorTile.AreIsoCoordsAdjacent(new Vector2(i, j), new Vector2(i, j + k)));
                    Assert.False(FloorTile.AreIsoCoordsAdjacent(new Vector2(i, j), new Vector2(i, j - k)));
                    Assert.False(FloorTile.AreIsoCoordsAdjacent(new Vector2(i, j), new Vector2(i + k, j)));
                    Assert.False(FloorTile.AreIsoCoordsAdjacent(new Vector2(i, j), new Vector2(i - k, j)));
                }
            }
        }
    }

    // ------------------------------------------------------------------------------------------------------

    [Test]
    public void FloorTilesAreAdjacent()
    {
        // Create pairs of floor tiles which are adjacent in iso coords, but distant in 3D space.
        // These coords are taken from the cubes in the scene

        // Cubes 33 and 34
        FloorTile cube33 = new FloorTile(0, new Vector3( 4,  0,  3));
        Vector2 iso33 = cube33.IsoCoord;

        FloorTile cube34 = new FloorTile(1, new Vector3(-1, -4, -1)); 
        Vector2 iso34 = cube34.IsoCoord;

        Assert.AreEqual(new Vector2(4, 3), iso33); // exp, actual
        Assert.AreEqual(new Vector2(3, 3), iso34); // exp, actual

        Assert.True(FloorTile.AreIsoCoordsAdjacent(iso33, iso34));
        Assert.True(cube33.IsAdjacentTo(cube34));

        // Cubes 39 and 40 - not broken down so much
        Assert.True(
            new FloorTile(2, new Vector3(-6,     -4,    -1)).IsAdjacentTo(
            new FloorTile(3, new Vector3(-10.5f, -7.5f, -4.5f))));        
    }

    // ------------------------------------------------------------------------------------------------------
}
