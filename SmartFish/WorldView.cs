using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace SmartFish
{
    public class WorldView : FrameworkElement
    {
        // Create a collection of child visual objects.
        private VisualCollection mChildren;

		public WorldView()
		{
			mChildren = new VisualCollection(this);
		}

		public void Clear()
		{
			mChildren.Clear();
		}

		public void AddBackgroundImage()
		{
			DrawingVisual drawingVisual = new DrawingVisual();
			DrawingContext drawingContext = drawingVisual.RenderOpen();

			Uri imageUri = new Uri("pack://application:,,,/SmartFish;component/Resources/sea3.jpg");
			System.Windows.Media.Imaging.BitmapImage bmi = 
				new System.Windows.Media.Imaging.BitmapImage( imageUri);
			Rect rect = new Rect(new Size(Config.WindowWidth, Config.WindowHeight));
			drawingContext.DrawImage( bmi, rect);
			drawingContext.Close();
			mChildren.Add(drawingVisual);
		}

		/*
		public void AddFloatPanel()
		{
			mChildren.Add(floatPanel);
		}*/

		public void AddPathGeometryVisual(
			System.Windows.Shapes.Polygon aPolygon, 
			System.Windows.Media.Brush brush)
		{
			DrawingVisual drawingVisual = new DrawingVisual();

			// Retrieve the DrawingContext in order to create new drawing content.
			DrawingContext drawingContext = drawingVisual.RenderOpen();

			Point[] pointArray = aPolygon.Points.ToArray();
			PathFigure pf = new PathFigure();
			pf.StartPoint = pointArray[0];
			for (int i = 1; i < pointArray.Length; ++i)
			{
				pf.Segments.Add(new LineSegment(pointArray[i], true));
			}
			PathGeometry pg = new PathGeometry();
			pg.Figures.Add(pf);

			drawingContext.DrawGeometry(brush, null, pg);
			drawingContext.Close();

			mChildren.Add(drawingVisual);
		}

		// Provide a required override for the VisualChildrenCount property.
		protected override int VisualChildrenCount
		{
			get { return mChildren.Count; }
		}

		// Provide a required override for the GetVisualChild method.
		protected override Visual GetVisualChild(int index)
		{
			if (index < 0 || index >= mChildren.Count)
			{
				throw new ArgumentOutOfRangeException();
			}

			return mChildren[index];
		}

    }
}
