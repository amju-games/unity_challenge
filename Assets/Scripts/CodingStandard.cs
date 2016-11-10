// ----------------------------------------------------------------------------------------------------------
// Unity test for ustwo - Jason Colman 2016 jason@amju.com
// ----------------------------------------------------------------------------------------------------------

/* 
 * This is my coding standard - still working on it.
 * This kind of comment (slash/star) explains the coding standard -- usually I use // only.
 * /

/*
 * All files start with the comment block seen at the top of the file.
 */

/*
 * Whitespace:
 * Use spaces, not tabs. 4 spaces for indentation.
 * Never more than one blank line.
 */  

/*
 * using/imports at top of file should be in alphabetical order
 * - Reduce merge conflicts
 * - Reduce chance of duplication
 * - They are not alphabetical by default
 */
using System.Collections;
using UnityEngine;

/* One class per file, except for tightly coupled classes */

/* 
 * Classes should be preceded by a comment block explaining responsibility, looking like this
 * /
// ----------------------------------------------------------------------------------------------------------
// CodingStandard class /* first line: class name */
// Shows examples of how I would like my code to look /* following lines: description */
// ----------------------------------------------------------------------------------------------------------
public class CodingStandard : MonoBehaviour
{
/*
 * Braces: ALWAYS on the next line, for ifs, for loops, etc., as well as opening function defs etc.
 * The braces themselves are not indented.
 */

    /*
     * Class organisation:
     * Public/private methods/private data: These should be in clearly separated sections.
     * Use lines of dashes to separate functions or data in a block. Use lines of == to separate the blocks.
     * Don't extend code or comments past the end of the separating lines.
     */

    /*
     * ONE Blank line above and below separator line. Never more than one blank line.
     */

    // ------------------------------------------------------------------------------------------------------

    // Use this for initialization
    public void PublicFunction()
    {
    }
	
    // ======================================================================================================
    // Private member functions

    // Update is called once per frame
    private void PrivateFunction()
    {
    }

    // ======================================================================================================
    // Private member variables

    // Private member variables go here.
    // Use m_ prefix. Static variables use s_ prefix.

}

