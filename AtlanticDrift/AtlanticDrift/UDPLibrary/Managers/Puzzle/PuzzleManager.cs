using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AtlanticDrift;


namespace UDPLibrary
{
    public class PuzzleManager : DrawableGameComponent
    {
        #region Variables
        private List<PuzzleItem> puzzleItemList;
        private Main game;
        private SpriteFont puzzleFont;
        private Color puzzleTextureBlendColor;

        private Texture2D[] puzzleTextures;
        private Rectangle textureRectangle;

        private PuzzleItem l1, m1, r1;
        private PuzzleItem l2, m2, r2;
        private PuzzleItem l3, m3, r3;


        protected int currentPuzzleTextureIndex = 0; //0 = main, 1 = volume
        private bool bPaused;
        #endregion

        #region Properties
        public Color PuzzleTextureBlendColor
        {
            get
            {
                return puzzleTextureBlendColor;
            }
            set
            {
                puzzleTextureBlendColor = value;
            }
        }
        public bool Pause
        {
            get
            {
                return bPaused;
            }
            set
            {
                bPaused = value;
            }
        }
        #endregion

        #region Core menu manager - No need to change this code
        public PuzzleManager(Main game, Texture2D[] puzzleTextures,
            SpriteFont puzzleFont, Integer2 textureBorderPadding,
            Color puzzleTextureBlendColor)
            : base(game)
        {
            this.game = game;

            //load the textures
            this.puzzleTextures = puzzleTextures;

            //background blend color for the menu
            this.puzzleTextureBlendColor = puzzleTextureBlendColor;

            //menu font
            this.puzzleFont = puzzleFont;

            //stores all menu item (e.g. Save, Resume, Exit) objects
            this.puzzleItemList = new List<PuzzleItem>();

            //set the texture background to occupy the entire screen dimension, less any padding
            this.textureRectangle = game.ScreenRectangle;

            //deflate the texture rectangle by the padding required
            this.textureRectangle.Inflate(-textureBorderPadding.X, -textureBorderPadding.Y);

            //show the menu
            ShowPuzzle();
        }
        public override void Initialize()
        {
            //add the basic items - "Resume", "Save", "Exit"
            InitialisePuzzleOptions();

            //show the menu screen
            ShowPuzzleScreen();

            base.Initialize();
        }

        public void Add(PuzzleItem thePuzzleItem)
        {
            puzzleItemList.Add(thePuzzleItem);
        }

        public void Remove(PuzzleItem thePuzzleItem)
        {
            puzzleItemList.Remove(thePuzzleItem);
        }

        public void RemoveAll()
        {
            puzzleItemList.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            TestIfPaused();

            //menu is not paused so show and process
            if (!bPaused)
                ProcessPuzzleItemList();


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!bPaused)
            {
                //enable alpha blending on the menu objects
                this.game.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, DepthStencilState.Default, null);
                //draw whatever background we expect to see based on what menu or sub-menu we are viewing
                game.SpriteBatch.Draw(puzzleTextures[currentPuzzleTextureIndex], textureRectangle, this.puzzleTextureBlendColor);

                //draw the text on top of the background
                for (int i = 0; i < puzzleItemList.Count; i++)
                {
                    puzzleItemList[i].Draw(game.SpriteBatch, puzzleFont);
                }

                game.SpriteBatch.End();
            }

