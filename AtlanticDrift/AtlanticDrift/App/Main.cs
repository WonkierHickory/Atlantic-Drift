using UDPLibrary;
using JigLibX.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Audio;

namespace AtlanticDrift
{
    public class Main : Microsoft.Xna.Framework.Game
    {

        #region Variables

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private BasicEffect primitiveEffect;
        private BasicEffect texturedPrimitiveEffect;
        private BasicEffect texturedModelEffect;

        private Vector2 screenCentre;
        private Microsoft.Xna.Framework.Rectangle screenRectangle;

        private CameraManager cameraManager;
        private KeyboardManager keyboardManager;
        private MouseManager mouseManager;
        private ObjectManager objectManager;
        private SoundManager soundManager;
        private PhysicsManager physicsManager;
        private UIManager uiManager;
        private MenuManager menuManager;

        //private GenericDictionary<string, Video> videoDictionary;
        private GenericDictionary<string, IVertexData> vertexDictionary;
        private GenericDictionary<string, Texture2D> textureDictionary;
        private GenericDictionary<string, SpriteFont> fontDictionary;
        private GenericDictionary<string, Model> modelDictionary;
        private GenericDictionary<string, Camera3DTrack> trackDictionary;

        private EventDispatcher eventDispatcher;

        private AudioEmitter emitter = new AudioEmitter();
        private AudioListener listener = new AudioListener();

        private float angle = 0;
        private float distance = 5;

        #endregion

        #region Properties

        public Microsoft.Xna.Framework.Rectangle ScreenRectangle
        {
            get
            {
                return screenRectangle;
            }
        }
        public SpriteBatch SpriteBatch
        {
            get
            {
                return this.spriteBatch;
            }
        }
        public PhysicsManager PhysicsManager
        {
            get
            {
                return this.physicsManager;
            }
        }
        public ObjectManager ObjectManager
        {
            get
            {
                return this.objectManager;
            }
        }
        public EventDispatcher EventDispatcher
        {
            get
            {
                return this.eventDispatcher;
            }
        }
        public Vector2 ScreenCentre
        {
            get
            {
                return this.screenCentre;
            }
        }
        //nmcg - 18.3.16
        public SoundManager SoundManager
        {
            get
            {
                return soundManager;
            }
        }
        public CameraManager CameraManager
        {
            get
            {
                return this.cameraManager;
            }
        }
        public KeyboardManager KeyboardManager
        {
            get
            {
                return this.keyboardManager;
            }
        }
        public MouseManager MouseManager
        {
            get
            {
                return this.mouseManager;
            }
        }

        #endregion

        #region Constructors
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        #endregion

        #region Event Handling
        //handle the relevant menu events
        public virtual void eventDispatcher_MainMenuChanged(EventData eventData)
        {
            if (eventData.EventType == EventType.OnExit)
            {
                Exit();
            }
            else if (eventData.EventType == EventType.OnRestart)
            {
                this.LoadGame();
            }


            if (eventData.EventCategoryType == EventCategoryType.MainMenu)
                this.soundManager.PlayCue("click");

            //if(eventData.EventType == EventType.OnVolumeUp)
            //this.soundManager.

            //if(eventData.EventType == EventType.OnVolumeDown)
            //this.soundManager.

            //if (eventData.EventType == EventType.OnMute)
            //    this.soundManager.ChangeVolume(0, "click");
        }

        public virtual void eventDispatcher_ZoneChanged(EventData eventData)
        {
            if (eventData.EventType == EventType.OnZoneEnter)
            {

            }
        }
        #endregion

        #region Load Assets

        private void LoadFonts()
        {

            this.fontDictionary.Add("menu", Content.Load<SpriteFont>("Assets/Fonts/menu"));

            this.fontDictionary.Add("debug", Content.Load<SpriteFont>("Assets/Debug/Fonts/debug"));

        }

        private void LoadModels()
        {
            this.modelDictionary.Add("box", Content.Load<Model>("Assets/Models/box"));

            this.modelDictionary.Add("LowPoly", Content.Load<Model>("Assets/Models/LowPolyIdea"));

            this.modelDictionary.Add("islandBarrier", Content.Load<Model>("Assets/Models/islandBarrier"));

            this.modelDictionary.Add("foliage", Content.Load<Model>("Assets/Models/AnotherFool"));

            this.modelDictionary.Add("tree", Content.Load<Model>("Assets/Models/tree"));

            this.modelDictionary.Add("treeLeaf", Content.Load<Model>("Assets/Models/leaf"));

            this.modelDictionary.Add("puzzleChest", Content.Load<Model>("Assets/Models/chest"));

            this.modelDictionary.Add("radio", Content.Load<Model>("Assets/Models/Radio"));

            this.modelDictionary.Add("volcano", Content.Load<Model>("Assets/Models/Volcano"));

            this.modelDictionary.Add("rock", Content.Load<Model>("Assets/Models/Rocks"));

        }

