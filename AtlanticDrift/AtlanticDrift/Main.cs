using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JigLibX.Collision;
using JigLibX.Geometry;
using UDPLibrary;

namespace AtlanticDrift
{
    public class Main : Microsoft.Xna.Framework.Game
    {

        #region Variables
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private BasicEffect wireframeEffect, texturedPrimitiveEffect, texturedModelEffect;

        private ObjectManager objectManager;
        private MouseManager mouseManager;
        private KeyboardManager keyboardManager;
        private CameraManager cameraManager;
        private PhysicsManager physicsManager;
        private MenuManager menuManager;
        private Microsoft.Xna.Framework.Rectangle screenRectangle;

        private GenericDictionary<string, Texture2D> textureDictionary;
        private GenericDictionary<string, IVertexData> vertexDictionary;
        private GenericDictionary<string, DrawnActor3D> objectDictionary;
        private GenericDictionary<string, Model> modelDictionary;
        private GenericDictionary<string, Transform3DCurve> curveDictionary;
        private GenericDictionary<string, RailParameters> railDictionary;
        private GenericDictionary<string, SpriteFont> fontDictionary;

        private Vector2 screenCentre;

        //temp
        private ModelObject drivableModelObject;



        #endregion

        #region Properties

        public SpriteBatch SpriteBatch
        {
            get
            {
                return this.spriteBatch;
            }
        }
        public GraphicsDeviceManager Graphics
        {
            get
            {
                return this.graphics;
            }
        }
        public Vector2 ScreenCentre
        {
            get
            {
                return this.screenCentre;
            }
        }

        public Microsoft.Xna.Framework.Rectangle ScreenRectangle
        {
            get
            {
                if (this.screenRectangle == Microsoft.Xna.Framework.Rectangle.Empty)
                    this.screenRectangle = new Microsoft.Xna.Framework.Rectangle(0, 0,
                        (int)graphics.PreferredBackBufferWidth,
                        (int)graphics.PreferredBackBufferHeight);

                return this.screenRectangle;
            }
        }

        public MouseManager MouseManager
        {
            get
            {
                return this.mouseManager;
            }
        }
        public KeyboardManager KeyboardManager
        {
            get
            {
                return this.keyboardManager;
            }
        }
        public CameraManager CameraManager
        {
            get
            {
                return this.cameraManager;
            }
        }
        public PhysicsManager PhysicsManager
        {
            get
            {
                return this.physicsManager;
            }
        }

        #endregion  

        #region Initialisation

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            int width = 1024, height = 768;
            int worldScale = 10000;

            InitializeStaticReferences();
            InitializeGraphics(width, height);
            InitializeEffects();

            InitializeDictionaries();

            LoadFonts();
            LoadModels();
            LoadTextures();
            LoadVertices();
            LoadPrimitiveArchetypes();

            InitializeManagers();

            InitializeStaticCollidableGround(worldScale);
            //InitializeStaticCollidableGround2(worldScale);
            InitializeCollidableObjects();
            InitializeNonCollidableModels();
            InitializeSkyBox(worldScale);

            InitializeCameraTracks();
            InitializeCameraRails();
            InitializeCamera();


            base.Initialize();
        }

        private void InitializeDictionaries()
        {
            this.textureDictionary = new GenericDictionary<string, Texture2D>("texture dictionary");

            this.vertexDictionary = new GenericDictionary<string, IVertexData>("vertex dictionary");

            this.modelDictionary = new GenericDictionary<string, Model>("model dictionary");

            this.fontDictionary = new GenericDictionary<string, SpriteFont>("font dictionary");

            this.objectDictionary = new GenericDictionary<string, DrawnActor3D>("object dictionary");

            this.curveDictionary = new GenericDictionary<string, Transform3DCurve>("curve dictionary");

            this.railDictionary = new GenericDictionary<string, RailParameters>("rail dictionary");
        }

