using System;
using Microsoft.Xna.Framework;


namespace UDPLibrary
{
    public class PuzzleData
    {
        #region Puzzle Strings
        //all the strings shown to the user through the menu
        public static String l1 = "";
        public static String l2 = "";
        public static String l3 = "";

        public static String m1 = "";
        public static String m2 = "";
        public static String m3 = "";

        public static String r1 = "";
        public static String r2 = "";
        public static String r3 = "";

        public static String end = "";
        #endregion

        #region Colours, Padding, Texture transparency , Array Indices and Bounds
        public static Integer2 PuzzleTexturePadding = new Integer2(10, 10);
        public static Color PuzzleTextureColor = new Color(1, 1, 1, 0.9f);

        //the hover colours for menu items
        public static Color ColorPuzzleInactive = Color.Black;
        public static Color ColorPuzzleActive = Color.Red;

        //the position of the texture in the array of textures provided to the menu manager
        public static int TextureIndexPuzzle = 0;

        //bounding rectangles used to detect mouse over
        public static Rectangle BoundsL1 = new Rectangle(50, 50, 70, 40); //x, y, width, height
        public static Rectangle BoundsL2 = new Rectangle(50, 100, 120, 40);
        public static Rectangle BoundsL3 = new Rectangle(50, 150, 90, 40);


        public static Rectangle BoundsM1 = new Rectangle(50, 200, 140, 40);
        public static Rectangle BoundsM2 = new Rectangle(50, 250, 70, 40);
        public static Rectangle BoundsM3 = new Rectangle(50, 50, 70, 40);

        public static Rectangle BoundsR1 = new Rectangle(50, 100, 150, 40);
        public static Rectangle BoundsR2 = new Rectangle(50, 150, 190, 40);
        public static Rectangle BoundsR3 = new Rectangle(50, 200, 190, 40);

        public static Rectangle BoundsMenuExit = new Rectangle(400, 500, 50, 40);
        #endregion

    }
}
