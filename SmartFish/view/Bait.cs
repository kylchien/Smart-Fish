using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Media;

namespace SmartFish
{
	
	public class Bait
	{
		private static readonly Point[] defaultPoints = new Point[]{
			new Point(2, 2), new Point(2, -2), 
			new Point(-2,-2), new Point(-2, 2)
		};
		
		private List<Point> mPoints = new List<Point>(defaultPoints);

		private Polygon mPolygon = new Polygon();
		public Polygon BaitPolygon { get { return mPolygon; } }
		
		private Point mCenter;
		public Point Center { get {return mCenter;} }
		
		private double mScale;
		
		public Bait()
		{
			mScale = Config.BaitScale;
			
			Reset();
			
			mPolygon.Fill = Brushes.OrangeRed;
            mPolygon.Stroke = Brushes.OrangeRed;
			mPolygon.StrokeThickness = 1;
		}
		
		public void Reset()
		{
			mPoints = new List<Point>(defaultPoints);
			//randomize center
			double x = Util.Rand(0, Config.WindowWidth);
			double y = Util.Rand(0, Config.WindowHeight);
			mCenter = new Point(x,y);
			Util.BaitWithinWindow(ref mCenter);
			
			double angle = Util.Rand((int)0, (int)180);
			
			Matrix m = new Matrix(1, 0, 0, 1, 0, 0);			
			m.Scale(mScale, mScale); 	//scale at (0,0)
			m.Rotate(angle); 			//rotate at (0,0)
			m.Translate(mCenter.X, mCenter.Y);

			MatrixTransform mt = new MatrixTransform(m);
			for (int i = 0; i < mPoints.Capacity; i++)
				mPoints[i] = mt.Transform(mPoints[i]);
			
			mPolygon.Points = new PointCollection(mPoints);
		}
	}
	
}//namespace