        private void InitializeManagers()
        {
            //CD-CR
            this.physicsManager = new PhysicsManager(this);
            Components.Add(physicsManager);

            bool bDebugMode = true; //show wireframe CD-CR surfaces
            this.objectManager = new ObjectManager(this, "gameObjects", bDebugMode);
            Components.Add(this.objectManager);

            this.mouseManager = new MouseManager(this, true);
            this.mouseManager.SetPosition(this.ScreenCentre);
            Components.Add(this.mouseManager);

            this.keyboardManager = new KeyboardManager(this);
            Components.Add(this.KeyboardManager);

            this.cameraManager = new CameraManager(this);
            Components.Add(this.cameraManager);

            Texture2D[] menuTextures = { this.textureDictionary["mainmenu"],
                this.textureDictionary["audio"],
                this.textureDictionary["controlsmenu"],
                this.textureDictionary["exitmenu"] };

            this.menuManager = new MenuManager(this, menuTextures, this.fontDictionary["menu"], Integer2.Zero, Color.White);

            Components.Add(this.menuManager);

        }

        private void InitializeStaticReferences()
        {
            Actor.game = this;
            Camera3D.game = this;
            Controller.game = this;
        }

        private void InitializeGraphics(int width, int height)
        {
            this.graphics.PreferredBackBufferWidth = width;
            this.graphics.PreferredBackBufferHeight = height;
            this.graphics.ApplyChanges();

            //or we can set full screen
               this.graphics.IsFullScreen = true;
                this.graphics.ApplyChanges();

            //records screen centre point - used by mouse to see how much the mouse pointer has moved
            this.screenCentre = new Vector2(this.graphics.PreferredBackBufferWidth / 2.0f,
                this.graphics.PreferredBackBufferHeight / 2.0f);
        }

        private void InitializeEffects()
        {
            this.wireframeEffect = new BasicEffect(graphics.GraphicsDevice);
            this.wireframeEffect.VertexColorEnabled = true;

            this.texturedPrimitiveEffect = new BasicEffect(graphics.GraphicsDevice);
            this.texturedPrimitiveEffect.VertexColorEnabled = true;
            this.texturedPrimitiveEffect.TextureEnabled = true;

            this.texturedModelEffect = new BasicEffect(graphics.GraphicsDevice);
            //    this.texturedModelEffect.VertexColorEnabled = true;
            this.texturedModelEffect.TextureEnabled = true;

        }

        #endregion

        #region Camera

        private void InitializeCameraRails()
        {
            RailParameters rail = null;

            rail = new RailParameters("rail1", new Vector3(-100, 50, 100), new Vector3(100, 100, 100));
            this.railDictionary.Add(rail.ID, rail);

            rail = new RailParameters("rail2", new Vector3(100, 50, 100), new Vector3(100, 50, -100));
            this.railDictionary.Add(rail.ID, rail);
        }

        private void InitializeCameraTracks()
        {
            Transform3DCurve curve = null;

            #region Curve1
            curve = new Transform3DCurve(CurveLoopType.Oscillate);
            curve.Add(new Vector3(0, 10, 200),
                    -Vector3.UnitZ, Vector3.UnitY, 0);

            curve.Add(new Vector3(0, 10, 20),
                   -Vector3.UnitZ, Vector3.UnitX, 2);

            this.curveDictionary.Add("room_action1", curve);
            #endregion
        }

