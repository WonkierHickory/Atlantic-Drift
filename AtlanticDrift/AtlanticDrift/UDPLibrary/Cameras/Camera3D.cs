﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AtlanticDrift;

/*
Function: 		Represents a Camera in 3D to which we can attach controllers (e.g. FirstPerson, Track etc)
Author: 		NMCG
Version:		1.0
Date Updated:	1/2/16
Bugs:			None
Fixes:			None
*/

namespace UDPLibrary
{
    //Represents a static camera with no attached controller (i.e. dynamic behaviour)
    public class Camera3D : Actor
    {
        #region Fields
        private ProjectionParameters projectionParameters;
        private Viewport viewPort;
        private Vector2 viewportCentre;
        #endregion

        #region Properties
        public Matrix View
        {
            get
            {
                return Matrix.CreateLookAt(this.Transform3D.Translation,
                    this.Transform3D.Translation + this.Transform3D.Look,
                    this.Transform3D.Up);
            }
        }
        public ProjectionParameters ProjectionParameters
        {
            get
            {
                return this.projectionParameters;
            }
            set
            {
                this.projectionParameters = value;
            }
        }
        public Vector2 ViewportCentre
        {
            get
            {
                if (this.viewportCentre == null)
                    this.viewportCentre = new Vector2(this.viewPort.X / 2.0f, this.viewPort.X / 2.0f);

                return this.viewportCentre;
            }
        }
        public Viewport Viewport
        {
            get
            {
                return this.viewPort;
            }
            set
            {
                this.viewPort = value;
            }
        }
        public int ViewportX
        {
            get
            {
                return this.viewPort.X;
            }
            set
            {
                this.viewPort.X = value;
            }
        }
        public int ViewportY
        {
            get
            {
                return this.viewPort.Y;
            }
            set
            {
                this.viewPort.Y = value;
            }
        }
        public int ViewportWidth
        {
            get
            {
                return this.viewPort.Width;
            }
            set
            {
                this.viewPort.Width = value;
            }
        }
        public int ViewportHeight
        {
            get
            {
                return this.viewPort.Height;
            }
            set
            {
                this.viewPort.Height = value;
            }
        }
        #endregion

        //creates a default camera3D - we can use this for a fixed camera archetype i.e. one we will clone - see MainApp::InitialiseCameras()
        public Camera3D(string id, ObjectType objectType)
            : this(id, objectType, Transform3D.Zero, ProjectionParameters.StandardMediumFourThree, game.GraphicsDevice.Viewport)
        {

        }
        public Camera3D(string id, ObjectType objectType,
            Transform3D transform, ProjectionParameters projectionParameters, Viewport viewPort)
            : base(id, objectType, transform)
        {
            this.projectionParameters = projectionParameters;
            this.viewPort = viewPort;
        }

        public virtual object Clone()
        {
            return new Camera3D(this.ID,
                this.ObjectType, (Transform3D)this.Transform3D.Clone(), (ProjectionParameters)this.projectionParameters.Clone(), this.viewPort);
        }

        public override string ToString()
        {
            return this.ID
                + ", Translation: " + MathUtility.Round(this.Transform3D.Translation, 0)
                + ", Look: " + MathUtility.Round(this.Transform3D.Look, 0)
                + ", Up: " + MathUtility.Round(this.Transform3D.Up, 0);

        }
    }
}