        private void LoadTextures()
        {
            this.textureDictionary.Add("checkerboard",
                Content.Load<Texture2D>("Assets/Textures/Debug/checkerboard"));

            this.textureDictionary.Add("sand",
                Content.Load<Texture2D>("Assets/Textures/island/temp"));

            this.textureDictionary.Add("water",
                Content.Load<Texture2D>("Assets/Textures/island/water"));

            this.textureDictionary.Add("islandTex",
                Content.Load<Texture2D>("Assets/Textures/island/islandTex"));

            this.textureDictionary.Add("chestTex",
                Content.Load<Texture2D>("Assets/Textures/Models/chestTex"));

            this.textureDictionary.Add("rockTex",
                Content.Load<Texture2D>("Assets/Textures/Models/rockTex"));

            #region Sky
            this.textureDictionary.Add("skybox_back",
                Content.Load<Texture2D>("Assets/Textures/Skybox/back"));
            this.textureDictionary.Add("skybox_front",
                Content.Load<Texture2D>("Assets/Textures/Skybox/front"));
            this.textureDictionary.Add("skybox_left",
                Content.Load<Texture2D>("Assets/Textures/Skybox/left"));
            this.textureDictionary.Add("skybox_right",
                Content.Load<Texture2D>("Assets/Textures/Skybox/right"));
            this.textureDictionary.Add("skybox_sky",
                Content.Load<Texture2D>("Assets/Textures/Skybox/sky"));
            #endregion

            #region other

            this.textureDictionary.Add("controlsmenu",
                Content.Load<Texture2D>("Assets/Textures/Menu/controlsmenu1"));

            this.textureDictionary.Add("exitmenu",
                Content.Load<Texture2D>("Assets/Textures/Menu/exitmenu1"));

            this.textureDictionary.Add("exitmenuwithtrans",
                Content.Load<Texture2D>("Assets/Textures/Menu/exitmenuwithtrans"));

            this.textureDictionary.Add("mainmenu",
                Content.Load<Texture2D>("Assets/Textures/Menu/mainmenu1"));

            this.textureDictionary.Add("audiomenu",
                Content.Load<Texture2D>("Assets/Textures/Menu/audiomenu1"));

            this.textureDictionary.Add("ml",
                Content.Load<Texture2D>("Assets/Debug/Textures/ml"));

            this.textureDictionary.Add("slj",
                Content.Load<Texture2D>("Assets/Debug/Textures/slj"));

            this.textureDictionary.Add("white", 
                Content.Load<Texture2D>("Assets/Textures/UI/white"));

            this.textureDictionary.Add("mouseicons", 
                Content.Load<Texture2D>("Assets/Textures/UI/mouseicons"));


            this.textureDictionary.Add("test",
                Content.Load<Texture2D>("Assets/Textures/island/test"));

            #endregion

        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

        }
        protected override void UnloadContent()
        {
            //unload all resources
            this.fontDictionary.Dispose();
            this.textureDictionary.Dispose();
            this.trackDictionary.Dispose();
            this.modelDictionary.Dispose();
            this.vertexDictionary.Dispose();
        }
        #endregion

        #region Initialize Core
        private void LoadGame()
        {
            //remove anything from a previous run in object and ui manager
            this.objectManager.Clear();
            this.uiManager.Clear();

            //setup the world and all its objects
            int worldScale = 10000;
            #region Non Collidable Primitives
            InitializeSkyBox(worldScale);
            #endregion

            #region Non Collidable Models
            InitializeNonCollidableModels();
            #endregion

            #region Collidable
            InitializeStaticCollidableGround(worldScale);
            InitializeCollidableObjects();
            #endregion

            InitializeCameras();
            //InitializeUI();
        }

        protected override void Initialize()
        {
            InitializeEventDispatcher();
            InitializeStatics();
            IntializeGraphics(1024, 768);

            InitializeDictionaries();
            LoadFonts();
            LoadModels();
            LoadTextures();
            InitializeManagers(false);
            InitializeVertexData();
            InitializeEffects();

            //pulling out load game allows us to reload from the menu to restart, if we lose
            LoadGame();

            #region Event Handling
            this.eventDispatcher.MainMenuChanged += new EventDispatcher.MainMenuEventHandler(eventDispatcher_MainMenuChanged);

            this.eventDispatcher.ZoneChanged += new EventDispatcher.ZoneEventHandler(eventDispatcher_ZoneChanged);
            #endregion

            base.Initialize();
        }

        private void InitializeEventDispatcher()
        {
            this.eventDispatcher = new EventDispatcher(this, 50);
            Components.Add(this.eventDispatcher);
        }