        private void InitializeCamera()
        {
            Transform3D transform = null;
            Camera3D camera = null;
            string cameraLayout = "";

            #region Layout 1x1
            cameraLayout = "1x1";

            #region First Person Camera
            transform = new Transform3D(new Vector3(400, 30, 450), -Vector3.UnitZ, Vector3.UnitY);
            camera = new Camera3D("Static", ActorType.Camera, transform,
                ProjectionParameters.standardBanter,
                new Viewport(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
            camera.AttachController(new FirstPersonController("firstPersControl1",
            ControllerType.FirstPerson, AppData.CameraMoveKeys,
            AppData.CameraMoveSpeed, AppData.CameraStrafeSpeed, AppData.CameraRotationSpeed));

            //add the new camera to the approriate K, V pair in the camera manager dictionary i.e. where key is "1x2"
            this.cameraManager.Add(cameraLayout, camera);
            #endregion
            #endregion

            #region Layout 1x1 Collidable
            cameraLayout = "1x1 Collidable";

            transform = new Transform3D(new Vector3(0, 50, 100), -Vector3.UnitZ, Vector3.UnitY);
            camera = new Camera3D("Static", ActorType.Camera, transform,
                ProjectionParameters.StandardMediumSixteenNine,
                new Viewport(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
            camera.AttachController(new CollidableFirstPersonController(
                camera + " controller",
                ControllerType.FirstPersonCollidable,
                AppData.CameraMoveKeys, AppData.CollidableCameraMoveSpeed,
                AppData.CollidableCameraStrafeSpeed, AppData.CollidableCameraRotationSpeed,
                2f, 10, 1, 1, 1, Vector3.Zero, camera));

            this.cameraManager.Add(cameraLayout, camera);
            #endregion

            //finally, set the active layout
            this.cameraManager.SetActiveCameraLayout("1x1");

        }
        #endregion

        #region Assets

        private void LoadFonts()
        {

            this.fontDictionary.Add("menu", Content.Load<SpriteFont>("Assets/Fonts/menu"));

        }

        private void LoadModels()
        {
            this.modelDictionary.Add("box", Content.Load<Model>("Assets/Models/box"));

            this.modelDictionary.Add("island", Content.Load<Model>("Assets/Models/Island"));

            this.modelDictionary.Add("islandMk2", Content.Load<Model>("Assets/Models/IslandMk2"));

            this.modelDictionary.Add("LowPoly", Content.Load<Model>("Assets/Models/LowPolyIdea"));

            //this.modelDictionary.Add("foliage", Content.Load<Model>("Assets/Models/foliage"));

            //this.modelDictionary.Add("tree", Content.Load<Model>("Assets/Models/TempTree"));

            this.modelDictionary.Add("puzzleChest", Content.Load<Model>("Assets/Models/PuzzleChest"));
        }

        private void LoadTextures()
        {
            this.textureDictionary.Add("checkerboard",
                Content.Load<Texture2D>("Assets/Textures/Debug/checkerboard"));

            this.textureDictionary.Add("sand",
                Content.Load<Texture2D>("Assets/Textures/island/temp"));

            this.textureDictionary.Add("water",
                Content.Load<Texture2D>("Assets/Textures/island/water"));

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
            Content.Load<Texture2D>("Assets/Textures/Menu/controlsmenu"));

            this.textureDictionary.Add("exitmenu",
            Content.Load<Texture2D>("Assets/Textures/Menu/exitmenu"));

            this.textureDictionary.Add("exitmenuwithtrans",
            Content.Load<Texture2D>("Assets/Textures/Menu/exitmenuwithtrans"));

            this.textureDictionary.Add("mainmenu",
            Content.Load<Texture2D>("Assets/Textures/Menu/mainmenu"));

            this.textureDictionary.Add("audio",
            Content.Load<Texture2D>("Assets/Textures/Menu/audiomenu"));

            #endregion

        }

        private void LoadVertices()
        {
            VertexPositionColor[] verticesPositionColor = null;
            VertexPositionColorTexture[] verticesPositionColorTexture = null;
            IVertexData vertexData = null;
            float halfLength = 0.5f;

            #region Textured Quad
            verticesPositionColorTexture = new VertexPositionColorTexture[4];

            //top left
            verticesPositionColorTexture[0] = new VertexPositionColorTexture(
                new Vector3(-halfLength, halfLength, 0), Color.White, new Vector2(0, 0));
            //top right
            verticesPositionColorTexture[1] = new VertexPositionColorTexture(
            new Vector3(halfLength, halfLength, 0), Color.White, new Vector2(1, 0));
            //bottom left
            verticesPositionColorTexture[2] = new VertexPositionColorTexture(
            new Vector3(-halfLength, -halfLength, 0), Color.White, new Vector2(0, 1));
            //bottom right
            verticesPositionColorTexture[3] = new VertexPositionColorTexture(
            new Vector3(halfLength, -halfLength, 0), Color.White, new Vector2(1, 1));

            vertexData = new VertexData<VertexPositionColorTexture>(verticesPositionColorTexture, Microsoft.Xna.Framework.Graphics.PrimitiveType.TriangleStrip, 2);
            this.vertexDictionary.Add("textured_quad", vertexData);
            #endregion

            #region Textured Cube
            verticesPositionColorTexture = new VertexPositionColorTexture[36];

            Vector3 topLeftFront = new Vector3(-halfLength, halfLength, halfLength);
            Vector3 topLeftBack = new Vector3(-halfLength, halfLength, -halfLength);
            Vector3 topRightFront = new Vector3(halfLength, halfLength, halfLength);
            Vector3 topRightBack = new Vector3(halfLength, halfLength, -halfLength);

            Vector3 bottomLeftFront = new Vector3(-halfLength, -halfLength, halfLength);
            Vector3 bottomLeftBack = new Vector3(-halfLength, -halfLength, -halfLength);
            Vector3 bottomRightFront = new Vector3(halfLength, -halfLength, halfLength);
            Vector3 bottomRightBack = new Vector3(halfLength, -halfLength, -halfLength);

            //uv coordinates
            Vector2 uvTopLeft = new Vector2(0, 0);
            Vector2 uvTopRight = new Vector2(1, 0);
            Vector2 uvBottomLeft = new Vector2(0, 1);
            Vector2 uvBottomRight = new Vector2(1, 1);


            //top - 1 polygon for the top
            verticesPositionColorTexture[0] = new VertexPositionColorTexture(topLeftFront, Color.White, uvBottomLeft);
            verticesPositionColorTexture[1] = new VertexPositionColorTexture(topLeftBack, Color.White, uvTopLeft);
            verticesPositionColorTexture[2] = new VertexPositionColorTexture(topRightBack, Color.White, uvTopRight);

            verticesPositionColorTexture[3] = new VertexPositionColorTexture(topLeftFront, Color.White, uvBottomLeft);
            verticesPositionColorTexture[4] = new VertexPositionColorTexture(topRightBack, Color.White, uvTopRight);
            verticesPositionColorTexture[5] = new VertexPositionColorTexture(topRightFront, Color.White, uvBottomRight);

            //front
            verticesPositionColorTexture[6] = new VertexPositionColorTexture(topLeftFront, Color.White, uvBottomLeft);
            verticesPositionColorTexture[7] = new VertexPositionColorTexture(topRightFront, Color.White, uvBottomRight);
            verticesPositionColorTexture[8] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvTopLeft);

            verticesPositionColorTexture[9] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvTopLeft);
            verticesPositionColorTexture[10] = new VertexPositionColorTexture(topRightFront, Color.White, uvBottomRight);
            verticesPositionColorTexture[11] = new VertexPositionColorTexture(bottomRightFront, Color.White, uvTopRight);

            //back
            verticesPositionColorTexture[12] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);
            verticesPositionColorTexture[13] = new VertexPositionColorTexture(topRightBack, Color.White, uvTopRight);
            verticesPositionColorTexture[14] = new VertexPositionColorTexture(topLeftBack, Color.White, uvTopLeft);

            verticesPositionColorTexture[15] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);
            verticesPositionColorTexture[16] = new VertexPositionColorTexture(topLeftBack, Color.White, uvTopLeft);
            verticesPositionColorTexture[17] = new VertexPositionColorTexture(bottomLeftBack, Color.White, uvBottomLeft);

