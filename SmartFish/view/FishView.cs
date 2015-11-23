using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Media;


namespace SmartFish
{
	class FishView
	{
		private static readonly Point[] defaultPoints = new Point[]{
				new Point(0, 8.5), new Point(5, 4.0), new Point(5, 1.5), new Point(2, -3.5), new Point(4,-8.5),
				new Point(-4,-8.5), new Point(-2, -3.5), new Point(-5, 1.5), new Point(-5, 4.0), new Point(0,8.5)
		};
		private static readonly Point defaultCenter = new Point(0, 0);
		
		private List<Point> mPoints = new List<Point>(defaultPoints);

		private Polygon mPolygon = new Polygon();

		public Polygon FishPolygon { get { return mPolygon; } }

		private double mScale;

		private Fish mModel;

		public FishView(Fish aFish)
		{
			mModel = aFish;
			mModel.MovingEvent += new MovingEventHandler(Move);
			Reset();
		}

		public void Reset()
		{
			mModel.Reset();

			mScale = Config.FishScale;
			mPoints = new List<Point>(defaultPoints);

			Matrix m = new Matrix(1, 0, 0, 1, 0, 0);			
			//scale at (0,0)
			m.Scale(mScale, mScale);
			m.Translate(mModel.CurPosition.X, mModel.CurPosition.Y); 

			MatrixTransform mt = new MatrixTransform(m);
			for (int i = 0; i < mPoints.Capacity; i++)
				mPoints[i] = mt.Transform(mPoints[i]);

			mPolygon.Points = new PointCollection(mPoints);
			mPolygon.Fill = Brushes.Aqua;
			mPolygon.Stroke = Brushes.Black;
			mPolygon.StrokeThickness = 1;
		
		}
		
		public void Move(Object fishObject, EventArgs args)
		{
			Fish fish = fishObject as Fish;
			//	sets up a translation matrix for the fishes according to its
			//  scale, rotation and position. Returns the transformed vertices.		
			Matrix m = new Matrix(1, 0, 0, 1, 0, 0);
			
			//scale at (0,0)
			m.Scale(mScale, mScale); 
			
			//rotate at (0,0)
			m.Rotate(Util.RadianToAngle(fish.Rotation)); 
			
			//move from local coordinate system to global
			m.Translate(fish.CurPosition.X, fish.CurPosition.Y); 

			mPoints =  new List<Point>(defaultPoints);
			MatrixTransform mt = new MatrixTransform(m);
			for (int i = 0; i < mPoints.Capacity; i++)
				mPoints[i] = mt.Transform(mPoints[i]);
			
			mPolygon.Points = new PointCollection(mPoints);		
		}
		
		

	}
}
