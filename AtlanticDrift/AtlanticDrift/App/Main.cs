using UDPLibrary;
using JigLibX.Collision;
using JigLibX.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
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
                Exit();
            else if (eventData.EventType == EventType.OnRestart)
                this.LoadGame();
        }
        #endregion

        #region Load Assets

        private void LoadFonts()
        {

            this.fontDictionary.Add("menu", Content.Load<SpriteFont>("Assets/Fonts/menu"));

        }

        private void LoadModels()
        {
            this.modelDictionary.Add("box", Content.Load<Model>("Assets/Models/box"));

            this.modelDictionary.Add("LowPoly", Content.Load<Model>("Assets/Models/LowPolyIdea"));

            this.modelDictionary.Add("foliage", Content.Load<Model>("Assets/Models/AnotherFool"));

            //this.modelDictionary.Add("tree", Content.Load<Model>("Assets/Models/PalmTree"));

            this.modelDictionary.Add("puzzleChest", Content.Load<Model>("Assets/Models/chest"));

            this.modelDictionary.Add("radio", Content.Load<Model>("Assets/Models/chest"));

            this.modelDictionary.Add("volcano", Content.Load<Model>("Assets/Models/Volcano"));

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

            bool bDebugMode = true;
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

            //used for billboards
            //this.billboardEffect = Content.Load<Effect>("Assets/Effects/Billboard");

            //used for animated models
            //this.animatedModelEffect = Content.Load<Effect>("Assets/Effects/Animated");
        }

        private void InitializeCameras()
        {
            string cameraLayoutName = "FirstPersonFullScreen";

            //use these for all our controllable and non-controllable clones
            PawnCamera3D clonePawnCamera = null;
            Camera3D cloneFixedCamera = null;

            #region Camera Archetypes 
            //notice we clone the archetypes but never add controllers - we add controllers to the clones
            PawnCamera3D pawnCameraArchetype = new PawnCamera3D("pawn camera archetype",
                ObjectType.PawnCamera,
                    new Transform3D(new Vector3(0, 40, 50), -Vector3.UnitZ, Vector3.UnitY),
                        ProjectionParameters.standardBanter, this.graphics.GraphicsDevice.Viewport);

            Camera3D fixedCameraArchetype = new Camera3D("fixed camera archetype", ObjectType.FixedCamera);
            #endregion

            #region Collidable First Person Camera
            clonePawnCamera = (PawnCamera3D)pawnCameraArchetype.Clone();
            clonePawnCamera.ID = "collidable 1st person front";
            clonePawnCamera.AddController(new CollidableFirstPersonController(clonePawnCamera + " controller", clonePawnCamera, KeyData.MoveKeys, GameData.CameraMoveSpeed,
                GameData.CameraStrafeSpeed, GameData.CameraRotationSpeed, 2f, 5, 1, 1, 1, Vector3.Zero));
            this.cameraManager.Add(cameraLayoutName, clonePawnCamera);
            #endregion

            #region Non-collidable 1st Person Front Camera
            clonePawnCamera = (PawnCamera3D)pawnCameraArchetype.Clone();

            clonePawnCamera.ID = "non-collidable 1st person front";
            clonePawnCamera.Transform3D.Translation = new Vector3(-10, 0, 30);
            clonePawnCamera.AddController(new FirstPersonController(clonePawnCamera + " controller", clonePawnCamera, KeyData.MoveKeys, GameData.CameraMoveSpeed, GameData.CameraStrafeSpeed, GameData.CameraRotationSpeed));
            this.cameraManager.Add(cameraLayoutName, clonePawnCamera);
            #endregion

            #region Non-collidable Fixed (i.e. no controller) Left Camera
            cloneFixedCamera = (Camera3D)fixedCameraArchetype.Clone();

            cloneFixedCamera.ID = "non-collidable front left fixed";
            cloneFixedCamera.Transform3D.Translation = new Vector3(-50, 5, 0); //on -ve X-axis 
            cloneFixedCamera.Transform3D.Look = Vector3.UnitX; //looking at origin
            cloneFixedCamera.Transform3D.Up = Vector3.UnitY;
            this.cameraManager.Add(cameraLayoutName, cloneFixedCamera);
            #endregion

            #region Non-collidable Fixed (i.e. no controller) Top Camera
            cloneFixedCamera = (Camera3D)fixedCameraArchetype.Clone();

            cloneFixedCamera.ID = "non-collidable front top fixed";
            cloneFixedCamera.Transform3D.Translation = new Vector3(0, 50, 0); //on +ve Y-axis 
            cloneFixedCamera.Transform3D.Look = -Vector3.UnitY; //looking down at origin
            cloneFixedCamera.Transform3D.Up = -Vector3.UnitZ;
            this.cameraManager.Add(cameraLayoutName, cloneFixedCamera);
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
            texture = this.textureDictionary["islandTex"];
            transform3D = new Transform3D(new Vector3(-90, 580, 100), new Vector3(0, 0, 0),
                new Vector3(0.7f, 0.7f, 0.7f), Vector3.UnitX, Vector3.UnitY);

            collidableObject = new TriangleMeshObject("beach", ObjectType.CollidableGround, transform3D, this.texturedModelEffect, texture, model, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            collidableObject.Enable(true, 1); //change to false, see what happens.
            this.objectManager.Add(collidableObject);

            Model model1 = this.modelDictionary["volcano"];
            texture = this.textureDictionary["sand"];
            transform3D = new Transform3D(new Vector3(-1000, -40, 2400), new Vector3(0, 0, 0),
                new Vector3(4, 3, 4), Vector3.UnitX, Vector3.UnitY);

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
            CollidableObject rock = null;
            CollidableObject radio = null;
            CollidableObject foliage = null;
            Transform3D transform3D = null;
            Texture2D texture = null;

            #endregion

            #region Chests

            Model puzzleModel = this.modelDictionary["puzzleChest"];
            texture = this.textureDictionary["chestTex"];
            transform3D = new Transform3D(new Vector3(10, 50, 10), new Vector3(0, 0, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            chest = new TriangleMeshObject("treasureChest", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, puzzleModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            chest.Enable(true, 1); //change to false, see what happens.
            chest.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(chest);

            #endregion

            #region Trees

            //Model treeModel = this.modelDictionary["tree"];
            //texture = this.textureDictionary["palmTreeTex"];
            //transform3D = new Transform3D(new Vector3(10, 50, 10), new Vector3(0, 0, 0),
            //    new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            //tree = new TriangleMeshObject("palmTree", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, treeModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            //tree.Enable(true, 1); //change to false, see what happens.
            //tree.ObjectType = ObjectType.CollidableProp;
            //this.objectManager.Add(tree);

            #endregion

            #region Rocks

            //Model rockModel = this.modelDictionary["rock"];
            //texture = this.textureDictionary["rockTex"];
            //transform3D = new Transform3D(new Vector3(10, 50, 10), new Vector3(0, 0, 0),
            //    new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            //rock = new TriangleMeshObject("rock", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, rockModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            //rock.Enable(true, 1); //change to false, see what happens.
            //rock.ObjectType = ObjectType.CollidableProp;
            //this.objectManager.Add(rock);

            #endregion

            #region Radio

            Model radioModel = this.modelDictionary["radio"];
            texture = this.textureDictionary["checkerboard"];
            transform3D = new Transform3D(new Vector3(10, 50, 10), new Vector3(0, 0, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            radio = new TriangleMeshObject("radio", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, radioModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            radio.Enable(true, 1); //change to false, see what happens.
            radio.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(radio);

            #endregion

            #region Foliage

            Model foliageModel = this.modelDictionary["foliage"];
            texture = this.textureDictionary["checkerboard"];
            transform3D = new Transform3D(new Vector3(10, 50, 10), new Vector3(0, 0, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            foliage = new TriangleMeshObject("foliage", ObjectType.CollidableProp, transform3D, this.texturedModelEffect, texture, foliageModel, Color.White, 1, new MaterialProperties(0.8f, 0.8f, 0.7f));
            foliage.Enable(true, 1); //change to false, see what happens.
            foliage.ObjectType = ObjectType.CollidableProp;
            this.objectManager.Add(foliage);

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

            distance += 0.1f;
            angle += 0.01f;  // rotate the emitter around a little bit
            listener.Position = Vector3.Zero;  // the listener just stays at the origin the whole time
            emitter.Position = CalculateLocation(angle, distance);  // calculate the location of the emitter again

            demoSoundManager(emitter);

            this.soundManager.Update(gameTime);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            this.graphics.GraphicsDevice.Viewport = this.cameraManager.ActiveCamera.Viewport;
            base.Draw(gameTime);

            //if (this.menuManager.Pause)
            //    drawDebugInfo();
        }

        #endregion

        #region demo

        private void demoSoundManager(AudioEmitter emitter)
        {
            //Notice that the cue name is taken from inside SoundBank1
            //To see the sound bank contents open the file Demo2DSound.xap using XACT3 found through the start menu on Windows

            this.soundManager.Play3DCue("ocean", emitter);
            
        }

        private Vector3 CalculateLocation(float angle, float distance)
        {
            return new Vector3(
                (float)Math.Cos(angle) * distance,
                0,
                (float)Math.Sin(angle) * distance);
        }

        #endregion























        //#region Initialisation

        ////protected override void Initialize()
        ////{
        ////    int width = 1024, height = 768;
        ////    int worldScale = 10000;

        ////    InitializeStaticReferences();
        ////    InitializeGraphics(width, height);
        ////    //InitializeEffects();

        ////    // InitializeDictionaries();

        ////    LoadFonts();
        ////    LoadModels();
        ////    LoadTextures();
        ////    LoadVertices();
        ////    LoadPrimitiveArchetypes();

        ////    InitializeManagers();

        ////    InitializeStaticCollidableGround(worldScale);
        ////    InitializeCollidableObjects();
        ////    InitializeNonCollidableModels();
        ////    InitializeSkyBox(worldScale);

        ////    //InitializeCameraTracks();
        ////    InitializeCameraRails();
        ////    InitializeCamera();


        ////    base.Initialize();
        ////}

        ////private void InitializeDictionaries()
        ////{
        ////    this.textureDictionary = new GenericDictionary<string, Texture2D>("texture dictionary");

        ////    this.vertexDictionary = new GenericDictionary<string, IVertexData>("vertex dictionary");

        ////    this.modelDictionary = new GenericDictionary<string, Model>("model dictionary");

        ////    this.fontDictionary = new GenericDictionary<string, SpriteFont>("font dictionary");
        ////}

        ////private void InitializeManagers()
        ////{
        ////    //CD-CR
        ////    this.physicsManager = new PhysicsManager(this);
        ////    Components.Add(physicsManager);

        ////    bool bDebugMode = true; //show wireframe CD-CR surfaces
        ////    this.objectManager = new ObjectManager(this, "gameObjects", bDebugMode);
        ////    Components.Add(this.objectManager);

        ////    this.mouseManager = new MouseManager(this, true);
        ////    this.mouseManager.SetPosition(this.ScreenCentre);
        ////    Components.Add(this.mouseManager);

        ////    this.keyboardManager = new KeyboardManager(this);
        ////    Components.Add(this.KeyboardManager);

        ////    this.cameraManager = new CameraManager(this);
        ////    Components.Add(this.cameraManager);

        ////    Texture2D[] menuTextures = { this.textureDictionary["mainmenu"],
        ////        this.textureDictionary["audio"],
        ////        this.textureDictionary["controlsmenu"],
        ////        this.textureDictionary["exitmenu"] };

        ////    this.menuManager = new MenuManager(this, menuTextures, this.fontDictionary["menu"], Integer2.Zero, Color.White);

        ////    Components.Add(this.menuManager);

        ////}

        ////private void InitializeStaticReferences()
        ////{
        ////    Actor.game = this;
        ////    Camera3D.game = this;
        ////    Controller.game = this;
        ////}

        ////private void InitializeGraphics(int width, int height)
        ////{
        ////    this.graphics.PreferredBackBufferWidth = width;
        ////    this.graphics.PreferredBackBufferHeight = height;
        ////    this.graphics.ApplyChanges();

        ////    //or we can set full screen
        ////    //this.graphics.IsFullScreen = true;
        ////    // this.graphics.ApplyChanges();

        ////    //records screen centre point - used by mouse to see how much the mouse pointer has moved
        ////    this.screenCentre = new Vector2(this.graphics.PreferredBackBufferWidth / 2.0f,
        ////        this.graphics.PreferredBackBufferHeight / 2.0f);
        ////}

        ////private void InitializeEffects()
        ////{
        ////    this.wireframeEffect = new BasicEffect(graphics.GraphicsDevice);
        ////    this.wireframeEffect.VertexColorEnabled = true;

        ////    this.texturedPrimitiveEffect = new BasicEffect(graphics.GraphicsDevice);
        ////    this.texturedPrimitiveEffect.VertexColorEnabled = true;
        ////    this.texturedPrimitiveEffect.TextureEnabled = true;

        ////    this.texturedModelEffect = new BasicEffect(graphics.GraphicsDevice);
        ////    //    this.texturedModelEffect.VertexColorEnabled = true;
        ////    this.texturedModelEffect.TextureEnabled = true;

        ////}

        //#endregion

        //#region Collidable & Non-Collidable

        //#region Non-Collidable

        ////private void InitializeNonCollidableModels()    
        ////{
        ////    //to do...
        ////    Transform3D transform = new Transform3D(new Vector3(400, 5, 450),
        ////            new Vector3(0, 0, 0), new Vector3(1, 1, 1),
        ////            Vector3.UnitX, Vector3.UnitY);

        ////    this.drivableModelObject = new ModelObject("box1",
        ////        ActorType.Pickup, transform,
        ////        this.texturedModelEffect, Color.White, 0.6f,
        ////        this.textureDictionary["checkerboard"],
        ////        this.modelDictionary["box"]);

        ////    this.drivableModelObject.AttachController(new DriveController("dc1", ControllerType.Drive,
        ////        AppData.PlayerMoveKeys, AppData.PlayerMoveSpeed,
        ////        AppData.PlayerStrafeSpeed, AppData.PlayerRotationSpeed));
        ////    this.objectManager.Add(drivableModelObject);
        ////}
        //#endregion

        //#region Collidable

        ////private void InitializeSkyBox(int worldScale)
        ////{
        ////    TexturedPrimitiveObject archTexturedPrimitiveObject = null, cloneTexturedPrimitiveObject = null;

        ////    #region Archetype
        ////    //we need to do an "as" typecast since the dictionary holds DrawnActor3D types
        ////    archTexturedPrimitiveObject = this.objectDictionary["textured_quad"] as TexturedPrimitiveObject;
        ////    archTexturedPrimitiveObject.Transform3D.Scale *= worldScale;
        ////    #endregion

        ////    #region Skybox
        ////    //back
        ////    cloneTexturedPrimitiveObject = (TexturedPrimitiveObject)archTexturedPrimitiveObject.Clone();
        ////    cloneTexturedPrimitiveObject.ID = "skybox_back";
        ////    cloneTexturedPrimitiveObject.Transform3D.Translation = new Vector3(0, 0, -worldScale / 2.0f);
        ////    cloneTexturedPrimitiveObject.Texture = this.textureDictionary["skybox_back"];
        ////    this.objectManager.Add(cloneTexturedPrimitiveObject);

        ////    //left
        ////    cloneTexturedPrimitiveObject = (TexturedPrimitiveObject)archTexturedPrimitiveObject.Clone();
        ////    cloneTexturedPrimitiveObject.ID = "skybox_left";
        ////    cloneTexturedPrimitiveObject.Transform3D.Translation = new Vector3(-worldScale / 2.0f, 0, 0);
        ////    cloneTexturedPrimitiveObject.Transform3D.Rotation = new Vector3(0, 90, 0);
        ////    cloneTexturedPrimitiveObject.Texture = this.textureDictionary["skybox_left"];
        ////    this.objectManager.Add(cloneTexturedPrimitiveObject);

        ////    //right
        ////    cloneTexturedPrimitiveObject = (TexturedPrimitiveObject)archTexturedPrimitiveObject.Clone();
        ////    cloneTexturedPrimitiveObject.ID = "skybox_right";
        ////    cloneTexturedPrimitiveObject.Transform3D.Translation = new Vector3(worldScale / 2.0f, 0, 0);
        ////    cloneTexturedPrimitiveObject.Transform3D.Rotation = new Vector3(0, -90, 0);
        ////    cloneTexturedPrimitiveObject.Texture = this.textureDictionary["skybox_right"];
        ////    this.objectManager.Add(cloneTexturedPrimitiveObject);

        ////    //front
        ////    cloneTexturedPrimitiveObject = (TexturedPrimitiveObject)archTexturedPrimitiveObject.Clone();
        ////    cloneTexturedPrimitiveObject.ID = "skybox_front";
        ////    cloneTexturedPrimitiveObject.Transform3D.Translation = new Vector3(0, 0, worldScale / 2.0f);
        ////    cloneTexturedPrimitiveObject.Transform3D.Rotation = new Vector3(0, 180, 0);
        ////    cloneTexturedPrimitiveObject.Texture = this.textureDictionary["skybox_front"];
        ////    this.objectManager.Add(cloneTexturedPrimitiveObject);

        ////    //top
        ////    cloneTexturedPrimitiveObject = (TexturedPrimitiveObject)archTexturedPrimitiveObject.Clone();
        ////    cloneTexturedPrimitiveObject.ID = "skybox_sky";
        ////    cloneTexturedPrimitiveObject.Transform3D.Translation = new Vector3(0, worldScale / 2.0f, 0);
        ////    cloneTexturedPrimitiveObject.Transform3D.Rotation = new Vector3(90, -90, 0);
        ////    cloneTexturedPrimitiveObject.Texture = this.textureDictionary["skybox_sky"];
        ////    this.objectManager.Add(cloneTexturedPrimitiveObject);

        ////    //water
        ////    cloneTexturedPrimitiveObject = (TexturedPrimitiveObject)archTexturedPrimitiveObject.Clone();
        ////    cloneTexturedPrimitiveObject.ID = "water";
        ////    cloneTexturedPrimitiveObject.Transform3D.Translation = new Vector3(0, 0, 0);
        ////    cloneTexturedPrimitiveObject.Transform3D.Rotation = new Vector3(90, 0, 0);
        ////    cloneTexturedPrimitiveObject.Texture = this.textureDictionary["water"];
        ////    this.objectManager.Add(cloneTexturedPrimitiveObject);
        ////    #endregion
        ////}

        ////private void InitializeCollidableObjects()
        ////{
        ////    #region Vars

        ////    CollidableObject chest = null;
        ////    CollidableObject tree = null;
        ////    CollidableObject rock = null;
        ////    Transform3D transform3D = null;
        ////    Texture2D texture = null;

        ////    #endregion

        ////    #region Chests

        ////    Model puzzleModel = this.modelDictionary["puzzleChest"];
        ////    texture = this.textureDictionary["chestTex"];
        ////    transform3D = new Transform3D(new Vector3(10, 50, 10), new Vector3(0, 0, 0),
        ////        new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

        ////    chest = new TriangleMeshObject("treasureChest", ActorType.CollidableProp, transform3D, this.texturedModelEffect, Color.White, 1, texture, puzzleModel, new MaterialProperties(0.8f, 0.8f, 0.7f));
        ////    chest.Enable(true, 1); //change to false, see what happens.
        ////    chest.ActorType = ActorType.CollidableProp;
        ////    this.objectManager.Add(chest);

        ////    #endregion

        ////    #region Trees

        ////    //Model treeModel = this.modelDictionary["tree"];
        ////    //texture = this.textureDictionary["palmTreeTex"];
        ////    //transform3D = new Transform3D(new Vector3(10, 50, 10), new Vector3(0, 0, 0),
        ////    //    new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

        ////    //tree = new TriangleMeshObject("palmTree", ActorType.CollidableProp, transform3D, this.texturedModelEffect, Color.White, 1, texture, treeModel, new MaterialProperties(0.8f, 0.8f, 0.7f));
        ////    //tree.Enable(true, 1); //change to false, see what happens.
        ////    //tree.ActorType = ActorType.CollidableProp;
        ////    //this.objectManager.Add(tree);

        ////    #endregion

        ////    #region Rocks

        ////    //Model rockModel = this.modelDictionary["rock"];
        ////    //texture = this.textureDictionary["rockTex"];
        ////    //transform3D = new Transform3D(new Vector3(10, 50, 10), new Vector3(0, 0, 0),
        ////    //    new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

        ////    //rock = new TriangleMeshObject("rock", ActorType.CollidableProp, transform3D, this.texturedModelEffect, Color.White, 1, texture, rockModel, new MaterialProperties(0.8f, 0.8f, 0.7f));
        ////    //rock.Enable(true, 1); //change to false, see what happens.
        ////    //rock.ActorType = ActorType.CollidableProp;
        ////    //this.objectManager.Add(rock);

        ////    #endregion

        ////}

        ////private void InitializeStaticCollidableGround(int scale)
        ////{
        ////    CollidableObject collidableObject = null;
        ////    Transform3D transform3D = null;
        ////    Texture2D texture = null;

        ////    Model model = this.modelDictionary["LowPoly"];
        ////    texture = this.textureDictionary["islandTex"];
        ////    transform3D = new Transform3D(new Vector3(-90, 580, 100), new Vector3(0, 0, 0),
        ////        new Vector3(0.7f, 0.7f, 0.7f), Vector3.UnitX, Vector3.UnitY);

        ////    collidableObject = new TriangleMeshObject("ground", ObjectType.CollidableGround, transform3D, this.texturedModelEffect, Color.White, 1, texture, model, new MaterialProperties(0.8f, 0.8f, 0.7f));
        ////    collidableObject.Enable(true, 1); //change to false, see what happens.
        ////    this.objectManager.Add(collidableObject);

        ////    Model model1 = this.modelDictionary["volcano"];
        ////    texture = this.textureDictionary["sand"];
        ////    transform3D = new Transform3D(new Vector3(-1000, -40, 2400), new Vector3(0, 0, 0),
        ////        new Vector3(4, 3, 4), Vector3.UnitX, Vector3.UnitY);

        ////    collidableObject = new TriangleMeshObject("volcano", ObjectType.CollidableGround, transform3D, this.texturedModelEffect, Color.White, 1, texture, model1, new MaterialProperties(0.8f, 0.8f, 0.7f));
        ////    collidableObject.Enable(true, 1); //change to false, see what happens.
        ////    this.objectManager.Add(collidableObject);


        ////}

        //#endregion

        //#endregion

        //#region Assets

        ////private void LoadFonts()
        ////{

        ////    this.fontDictionary.Add("menu", Content.Load<SpriteFont>("Assets/Fonts/menu"));

        ////}

        ////private void LoadModels()
        ////{
        ////    this.modelDictionary.Add("box", Content.Load<Model>("Assets/Models/box"));

        ////    this.modelDictionary.Add("LowPoly", Content.Load<Model>("Assets/Models/LowPolyIdea"));

        ////    this.modelDictionary.Add("foliage", Content.Load<Model>("Assets/Models/AnotherFool"));

        ////    //this.modelDictionary.Add("tree", Content.Load<Model>("Assets/Models/PalmTree"));

        ////    this.modelDictionary.Add("puzzleChest", Content.Load<Model>("Assets/Models/chest"));

        ////    this.modelDictionary.Add("radio", Content.Load<Model>("Assets/Models/chest"));

        ////    this.modelDictionary.Add("volcano", Content.Load<Model>("Assets/Models/Volcano"));

        ////}

        ////private void LoadTextures()
        ////{
        ////    this.textureDictionary.Add("checkerboard",
        ////        Content.Load<Texture2D>("Assets/Textures/Debug/checkerboard"));

        ////    this.textureDictionary.Add("sand",
        ////        Content.Load<Texture2D>("Assets/Textures/island/temp"));

        ////    this.textureDictionary.Add("water",
        ////        Content.Load<Texture2D>("Assets/Textures/island/water"));

        ////    this.textureDictionary.Add("islandTex",
        ////        Content.Load<Texture2D>("Assets/Textures/island/islandTex"));

        ////    this.textureDictionary.Add("chestTex",
        ////        Content.Load<Texture2D>("Assets/Textures/Models/chestTex"));

        ////    this.textureDictionary.Add("rockTex",
        ////        Content.Load<Texture2D>("Assets/Textures/Models/rockTex"));

        ////    #region Sky
        ////    this.textureDictionary.Add("skybox_back",
        ////        Content.Load<Texture2D>("Assets/Textures/Skybox/back"));
        ////    this.textureDictionary.Add("skybox_front",
        ////        Content.Load<Texture2D>("Assets/Textures/Skybox/front"));
        ////    this.textureDictionary.Add("skybox_left",
        ////        Content.Load<Texture2D>("Assets/Textures/Skybox/left"));
        ////    this.textureDictionary.Add("skybox_right",
        ////        Content.Load<Texture2D>("Assets/Textures/Skybox/right"));
        ////    this.textureDictionary.Add("skybox_sky",
        ////        Content.Load<Texture2D>("Assets/Textures/Skybox/sky"));
        ////    #endregion

        ////    #region other

        ////    this.textureDictionary.Add("controlsmenu",
        ////    Content.Load<Texture2D>("Assets/Textures/Menu/controlsmenu"));

        ////    this.textureDictionary.Add("exitmenu",
        ////    Content.Load<Texture2D>("Assets/Textures/Menu/exitmenu"));

        ////    this.textureDictionary.Add("exitmenuwithtrans",
        ////    Content.Load<Texture2D>("Assets/Textures/Menu/exitmenuwithtrans"));

        ////    this.textureDictionary.Add("mainmenu",
        ////    Content.Load<Texture2D>("Assets/Textures/Menu/mainmenu"));

        ////    this.textureDictionary.Add("audio",
        ////    Content.Load<Texture2D>("Assets/Textures/Menu/audiomenu"));

        ////    #endregion

        ////}

        ////private void LoadVertices()
        ////{
        ////    VertexPositionColor[] verticesPositionColor = null;
        ////    VertexPositionColorTexture[] verticesPositionColorTexture = null;
        ////    IVertexData vertexData = null;
        ////    float halfLength = 0.5f;

        ////    #region Textured Quad
        ////    verticesPositionColorTexture = new VertexPositionColorTexture[4];

        ////    //top left
        ////    verticesPositionColorTexture[0] = new VertexPositionColorTexture(
        ////        new Vector3(-halfLength, halfLength, 0), Color.White, new Vector2(0, 0));
        ////    //top right
        ////    verticesPositionColorTexture[1] = new VertexPositionColorTexture(
        ////    new Vector3(halfLength, halfLength, 0), Color.White, new Vector2(1, 0));
        ////    //bottom left
        ////    verticesPositionColorTexture[2] = new VertexPositionColorTexture(
        ////    new Vector3(-halfLength, -halfLength, 0), Color.White, new Vector2(0, 1));
        ////    //bottom right
        ////    verticesPositionColorTexture[3] = new VertexPositionColorTexture(
        ////    new Vector3(halfLength, -halfLength, 0), Color.White, new Vector2(1, 1));

        ////    vertexData = new VertexData<VertexPositionColorTexture>(verticesPositionColorTexture, Microsoft.Xna.Framework.Graphics.PrimitiveType.TriangleStrip, 2);
        ////    this.vertexDictionary.Add("textured_quad", vertexData);
        ////    #endregion

        ////    #region Textured Cube
        ////    verticesPositionColorTexture = new VertexPositionColorTexture[36];

        ////    Vector3 topLeftFront = new Vector3(-halfLength, halfLength, halfLength);
        ////    Vector3 topLeftBack = new Vector3(-halfLength, halfLength, -halfLength);
        ////    Vector3 topRightFront = new Vector3(halfLength, halfLength, halfLength);
        ////    Vector3 topRightBack = new Vector3(halfLength, halfLength, -halfLength);

        ////    Vector3 bottomLeftFront = new Vector3(-halfLength, -halfLength, halfLength);
        ////    Vector3 bottomLeftBack = new Vector3(-halfLength, -halfLength, -halfLength);
        ////    Vector3 bottomRightFront = new Vector3(halfLength, -halfLength, halfLength);
        ////    Vector3 bottomRightBack = new Vector3(halfLength, -halfLength, -halfLength);

        ////    //uv coordinates
        ////    Vector2 uvTopLeft = new Vector2(0, 0);
        ////    Vector2 uvTopRight = new Vector2(1, 0);
        ////    Vector2 uvBottomLeft = new Vector2(0, 1);
        ////    Vector2 uvBottomRight = new Vector2(1, 1);


        ////    //top - 1 polygon for the top
        ////    verticesPositionColorTexture[0] = new VertexPositionColorTexture(topLeftFront, Color.White, uvBottomLeft);
        ////    verticesPositionColorTexture[1] = new VertexPositionColorTexture(topLeftBack, Color.White, uvTopLeft);
        ////    verticesPositionColorTexture[2] = new VertexPositionColorTexture(topRightBack, Color.White, uvTopRight);

        ////    verticesPositionColorTexture[3] = new VertexPositionColorTexture(topLeftFront, Color.White, uvBottomLeft);
        ////    verticesPositionColorTexture[4] = new VertexPositionColorTexture(topRightBack, Color.White, uvTopRight);
        ////    verticesPositionColorTexture[5] = new VertexPositionColorTexture(topRightFront, Color.White, uvBottomRight);

        ////    //front
        ////    verticesPositionColorTexture[6] = new VertexPositionColorTexture(topLeftFront, Color.White, uvBottomLeft);
        ////    verticesPositionColorTexture[7] = new VertexPositionColorTexture(topRightFront, Color.White, uvBottomRight);
        ////    verticesPositionColorTexture[8] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvTopLeft);

        ////    verticesPositionColorTexture[9] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvTopLeft);
        ////    verticesPositionColorTexture[10] = new VertexPositionColorTexture(topRightFront, Color.White, uvBottomRight);
        ////    verticesPositionColorTexture[11] = new VertexPositionColorTexture(bottomRightFront, Color.White, uvTopRight);

        ////    //back
        ////    verticesPositionColorTexture[12] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);
        ////    verticesPositionColorTexture[13] = new VertexPositionColorTexture(topRightBack, Color.White, uvTopRight);
        ////    verticesPositionColorTexture[14] = new VertexPositionColorTexture(topLeftBack, Color.White, uvTopLeft);

        ////    verticesPositionColorTexture[15] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);
        ////    verticesPositionColorTexture[16] = new VertexPositionColorTexture(topLeftBack, Color.White, uvTopLeft);
        ////    verticesPositionColorTexture[17] = new VertexPositionColorTexture(bottomLeftBack, Color.White, uvBottomLeft);

        ////    //left 
        ////    verticesPositionColorTexture[18] = new VertexPositionColorTexture(topLeftBack, Color.White, uvTopLeft);
        ////    verticesPositionColorTexture[19] = new VertexPositionColorTexture(topLeftFront, Color.White, uvTopRight);
        ////    verticesPositionColorTexture[20] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvBottomRight);

        ////    verticesPositionColorTexture[21] = new VertexPositionColorTexture(bottomLeftBack, Color.White, uvBottomLeft);
        ////    verticesPositionColorTexture[22] = new VertexPositionColorTexture(topLeftBack, Color.White, uvTopLeft);
        ////    verticesPositionColorTexture[23] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvBottomRight);

        ////    //right
        ////    verticesPositionColorTexture[24] = new VertexPositionColorTexture(bottomRightFront, Color.White, uvBottomLeft);
        ////    verticesPositionColorTexture[25] = new VertexPositionColorTexture(topRightFront, Color.White, uvTopLeft);
        ////    verticesPositionColorTexture[26] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);

        ////    verticesPositionColorTexture[27] = new VertexPositionColorTexture(topRightFront, Color.White, uvTopLeft);
        ////    verticesPositionColorTexture[28] = new VertexPositionColorTexture(topRightBack, Color.White, uvTopRight);
        ////    verticesPositionColorTexture[29] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);

        ////    //bottom
        ////    verticesPositionColorTexture[30] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvTopLeft);
        ////    verticesPositionColorTexture[31] = new VertexPositionColorTexture(bottomRightFront, Color.White, uvTopRight);
        ////    verticesPositionColorTexture[32] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);

        ////    verticesPositionColorTexture[33] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvTopLeft);
        ////    verticesPositionColorTexture[34] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);
        ////    verticesPositionColorTexture[35] = new VertexPositionColorTexture(bottomLeftBack, Color.White, uvBottomLeft);

        ////    vertexData = new VertexData<VertexPositionColorTexture>(verticesPositionColorTexture, Microsoft.Xna.Framework.Graphics.PrimitiveType.TriangleList, 12);
        ////    this.vertexDictionary.Add("textured_cube", vertexData);
        ////    #endregion

        ////    #region Wireframe Origin Helper
        ////    verticesPositionColor = new VertexPositionColor[6];

        ////    //x-axis
        ////    verticesPositionColor[0] = new VertexPositionColor(new Vector3(-halfLength, 0, 0), Color.Red);
        ////    verticesPositionColor[1] = new VertexPositionColor(new Vector3(halfLength, 0, 0), Color.Red);
        ////    //y-axis
        ////    verticesPositionColor[2] = new VertexPositionColor(new Vector3(0, halfLength, 0), Color.Green);
        ////    verticesPositionColor[3] = new VertexPositionColor(new Vector3(0, -halfLength, 0), Color.Green);
        ////    //z-axis
        ////    verticesPositionColor[4] = new VertexPositionColor(new Vector3(0, 0, halfLength), Color.Blue);
        ////    verticesPositionColor[5] = new VertexPositionColor(new Vector3(0, 0, -halfLength), Color.Blue);

        ////    vertexData = new VertexData<VertexPositionColor>(verticesPositionColor, Microsoft.Xna.Framework.Graphics.PrimitiveType.LineList, 3);
        ////    this.vertexDictionary.Add("wireframe_origin_helper", vertexData);
        ////    #endregion

        ////    #region Wireframe Triangle
        ////    verticesPositionColor = new VertexPositionColor[3];

        ////    verticesPositionColor[0] = new VertexPositionColor(new Vector3(0, 1, 0), Color.Red);
        ////    verticesPositionColor[1] = new VertexPositionColor(new Vector3(1, 0, 0), Color.Green);
        ////    verticesPositionColor[2] = new VertexPositionColor(new Vector3(-1, 0, 0), Color.Blue);

        ////    vertexData = new VertexData<VertexPositionColor>(verticesPositionColor, Microsoft.Xna.Framework.Graphics.PrimitiveType.TriangleStrip, 1);
        ////    this.vertexDictionary.Add("wireframe_triangle", vertexData);
        ////    #endregion
        ////}

        ////private void LoadPrimitiveArchetypes()
        ////{
        ////    Transform3D transform = null;
        ////    TexturedPrimitiveObject texturedQuad = null;

        ////    #region Textured Quad Archetype
        ////    transform = new Transform3D(Vector3.Zero, Vector3.Zero, Vector3.One, Vector3.UnitZ, Vector3.UnitY);
        ////    texturedQuad = new TexturedPrimitiveObject("textured quad archetype", ActorType.Decorator,
        ////             transform, this.texturedPrimitiveEffect, this.vertexDictionary["textured_quad"],
        ////             this.textureDictionary["checkerboard"]); //or we can leave texture null since we will replace it later

        ////    this.objectDictionary.Add("textured_quad", texturedQuad);
        ////    #endregion
        ////}

        //#endregion

        //#region Camera

        ////private void InitializeCameraRails()
        ////{
        ////    RailParameters rail = null;

        ////    rail = new RailParameters("rail1", new Vector3(-100, 50, 100), new Vector3(100, 100, 100));
        ////    this.railDictionary.Add(rail.ID, rail);

        ////    rail = new RailParameters("rail2", new Vector3(100, 50, 100), new Vector3(100, 50, -100));
        ////    this.railDictionary.Add(rail.ID, rail);
        ////}

        ////private void InitializeCameraTracks()
        ////{
        ////    Transform3DCurve curve = null;

        ////    #region Curve1
        ////    curve = new Transform3DCurve(CurveLoopType.Oscillate);
        ////    curve.Add(new Vector3(0, 10, 200),
        ////            -Vector3.UnitZ, Vector3.UnitY, 0);

        ////    curve.Add(new Vector3(0, 10, 20),
        ////           -Vector3.UnitZ, Vector3.UnitX, 2);

        ////    this.curveDictionary.Add("room_action1", curve);
        ////    #endregion
        ////}

        ////private void InitializeCamera()
        ////{
        ////    Transform3D transform = null;
        ////    Camera3D camera = null;
        ////    string cameraLayout = "";

        ////    #region Layout 1x1
        ////    cameraLayout = "1x1";

        ////    #region First Person Camera
        ////    transform = new Transform3D(new Vector3(400, 50, 450), -Vector3.UnitZ, Vector3.UnitY);
        ////    camera = new Camera3D("Static", ActorType.Camera, transform,
        ////        ProjectionParameters.standardBanter,
        ////        new Viewport(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
        ////    camera.AttachController(new FirstPersonController("firstPersControl1",
        ////    ControllerType.FirstPerson, AppData.CameraMoveKeys,
        ////    AppData.CameraMoveSpeed, AppData.CameraStrafeSpeed, AppData.CameraRotationSpeed));

        ////    //add the new camera to the approriate K, V pair in the camera manager dictionary i.e. where key is "1x2"
        ////    this.cameraManager.Add(cameraLayout, camera);
        ////    #endregion
        ////    #endregion

        ////    #region Layout 1x1 Collidable
        ////    cameraLayout = "1x1 Collidable";

        ////    transform = new Transform3D(new Vector3(0, 100, 100), -Vector3.UnitZ, Vector3.UnitY);
        ////    camera = new Camera3D("Static", ActorType.Camera, transform,
        ////        ProjectionParameters.StandardMediumSixteenNine,
        ////        new Viewport(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
        ////    camera.AttachController(new CollidableFirstPersonController(
        ////        camera + " controller",
        ////        ControllerType.FirstPersonCollidable,
        ////        AppData.CameraMoveKeys, AppData.CollidableCameraMoveSpeed,
        ////        AppData.CollidableCameraStrafeSpeed, AppData.CollidableCameraRotationSpeed,
        ////        2f, 10, 1, 1, 1, Vector3.Zero, camera));

        ////    this.cameraManager.Add(cameraLayout, camera);
        ////    #endregion

        ////    //finally, set the active layout
        ////    this.cameraManager.SetActiveCameraLayout("1x1");

        ////}
        //#endregion

    }
}