        private void InitializeVertexData()
        {
            IVertexData vertexData = null;
            Microsoft.Xna.Framework.Graphics.PrimitiveType primitiveType;
            int primitiveCount;
            VertexPositionColor[] vertices = null;                  //anything wireframe
            VertexPositionColorTexture[] texturedVertices = null;   //anything with a texture!

            #region Origin Helper
            vertices = VertexInfo.GetVerticesPositionColorOriginHelper(out primitiveType, out primitiveCount);
            vertexData = new VertexData<VertexPositionColor>(vertices, primitiveType, primitiveCount);
            this.vertexDictionary.Add("origin", vertexData);
            #endregion

            #region Sphere
            vertices = VertexInfo.GetVerticesPositionColorSphere(1, 15, out primitiveType, out primitiveCount);
            vertexData = new VertexData<VertexPositionColor>(vertices, primitiveType, primitiveCount);
            this.vertexDictionary.Add("sphere", vertexData);
            #endregion

            #region Textured Quad (e.g. sky, signs, billboards)
            texturedVertices = VertexInfo.GetVerticesPositionColorTextureQuad(1, out primitiveType, out primitiveCount);
            vertexData = new VertexData<VertexPositionColorTexture>(texturedVertices, primitiveType, primitiveCount);
            this.vertexDictionary.Add("texturedquad", vertexData);
            #endregion

        }

        private void InitializeUI()
        {
            InitializeUIMousePointer();
        }

        private void InitializeUIMousePointer()
        {
            Transform2D transform = null;
            Texture2D texture = null;
            Microsoft.Xna.Framework.Rectangle sourceRectangle;

            //texture
            texture = this.textureDictionary["mouseicons"];
            transform = new Transform2D(Vector2.One);

            //show first of three images from the file
            sourceRectangle = new Microsoft.Xna.Framework.Rectangle(0, 0, 128, 128);

            UITextureObject texture2DObject = new UIMouseObject("mouse icon",
                ObjectType.UITexture2D, transform, new Color(127, 127, 127, 50),
                SpriteEffects.None, 1, texture,
                sourceRectangle,
                new Vector2(sourceRectangle.Width / 2.0f, sourceRectangle.Height / 2.0f),
                true);
            this.uiManager.Add(texture2DObject);
        }

        private void InitializeCameraTracks()
        {
            Camera3DTrack cameraTrack = null;

            cameraTrack = new Camera3DTrack(CurveLoopType.Oscillate);
            cameraTrack.Add(new Vector3(-20, 10, 10), -Vector3.UnitZ, Vector3.UnitY, 0);
            cameraTrack.Add(new Vector3(20, 5, 10), -Vector3.UnitZ, Vector3.UnitY, 5);
            cameraTrack.Add(new Vector3(50, 5, 10), -Vector3.UnitX, Vector3.UnitY, 10);

            this.trackDictionary.Add("simple", cameraTrack);

            cameraTrack = new Camera3DTrack(CurveLoopType.Oscillate);
            //start
            cameraTrack.Add(new Vector3(0, 2, 0), -Vector3.UnitY, Vector3.UnitZ, 0);
            //fast
            cameraTrack.Add(new Vector3(0, 100, 0), Vector3.UnitZ, Vector3.UnitY, 5);
            //slow
            cameraTrack.Add(new Vector3(0, 105, 0), Vector3.UnitZ, Vector3.UnitY, 7);
            //fall
            cameraTrack.Add(new Vector3(0, 2, 0), -Vector3.UnitY, Vector3.UnitZ, 8);

            this.trackDictionary.Add("puke", cameraTrack);
        }

        private void InitializeDictionaries()
        {
            this.textureDictionary = new GenericDictionary<string, Texture2D>("textures");
            this.fontDictionary = new GenericDictionary<string, SpriteFont>("fonts");
            this.modelDictionary = new GenericDictionary<string, Model>("model");
            this.trackDictionary = new GenericDictionary<string, Camera3DTrack>("camera tracks");

            //stores vertices for use by primitive object and textured primitive object
            this.vertexDictionary = new GenericDictionary<string, IVertexData>("vertex data");
        }

        private void IntializeGraphics(int width, int height)
        {
            this.graphics.PreferredBackBufferWidth = width;
            this.graphics.PreferredBackBufferHeight = height;
            this.screenCentre = new Vector2(width / 2, height / 2);
            this.screenRectangle = new Microsoft.Xna.Framework.Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            this.graphics.ApplyChanges();
        }

        private void InitializeStatics()
        {
            Actor.game = this;
            Controller.game = this;
            UIActor.game = this;
        }