            //left 
            verticesPositionColorTexture[18] = new VertexPositionColorTexture(topLeftBack, Color.White, uvTopLeft);
            verticesPositionColorTexture[19] = new VertexPositionColorTexture(topLeftFront, Color.White, uvTopRight);
            verticesPositionColorTexture[20] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvBottomRight);

            verticesPositionColorTexture[21] = new VertexPositionColorTexture(bottomLeftBack, Color.White, uvBottomLeft);
            verticesPositionColorTexture[22] = new VertexPositionColorTexture(topLeftBack, Color.White, uvTopLeft);
            verticesPositionColorTexture[23] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvBottomRight);

            //right
            verticesPositionColorTexture[24] = new VertexPositionColorTexture(bottomRightFront, Color.White, uvBottomLeft);
            verticesPositionColorTexture[25] = new VertexPositionColorTexture(topRightFront, Color.White, uvTopLeft);
            verticesPositionColorTexture[26] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);

            verticesPositionColorTexture[27] = new VertexPositionColorTexture(topRightFront, Color.White, uvTopLeft);
            verticesPositionColorTexture[28] = new VertexPositionColorTexture(topRightBack, Color.White, uvTopRight);
            verticesPositionColorTexture[29] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);

            //bottom
            verticesPositionColorTexture[30] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvTopLeft);
            verticesPositionColorTexture[31] = new VertexPositionColorTexture(bottomRightFront, Color.White, uvTopRight);
            verticesPositionColorTexture[32] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);

            verticesPositionColorTexture[33] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvTopLeft);
            verticesPositionColorTexture[34] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);
            verticesPositionColorTexture[35] = new VertexPositionColorTexture(bottomLeftBack, Color.White, uvBottomLeft);

            vertexData = new VertexData<VertexPositionColorTexture>(verticesPositionColorTexture, Microsoft.Xna.Framework.Graphics.PrimitiveType.TriangleList, 12);
            this.vertexDictionary.Add("textured_cube", vertexData);
            #endregion

            #region Wireframe Origin Helper
            verticesPositionColor = new VertexPositionColor[6];

            //x-axis
            verticesPositionColor[0] = new VertexPositionColor(new Vector3(-halfLength, 0, 0), Color.Red);
            verticesPositionColor[1] = new VertexPositionColor(new Vector3(halfLength, 0, 0), Color.Red);
            //y-axis
            verticesPositionColor[2] = new VertexPositionColor(new Vector3(0, halfLength, 0), Color.Green);
            verticesPositionColor[3] = new VertexPositionColor(new Vector3(0, -halfLength, 0), Color.Green);
            //z-axis
            verticesPositionColor[4] = new VertexPositionColor(new Vector3(0, 0, halfLength), Color.Blue);
            verticesPositionColor[5] = new VertexPositionColor(new Vector3(0, 0, -halfLength), Color.Blue);

            vertexData = new VertexData<VertexPositionColor>(verticesPositionColor, Microsoft.Xna.Framework.Graphics.PrimitiveType.LineList, 3);
            this.vertexDictionary.Add("wireframe_origin_helper", vertexData);
            #endregion

            #region Wireframe Triangle
            verticesPositionColor = new VertexPositionColor[3];

            verticesPositionColor[0] = new VertexPositionColor(new Vector3(0, 1, 0), Color.Red);
            verticesPositionColor[1] = new VertexPositionColor(new Vector3(1, 0, 0), Color.Green);
            verticesPositionColor[2] = new VertexPositionColor(new Vector3(-1, 0, 0), Color.Blue);

            vertexData = new VertexData<VertexPositionColor>(verticesPositionColor, Microsoft.Xna.Framework.Graphics.PrimitiveType.TriangleStrip, 1);
            this.vertexDictionary.Add("wireframe_triangle", vertexData);
            #endregion
        }

        private void LoadPrimitiveArchetypes()
        {
            Transform3D transform = null;
            TexturedPrimitiveObject texturedQuad = null;

            #region Textured Quad Archetype
            transform = new Transform3D(Vector3.Zero, Vector3.Zero, Vector3.One, Vector3.UnitZ, Vector3.UnitY);
            texturedQuad = new TexturedPrimitiveObject("textured quad archetype", ActorType.Decorator,
                     transform, this.texturedPrimitiveEffect, this.vertexDictionary["textured_quad"],
                     this.textureDictionary["checkerboard"]); //or we can leave texture null since we will replace it later

            this.objectDictionary.Add("textured_quad", texturedQuad);
            #endregion
        }

        #endregion

        #region Collidable & Non-Collidable

        #region Non-Collidable

        private void InitializeNonCollidableModels()
        {
            //to do...
            Transform3D transform = new Transform3D(new Vector3(400, 5, 450),
                    new Vector3(0, 0, 0), new Vector3(1, 1, 1),
                    Vector3.UnitX, Vector3.UnitY);

            this.drivableModelObject = new ModelObject("box1",
                ActorType.Pickup, transform,
                this.texturedModelEffect, Color.White, 0.6f,
                this.textureDictionary["checkerboard"],
                this.modelDictionary["box"]);

            this.drivableModelObject.AttachController(new DriveController("dc1", ControllerType.Drive,
                AppData.PlayerMoveKeys, AppData.PlayerMoveSpeed,
                AppData.PlayerStrafeSpeed, AppData.PlayerRotationSpeed));
            this.objectManager.Add(drivableModelObject);
        }
        #endregion

        #region Collidable

        private void InitializeSkyBox(int worldScale)
        {
            TexturedPrimitiveObject archTexturedPrimitiveObject = null, cloneTexturedPrimitiveObject = null;

            #region Archetype
            //we need to do an "as" typecast since the dictionary holds DrawnActor3D types
            archTexturedPrimitiveObject = this.objectDictionary["textured_quad"] as TexturedPrimitiveObject;
            archTexturedPrimitiveObject.Transform3D.Scale *= worldScale;
            #endregion

            #region Skybox
            //back
            cloneTexturedPrimitiveObject = (TexturedPrimitiveObject)archTexturedPrimitiveObject.Clone();
            cloneTexturedPrimitiveObject.ID = "skybox_back";
            cloneTexturedPrimitiveObject.Transform3D.Translation = new Vector3(0, 0, -worldScale / 2.0f);
            cloneTexturedPrimitiveObject.Texture = this.textureDictionary["skybox_back"];
            this.objectManager.Add(cloneTexturedPrimitiveObject);

            //left
            cloneTexturedPrimitiveObject = (TexturedPrimitiveObject)archTexturedPrimitiveObject.Clone();
            cloneTexturedPrimitiveObject.ID = "skybox_left";
            cloneTexturedPrimitiveObject.Transform3D.Translation = new Vector3(-worldScale / 2.0f, 0, 0);
            cloneTexturedPrimitiveObject.Transform3D.Rotation = new Vector3(0, 90, 0);
            cloneTexturedPrimitiveObject.Texture = this.textureDictionary["skybox_left"];
            this.objectManager.Add(cloneTexturedPrimitiveObject);

            //right
            cloneTexturedPrimitiveObject = (TexturedPrimitiveObject)archTexturedPrimitiveObject.Clone();
            cloneTexturedPrimitiveObject.ID = "skybox_right";
            cloneTexturedPrimitiveObject.Transform3D.Translation = new Vector3(worldScale / 2.0f, 0, 0);
            cloneTexturedPrimitiveObject.Transform3D.Rotation = new Vector3(0, -90, 0);
            cloneTexturedPrimitiveObject.Texture = this.textureDictionary["skybox_right"];
            this.objectManager.Add(cloneTexturedPrimitiveObject);

            //front
            cloneTexturedPrimitiveObject = (TexturedPrimitiveObject)archTexturedPrimitiveObject.Clone();
            cloneTexturedPrimitiveObject.ID = "skybox_front";
            cloneTexturedPrimitiveObject.Transform3D.Translation = new Vector3(0, 0, worldScale / 2.0f);
            cloneTexturedPrimitiveObject.Transform3D.Rotation = new Vector3(0, 180, 0);
            cloneTexturedPrimitiveObject.Texture = this.textureDictionary["skybox_front"];
            this.objectManager.Add(cloneTexturedPrimitiveObject);

            //top
            cloneTexturedPrimitiveObject = (TexturedPrimitiveObject)archTexturedPrimitiveObject.Clone();
            cloneTexturedPrimitiveObject.ID = "skybox_sky";
            cloneTexturedPrimitiveObject.Transform3D.Translation = new Vector3(0, worldScale / 2.0f, 0);
            cloneTexturedPrimitiveObject.Transform3D.Rotation = new Vector3(90, -90, 0);
            cloneTexturedPrimitiveObject.Texture = this.textureDictionary["skybox_sky"];
            this.objectManager.Add(cloneTexturedPrimitiveObject);

            //water
            cloneTexturedPrimitiveObject = (TexturedPrimitiveObject)archTexturedPrimitiveObject.Clone();
            cloneTexturedPrimitiveObject.ID = "water";
            cloneTexturedPrimitiveObject.Transform3D.Translation = new Vector3(0, 0, 0);
            cloneTexturedPrimitiveObject.Transform3D.Rotation = new Vector3(90, 0, 0);
            cloneTexturedPrimitiveObject.Texture = this.textureDictionary["water"];
            this.objectManager.Add(cloneTexturedPrimitiveObject);
            #endregion
        }

        private void InitializeCollidableObjects()
        {
            CollidableObject chest = null;
            Transform3D transform3D = null;
            Texture2D texture = null;

            Model model = this.modelDictionary["puzzleChest"];
            texture = this.textureDictionary["checkerboard"];
            transform3D = new Transform3D(new Vector3(10, 50, 10), new Vector3(0, 0, 0),
                new Vector3(0.2f, 0.2f, 0.2f), Vector3.UnitX, Vector3.UnitY);

            chest = new CollidableObject("chest", ActorType.CollidableGround, transform3D, this.texturedModelEffect, Color.White, 1, texture, model);
            chest.AddPrimitive(new Box(transform3D.Translation, Matrix.Identity, transform3D.Scale), new MaterialProperties(0.2f, 0.2f, 0.2f));
            chest.Enable(true, 1); //change to false, see what happens.
            this.objectManager.Add(chest);
        }
        
        private void InitializeStaticCollidableGround(int scale)
        {
            CollidableObject collidableObject = null;
            Transform3D transform3D = null;
            Texture2D texture = null;

            Model model = this.modelDictionary["LowPoly"];
            texture = this.textureDictionary["sand"];
            transform3D = new Transform3D(new Vector3(-90, 580, 100), new Vector3(0, 0, 0),
                new Vector3(0.7f, 0.7f, 0.7f), Vector3.UnitX, Vector3.UnitY);

            collidableObject = new CollidableObject("ground", ActorType.CollidableGround, transform3D, this.texturedModelEffect, Color.White, 1, texture, model);
            collidableObject.AddPrimitive(new Box(transform3D.Translation, Matrix.Identity, transform3D.Scale), new MaterialProperties(0.8f, 0.8f, 0.7f));
            collidableObject.Enable(true, 1); //change to false, see what happens.
            this.objectManager.Add(collidableObject);
        }

        //private void InitializeStaticCollidableGround2(int scale)
        //{
        //    CollidableObject collidableObject = null;
        //    Transform3D transform3D = null;
        //    Texture2D texture = null;

        //    Model model = this.modelDictionary["box"];
        //    texture = this.textureDictionary["sand"];
        //    transform3D = new Transform3D(new Vector3(0, 5, 0), new Vector3(0, 0, 0),
        //        new Vector3(scale, 0.01f, scale), Vector3.UnitX, Vector3.UnitY);

        //    collidableObject = new CollidableObject("ground", ActorType.CollidableGround, transform3D, this.texturedModelEffect, Color.White, 1, texture, model);
        //    collidableObject.AddPrimitive(new Box(transform3D.Translation, Matrix.Identity, transform3D.Scale), new MaterialProperties(0.8f, 0.8f, 0.7f));
        //    collidableObject.Enable(true, 1); //change to false, see what happens.
        //    this.objectManager.Add(collidableObject);
        //}

        #endregion

        #endregion

        #region Game Loop & Content

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            this.modelDictionary.Dispose();
            this.textureDictionary.Dispose();
            this.objectDictionary.Dispose();
            this.vertexDictionary.Dispose();
            this.railDictionary.Dispose();
            this.curveDictionary.Dispose();
        }
        
        protected override void Update(GameTime gameTime)
        {
         
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            foreach (Camera3D camera in this.cameraManager)
            {
                //set the viewport based on the current camera
                graphics.GraphicsDevice.Viewport = camera.Viewport;
                base.Draw(gameTime);

                //set which is the active camera (remember that our objects use the CameraManager::ActiveCamera property to access View and Projection for rendering
                this.cameraManager.ActiveCameraIndex++;
            }
        }

        #endregion

    }
}