            base.Draw(gameTime);
        }

        private void TestIfPaused()
        {
            //if menu is pause and we press the show menu button then show the menu
            if ((bPaused) && this.game.KeyboardManager.IsFirstKeyPress(
                KeyData.KeyPauseShowMenu))
            {
                ShowPuzzle();
            }
        }

        private void ShowPuzzle()
        {
            //show the menu by setting pause to false
            bPaused = false;
            //generate an event to tell the object manager to pause
            EventDispatcher.Publish(new EventData("puzzle event", this, EventType.OnPause, EventCategoryType.MainMenu));

            //if the mouse is invisible then show it
            if (!this.game.IsMouseVisible)
                this.game.IsMouseVisible = true;
        }

        private void HidePuzzle()
        {
            //hide the menu by setting pause to true
            bPaused = true;
            //generate an event to tell the object manager to unpause
            EventDispatcher.Publish(new EventData("puzzle event", this, EventType.OnPlay, EventCategoryType.MainMenu));

            //if the mouse is invisible then show it
            if (this.game.IsMouseVisible)
                this.game.IsMouseVisible = false;

            this.game.MouseManager.SetPosition(this.game.ScreenCentre);
        }

        private void ExitPuzzle()
        {
            //generate an event to tell the main method to exit - need to add code in main to catch this event
            EventDispatcher.Publish(new EventData("puzzle event", this, EventType.OnExit, EventCategoryType.MainMenu));
        }

        //iterate through each menu item and see if it is "highlighted" or "highlighted and clicked upon"
        private void ProcessPuzzleItemList()
        {
            for (int i = 0; i < puzzleItemList.Count; i++)
            {
                PuzzleItem item = puzzleItemList[i];

                //is the mouse over the item?
                if (this.game.MouseManager.Bounds.Intersects(item.Bounds))
                {
                    item.SetActive(true);

                    //is the left mouse button clicked
                    if (game.MouseManager.IsLeftButtonClickedOnce())
                    {
                        DoPuzzleAction(puzzleItemList[i].Name);
                        break;
                    }
                }
                else
                {
                    item.SetActive(false);
                }
            }
        }

        //to do - dispose, clone
        #endregion

        #region Code specific to your application
        private void InitialisePuzzleOptions()
        {
            //add the menu items to the list
            this.l1 = new PuzzleItem(MenuData.Menu_Play, MenuData.Menu_Play,
                MenuData.BoundsMenuPlay, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.l2 = new PuzzleItem(MenuData.StringMenuRestart, MenuData.StringMenuRestart,
                MenuData.BoundsMenuRestart, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.l3 = new PuzzleItem(MenuData.StringMenuAudio, MenuData.StringMenuAudio,
               MenuData.BoundsMenuAudio, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);

            this.m1 = new PuzzleItem(MenuData.Menu_Play, MenuData.Menu_Play,
                MenuData.BoundsMenuPlay, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.m2 = new PuzzleItem(MenuData.StringMenuRestart, MenuData.StringMenuRestart,
                MenuData.BoundsMenuRestart, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.m3 = new PuzzleItem(MenuData.StringMenuAudio, MenuData.StringMenuAudio,
               MenuData.BoundsMenuAudio, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);

            this.r1 = new PuzzleItem(MenuData.Menu_Play, MenuData.Menu_Play,
                MenuData.BoundsMenuPlay, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.r2 = new PuzzleItem(MenuData.StringMenuRestart, MenuData.StringMenuRestart,
                MenuData.BoundsMenuRestart, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.r3 = new PuzzleItem(MenuData.StringMenuAudio, MenuData.StringMenuAudio,
               MenuData.BoundsMenuAudio, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);


            //add your new menu options here...
        }
        //perform whatever actions are listed on the menu
        private void DoPuzzleAction(String name)
        {
            if (name.Equals(MenuData.Menu_Play))
            {
                HidePuzzle();
            }
            
            else if (name.Equals(MenuData.StringMenuExitYes))
            {
                ExitPuzzle();
            }


            //add your new menu actions here...
        }

        private void ShowPuzzleScreen()
        {
            //remove any items in the menu
            RemoveAll();
            //add the appropriate items
            Add(l1);
            Add(l2);
            Add(l3);

            Add(m1);
            Add(m2);
            Add(m3);

            Add(r1);
            Add(r2);
            Add(r3);

            //set the background texture
            currentPuzzleTextureIndex = MenuData.TextureIndexMainMenu;
        }

        #endregion
    }
}