        private void InitializeManagers(bool isMouseVisible)
        {
            //CD/CR
            this.physicsManager = new PhysicsManager(this);
            Components.Add(physicsManager);

            this.cameraManager = new CameraManager(this);
            Components.Add(this.cameraManager);

            this.keyboardManager = new KeyboardManager(this);
            Components.Add(this.keyboardManager);

            this.mouseManager = new MouseManager(this, isMouseVisible);
            //centre the mouse otherwise the movement for 1st person camera will be unpredictable
            this.mouseManager.SetPosition(this.screenCentre);
            Components.Add(this.mouseManager);

            bool bDebugMode = false;
            this.objectManager = new ObjectManager(this, 10, 10, bDebugMode);
            this.objectManager.DrawOrder = 1;
            Components.Add(this.objectManager);

            //sound
            this.soundManager = new SoundManager(this,
                 "Content\\Assets\\Audio\\AtlanticDrift.xgs",
                 "Content\\Assets\\Audio\\WaveBank.xwb",
                "Content\\Assets\\Audio\\SoundBank.xsb");
            Components.Add(this.soundManager);


            this.uiManager = new UIManager(this, 10, 10);
            this.uiManager.DrawOrder = 2; //always draw after object manager(1)
            Components.Add(this.uiManager);

            Texture2D[] menuTexturesArray = {
                                                this.textureDictionary["mainmenu"],
                                                this.textureDictionary["audiomenu"],
                                                 this.textureDictionary["controlsmenu"],
                                                this.textureDictionary["exitmenu"]
                                            };

            this.menuManager = new MenuManager(this, menuTexturesArray, this.fontDictionary["menu"], MenuData.MenuTexturePadding, MenuData.MenuTextureColor);
            this.menuManager.DrawOrder = 3; //always draw after ui manager(2)
            Components.Add(this.menuManager);
        }

        private void InitializeEffects()
        {
            this.primitiveEffect = new BasicEffect(graphics.GraphicsDevice);
            this.primitiveEffect.VertexColorEnabled = true;

            this.texturedPrimitiveEffect = new BasicEffect(graphics.GraphicsDevice);
            this.texturedPrimitiveEffect.VertexColorEnabled = true;
            this.texturedPrimitiveEffect.TextureEnabled = true;
           

            this.texturedModelEffect = new BasicEffect(graphics.GraphicsDevice);
            // this.texturedModelEffect.VertexColorEnabled = true; 
            this.texturedModelEffect.TextureEnabled = true;
            this.texturedModelEffect.EnableDefaultLighting();

            //used for billboards
            //this.billboardEffect = Content.Load<Effect>("Assets/Effects/Billboard");

            //used for animated models
            //this.animatedModelEffect = Content.Load<Effect>("Assets/Effects/Animated");

            //this.texturedModelEffect.FogEnabled = true;
            //this.texturedModelEffect.FogColor = Color.LightSlateGray.ToVector3();
            //this.texturedModelEffect.FogStart = 20.75f;
            //this.texturedModelEffect.FogEnd = 30.25f;

            //this.texturedPrimitiveEffect.FogEnabled = true;


        }

        private void InitializeCameras()
        {
            string cameraLayoutName = "FirstPersonFullScreen";

            //use these for all our controllable and non-controllable clones
            PawnCamera3D clonePawnCamera = null;

            #region Camera Archetypes 
            //notice we clone the archetypes but never add controllers - we add controllers to the clones
            PawnCamera3D pawnCameraArchetype = new PawnCamera3D("pawn camera archetype",
                ObjectType.PawnCamera,
                    new Transform3D(new Vector3(-990, 50, -1740), -Vector3.UnitZ, Vector3.UnitY),
                        ProjectionParameters.standardBanter, this.graphics.GraphicsDevice.Viewport);

            Camera3D fixedCameraArchetype = new Camera3D("fixed camera archetype", ObjectType.FixedCamera);
            #endregion

            #region Collidable First Person Camera
            clonePawnCamera = (PawnCamera3D)pawnCameraArchetype.Clone();
            clonePawnCamera.ID = "collidable 1st person front";
            clonePawnCamera.AddController(new CollidableFirstPersonController(clonePawnCamera + " controller", clonePawnCamera, KeyData.MoveKeys, GameData.CameraMoveSpeed,
                GameData.CameraStrafeSpeed, GameData.CameraRotationSpeed, 2f, 40, 1, 1, 1, Vector3.Zero));
            this.cameraManager.Add(cameraLayoutName, clonePawnCamera);
            #endregion

            #region Non-collidable 1st Person Front Camera
            clonePawnCamera = (PawnCamera3D)pawnCameraArchetype.Clone();

            clonePawnCamera.ID = "non-collidable 1st person front";
            clonePawnCamera.Transform3D.Translation = new Vector3(-10, 50, 30);
            clonePawnCamera.AddController(new FirstPersonController(clonePawnCamera + " controller", clonePawnCamera, KeyData.MoveKeys, GameData.CameraMoveSpeed, GameData.CameraStrafeSpeed, GameData.CameraRotationSpeed));
            this.cameraManager.Add(cameraLayoutName, clonePawnCamera);
            #endregion

            //set the layout - we've only got one!
            this.cameraManager.SetCameraLayout("FirstPersonFullScreen");
        }
        #endregion
        
        #region Initialize Drawn Objects

        #region Non-Collidable
 
        private void InitializeNonCollidableModels()
        {
            
        }

        #endregion

        #region Collidable

        private void InitializePlayerObjects()
        {
            //PlayerObject playerObject = null;
            //Transform3D transform3D = null;

            //transform3D = new Transform3D(new Vector3(60, 5, -50),
            //    Vector3.Zero, new Vector3(1.5f, 1.5f, 5), //scale?
            //    Vector3.UnitZ, Vector3.UnitY);

            //playerObject = new PlayerObject("p1",
            //    ObjectType.Player, transform3D,
            //    this.texturedModelEffect,
            //    this.textureDictionary["slj"],
            //    this.modelDictionary["cube"],
            //    Color.White,
            //    1,
            //    KeyData.MoveKeysOther, 1, 5, 1.2f, 1, Vector3.Zero);
            //playerObject.Enable(false, 1);

            //this.objectManager.Add(playerObject);
        }

        private void InitializeStaticCollidableGround(int scale)
        {
            CollidableObject collidableObject = null;
            Transform3D transform3D = null;
            Texture2D texture = null;

            Model model = this.modelDictionary["LowPoly"];
            texture = this.textureDictionary["test"];
            transform3D = new Transform3D(new Vector3(-90, 580, 100), new Vector3(0, 0, 0),
                new Vector3(0.45f, 0.7f, 0.45f), Vector3.UnitX, Vector3.UnitY);

            collidableObject = new TriangleMeshObject("beach", ObjectType.CollidableGround, transform3D, this.texturedModelEffect, texture, model, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            collidableObject.Enable(true, 1); //change to false, see what happens.
            this.objectManager.Add(collidableObject);

            Model modelBarrier = this.modelDictionary["islandBarrier"];
            texture = this.textureDictionary["test"];
            transform3D = new Transform3D(new Vector3(-90, 0, 100), new Vector3(0, 0, 0),
                new Vector3(0.45f, 0.7f, 0.45f), Vector3.UnitX, Vector3.UnitY);

            collidableObject = new TriangleMeshObject("beach", ObjectType.CollidableGround, transform3D, this.texturedModelEffect, texture, modelBarrier, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            collidableObject.Enable(true, 1); //change to false, see what happens.
            this.objectManager.Add(collidableObject);

            Model model1 = this.modelDictionary["volcano"];
            texture = this.textureDictionary["sand"];
            transform3D = new Transform3D(new Vector3(-850, -40, 1550), new Vector3(0, 0, 0),
                new Vector3(3, 3, 3), Vector3.UnitX, Vector3.UnitY);

            collidableObject = new TriangleMeshObject("volcano", ObjectType.CollidableGround, transform3D, this.texturedModelEffect, texture, model1, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            collidableObject.Enable(true, 1); //change to false, see what happens.
            this.objectManager.Add(collidableObject);

            Model model2 = this.modelDictionary["box"];
            texture = this.textureDictionary["water"];
            transform3D = new Transform3D(new Vector3(0, -115, 0), new Vector3(0, 0, 0),
                new Vector3(1000, 10, 1000), Vector3.UnitX, Vector3.UnitY);

            collidableObject = new TriangleMeshObject("water", ObjectType.CollidableGround, transform3D, this.texturedModelEffect, texture, model2, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            collidableObject.Enable(true, 1); //change to false, see what happens.
            this.objectManager.Add(collidableObject);
        }

        private void InitializeSkyBox(int scale)
        {
            VertexPositionColorTexture[] vertices = VertexFactory.GetTextureQuadVertices();

            Transform3D transform = null;
            TexturedPrimitiveObject texturedPrimitive = null, clone = null;
            IVertexData vertexData = null;
            int halfScale = scale / 2;

            //back
            transform = new Transform3D(new Vector3(0, 0, -halfScale), new Vector3(0, 0, 0), scale * Vector3.One, Vector3.UnitZ, Vector3.UnitY);
            vertexData = this.vertexDictionary["texturedquad"];
            texturedPrimitive = new TexturedPrimitiveObject("sky", ObjectType.Decorator,
            transform, vertexData, this.texturedPrimitiveEffect, Color.White, 1, this.textureDictionary["skybox_back"]);
            this.objectManager.Add(texturedPrimitive);

            //top
            clone = (TexturedPrimitiveObject)texturedPrimitive.Clone();
            clone.Texture = this.textureDictionary["skybox_sky"];
            clone.Transform3D.Translation = new Vector3(0, halfScale, 0);
            clone.Transform3D.Rotation = new Vector3(90, -90, 0);
            this.objectManager.Add(clone);

            //left
            clone = (TexturedPrimitiveObject)texturedPrimitive.Clone();
            clone.Texture = this.textureDictionary["skybox_left"];
            clone.Transform3D.Translation = new Vector3(-halfScale, 0, 0);
            clone.Transform3D.Rotation = new Vector3(0, 90, 0);
            this.objectManager.Add(clone);

            //right
            clone = (TexturedPrimitiveObject)texturedPrimitive.Clone();
            clone.Texture = this.textureDictionary["skybox_right"];
            clone.Transform3D.Translation = new Vector3(halfScale, 0, 0);
            clone.Transform3D.Rotation = new Vector3(0, -90, 0);
            this.objectManager.Add(clone);

            //front
            clone = (TexturedPrimitiveObject)texturedPrimitive.Clone();
            clone.Texture = this.textureDictionary["skybox_front"];
            clone.Transform3D.Translation = new Vector3(0, 0, halfScale);
            clone.Transform3D.Rotation = new Vector3(0, 180, 0);
            this.objectManager.Add(clone);

            //water
            //clone = (TexturedPrimitiveObject)texturedPrimitive.Clone();
            //clone.Texture = this.textureDictionary["water"];
            //clone.Transform3D.Translation = new Vector3(0, 0, 0);
            //clone.Transform3D.Rotation = new Vector3(-90, -90, 0);
            //this.objectManager.Add(clone);
        }

        private void InitializeCollidableObjects()
        {
            #region Vars

            CollidableObject chest = null;
            CollidableObject tree = null;
            CollidableObject treeLeaf = null;
            CollidableObject rock = null;
            CollidableObject radio = null;
            CollidableObject foliage = null;
            Transform3D transform3D = null;
            Texture2D texture = null;

            #endregion

            #region Chests

            Model puzzleModel = this.modelDictionary["puzzleChest"];
            texture = this.textureDictionary["chestTex"];
            transform3D = new Transform3D(new Vector3(-150, 30, -1000), new Vector3(0, 45, 0),
                new Vector3(0.25f, 0.25f, 0.25f), Vector3.UnitX, Vector3.UnitY);

            chest = new TriangleMeshObject("treasureChest3", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, puzzleModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            chest.Enable(true, 1); //change to false, see what happens.
            chest.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(chest);

          
            transform3D = new Transform3D(new Vector3(-100, 30, -920), new Vector3(0, 0, 0),
                new Vector3(0.25f, 0.25f, 0.25f), Vector3.UnitX, Vector3.UnitY);

            chest = new TriangleMeshObject("treasureChest1", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, puzzleModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            chest.Enable(true, 1); //change to false, see what happens.
            chest.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(chest);

            transform3D = new Transform3D(new Vector3(-200, 30, -1111), new Vector3(0, 20, 0),
                new Vector3(0.25f, 0.25f, 0.25f), Vector3.UnitX, Vector3.UnitY);

            chest = new TriangleMeshObject("treasureChest2", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, puzzleModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            chest.Enable(true, 1); //change to false, see what happens.
            chest.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(chest);

            transform3D = new Transform3D(new Vector3(-220, 30, -900), new Vector3(0, 20, 0),
                new Vector3(0.25f, 0.25f, 0.25f), Vector3.UnitX, Vector3.UnitY);

            chest = new TriangleMeshObject("treasureChestKey", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, puzzleModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            chest.Enable(true, 1); //change to false, see what happens.
            chest.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(chest);

            transform3D = new Transform3D(new Vector3(-100, 30, -1000), new Vector3(0, 60, 0),
                new Vector3(0.25f, 0.25f, 0.25f), Vector3.UnitX, Vector3.UnitY);

            chest = new TriangleMeshObject("treasureChest4", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, puzzleModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            chest.Enable(true, 1); //change to false, see what happens.
            chest.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(chest);

            transform3D = new Transform3D(new Vector3(-200, 30, -1000), new Vector3(0, 60, 0),
                new Vector3(0.25f, 0.25f, 0.25f), Vector3.UnitX, Vector3.UnitY);

            chest = new TriangleMeshObject("treasureChest5", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, puzzleModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            chest.Enable(true, 1); //change to false, see what happens.
            chest.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(chest);

            #endregion

            #region Rocks

            Model rockModel = this.modelDictionary["rock"];
            texture = this.textureDictionary["rockTex"];
            transform3D = new Transform3D(new Vector3(10, 0, 10), new Vector3(0, 0, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            rock = new TriangleMeshObject("rock", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, rockModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            rock.Enable(true, 1); //change to false, see what happens.
            rock.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(rock);

            #endregion

            #region Radio

            Model radioModel = this.modelDictionary["radio"];
            texture = this.textureDictionary["checkerboard"];
            transform3D = new Transform3D(new Vector3(-220, 30, -900), new Vector3(0, 20, 0),
                new Vector3(0.1f, 0.1f, 0.1f), Vector3.UnitX, Vector3.UnitY);

            radio = new TriangleMeshObject("radio", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, radioModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            radio.Enable(true, 1); //change to false, see what happens.
            radio.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(radio);

            #endregion

            #region Foliage

            #region Barrier
            Model foliageModel = this.modelDictionary["foliage"];
            texture = this.textureDictionary["checkerboard"];
            transform3D = new Transform3D(new Vector3(-737, 20, -413), new Vector3(0, 0, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            foliage = new TriangleMeshObject("foliage", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, foliageModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            foliage.Enable(true, 1); //change to false, see what happens.
            foliage.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(foliage);

            transform3D = new Transform3D(new Vector3(-580, 20, -386), new Vector3(0, 0, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            foliage = new TriangleMeshObject("foliage", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, foliageModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            foliage.Enable(true, 1); //change to false, see what happens.
            foliage.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(foliage);

            transform3D = new Transform3D(new Vector3(-463, 20, -364), new Vector3(0, 0, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            foliage = new TriangleMeshObject("foliage", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, foliageModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            foliage.Enable(true, 1); //change to false, see what happens.
            foliage.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(foliage);

            transform3D = new Transform3D(new Vector3(-337, 20, -316), new Vector3(0, 0, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            foliage = new TriangleMeshObject("foliage", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, foliageModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            foliage.Enable(true, 1); //change to false, see what happens.
            foliage.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(foliage);

            transform3D = new Transform3D(new Vector3(-208, 20, -232), new Vector3(0, 0, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            foliage = new TriangleMeshObject("foliage", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, foliageModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            foliage.Enable(true, 1); //change to false, see what happens.
            foliage.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(foliage);

            transform3D = new Transform3D(new Vector3(-83, 20, -156), new Vector3(0, 0, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            foliage = new TriangleMeshObject("foliage", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, foliageModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            foliage.Enable(true, 1); //change to false, see what happens.
            foliage.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(foliage);

            transform3D = new Transform3D(new Vector3(111, 20, -125), new Vector3(0, 0, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            foliage = new TriangleMeshObject("foliage", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, foliageModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            foliage.Enable(true, 1); //change to false, see what happens.
            foliage.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(foliage);

            transform3D = new Transform3D(new Vector3(257, 20, -86), new Vector3(0, 0, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            foliage = new TriangleMeshObject("foliage", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, foliageModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            foliage.Enable(true, 1); //change to false, see what happens.
            foliage.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(foliage);

            transform3D = new Transform3D(new Vector3(389, 20, 5), new Vector3(0, 0, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            foliage = new TriangleMeshObject("foliage", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, foliageModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            foliage.Enable(true, 1); //change to false, see what happens.
            foliage.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(foliage);

            transform3D = new Transform3D(new Vector3(479, 20, 104), new Vector3(0, 0, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            foliage = new TriangleMeshObject("foliage", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, foliageModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            foliage.Enable(true, 1); //change to false, see what happens.
            foliage.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(foliage);

            transform3D = new Transform3D(new Vector3(562, 20, 221), new Vector3(0, 0, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            foliage = new TriangleMeshObject("foliage", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, foliageModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            foliage.Enable(true, 1); //change to false, see what happens.
            foliage.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(foliage);

            transform3D = new Transform3D(new Vector3(644, 20, 352), new Vector3(0, 0, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            foliage = new TriangleMeshObject("foliage", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, foliageModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            foliage.Enable(true, 1); //change to false, see what happens.
            foliage.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(foliage);

            #endregion

            #region Trees
            Model treeModel = this.modelDictionary["tree"];
            texture = this.textureDictionary["checkerboard"];
            transform3D = new Transform3D(new Vector3(-912, 90, -1611), new Vector3(0, 45, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            tree = new TriangleMeshObject("tree", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, treeModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            tree.Enable(true, 1); //change to false, see what happens.
            tree.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(tree);

            Model treeLeafModel = this.modelDictionary["treeLeaf"];
            texture = this.textureDictionary["checkerboard"];
            transform3D = new Transform3D(new Vector3(-912, 90, -1611), new Vector3(0, 45, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            treeLeaf = new TriangleMeshObject("treeLeaf", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, treeLeafModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            treeLeaf.Enable(true, 1); //change to false, see what happens.
            treeLeaf.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(treeLeaf);



            transform3D = new Transform3D(new Vector3(-840, 90, -1750), new Vector3(0, 260, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            tree = new TriangleMeshObject("tree", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, treeModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            tree.Enable(true, 1); //change to false, see what happens.
            tree.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(tree);

            transform3D = new Transform3D(new Vector3(-840, 90, -1750), new Vector3(0, 260, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            treeLeaf = new TriangleMeshObject("treeLeaf", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, treeLeafModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            treeLeaf.Enable(true, 1); //change to false, see what happens.
            treeLeaf.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(treeLeaf);



            transform3D = new Transform3D(new Vector3(-840, 90, -1750), new Vector3(0, 260, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            tree = new TriangleMeshObject("tree", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, treeModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            tree.Enable(true, 1); //change to false, see what happens.
            tree.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(tree);

            transform3D = new Transform3D(new Vector3(-840, 90, -1750), new Vector3(0, 260, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            treeLeaf = new TriangleMeshObject("treeLeaf", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, treeLeafModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            treeLeaf.Enable(true, 1); //change to false, see what happens.
            treeLeaf.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(treeLeaf);
            #endregion

            #endregion
        }

        private void InitializeNonCollidablePrimitives()
        {
            //Origin
            Transform3D transform = null;
            PrimitiveObject primitiveObject = null;
            IVertexData vertexData = null;

            //origin helper
            transform = new Transform3D(new Vector3(0, 5, -10), Vector3.Zero, 4 * Vector3.One, Vector3.UnitX, Vector3.UnitY);
            vertexData = this.vertexDictionary["origin"];
            primitiveObject = new PrimitiveObject("origin", ObjectType.Helper, transform, vertexData, this.primitiveEffect, Color.White, 1);
            this.objectManager.Add(primitiveObject);
        }

        #endregion

        #endregion

        #region Update & Draw

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            angle += 0.01f;
            listener.Position = this.cameraManager.ActiveCamera.Transform3D.Translation;
            emitter.Position = CalculateLocation(angle, distance);

            
            demoSoundManager(emitter);
            demoCameraLayout();

            this.soundManager.Update(gameTime);
            

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            this.graphics.GraphicsDevice.Viewport = this.cameraManager.ActiveCamera.Viewport;
            base.Draw(gameTime);

            if (this.menuManager.Pause)
                drawDebugInfo();
        }

        #endregion

        #region demo

        private void demoSoundManager(AudioEmitter emitter)
        {

            if (this.menuManager.Pause)
            {
                #region In-Game
                this.soundManager.Play3DCue("ocean", emitter);
                this.soundManager.Resume3DCue("ocean");
                #endregion

                #region Menu
                this.soundManager.Pause3DCue("menu");
                #endregion

            }
            if (!this.menuManager.Pause)
            {
                #region Menu
                this.soundManager.Play3DCue("menu", emitter);
                this.soundManager.Resume3DCue("menu");
                #endregion

                #region In-Game
                this.soundManager.Pause3DCue("ocean");
                #endregion
            }
            
        }

        private Vector3 CalculateLocation(float angle, float distance)
        {
            return new Vector3(
                (float)Math.Cos(angle) * distance,
                0,
                (float)Math.Sin(angle) * distance);
        }

        private void demoCameraLayout()
        {
            if (this.keyboardManager.IsFirstKeyPress(Keys.F1))
                this.cameraManager.CycleCamera();
        }

        Vector2 positionOffset = new Vector2(0, 25);
        Color debugColor = Color.Red;
        SpriteFont debugFont = null;

        private void drawDebugInfo()
        {
            //draw debug text after base.Draw() otherwise it will be behind the scene!
            if (debugFont == null)
                debugFont = this.fontDictionary["debug"];

            Vector2 debugPosition = new Vector2(20, 20);

            this.spriteBatch.Begin();
            this.spriteBatch.DrawString(debugFont, "ID:         " + this.cameraManager.ActiveCamera.ID, debugPosition, debugColor);
            debugPosition += positionOffset;

            this.spriteBatch.DrawString(debugFont, "Object Type:" + this.cameraManager.ActiveCamera.ObjectType, debugPosition, debugColor);
            debugPosition += positionOffset;

            this.spriteBatch.DrawString(debugFont, "Translation:" + MathUtility.Round(this.cameraManager.ActiveCamera.Transform3D.Translation, 1), debugPosition, debugColor);
            debugPosition += positionOffset;

            this.spriteBatch.DrawString(debugFont, "Look:       " + MathUtility.Round(this.cameraManager.ActiveCamera.Transform3D.Look, 1), debugPosition, debugColor);
            debugPosition += positionOffset;

            this.spriteBatch.DrawString(debugFont, "Up:         " + MathUtility.Round(this.cameraManager.ActiveCamera.Transform3D.Up, 1), debugPosition, debugColor);
            debugPosition += positionOffset;

            this.spriteBatch.DrawString(debugFont, "F1 - cycle cameras, WASD - move pawn camera, Space - Jump(1st person pawn only)", debugPosition, debugColor);
            this.spriteBatch.End();

        }

        #endregion

    }
}
