/*
 * Created by SharpDevelop.
 * User: moneypig
 * Date: 04/18/2012
 * Time: 21:02
 */
using System;
using System.Windows;
using System.Collections.Generic;

namespace SmartFish
{
	/// <summary>
	/// Not required, but save it here as a reference
	/// </summary>
	public class Matrix2D
	{
		// create identiy matrix at initialization
		private double[,] mMat = new double[3,3]{ {1,0,0}, {0,1,0}, {0,0,1}};
		
		public Matrix2D(){}
		
		private void Multiply(Matrix2D iMat)
		{
			double[,] newMat = new double[3,3]{ {0,0,0}, {0,0,0}, {0,0,0}};
			
			for(int i=0;i<3;i++)
				for(int j=0; j<3; j++)
					for(int k=0; k<3; k++)
						newMat[i,j] += mMat[i,k] * iMat.mMat[k,j];

			mMat = newMat;
		}
		
		
		public void Identity()
		{
			for(int i =0; i<3;i++)
				for(int j = 0; j<3; j++)
					mMat[i,j] = 0;

			mMat[0,0] = 1;
			mMat[1,1] = 1;
			mMat[2,2] = 1;
		}
		
		
		public void Scale(double xScale, double yScale)
		{
			Matrix2D iMat = new Matrix2D();
			iMat.mMat[0,0] = xScale;
			iMat.mMat[1,1] = yScale;
			Multiply(iMat);
		}
		
		
		public void Rotate(double radian)
		{
			double sin = Math.Sin(radian); 
			double cos = Math.Cos(radian);
			Matrix2D iMat = new Matrix2D();
			iMat.mMat[0,0] = cos;
			iMat.mMat[0,1] = sin;
			iMat.mMat[1,0] = -sin;
			iMat.mMat[1,1] = cos;
			Multiply(iMat);
		}
		
		public void Translate(double x, double y)
		{
			Matrix2D iMat = new Matrix2D();
			iMat.mMat[2,0] = x;
			iMat.mMat[2,1] = y;
			Multiply(iMat);
		}
		
		public void TransformPoints(ref List<Point> iPoints)
		{
			List<Point> oPoints = new List<Point>();
			foreach(Point p in iPoints)
			{
				double x = mMat[0,0] * p.X + mMat[1,0] * p.Y + mMat[2,0];
				double y = mMat[0,1] * p.X + mMat[1,1] * p.Y + mMat[2,1];
				oPoints.Add(new Point(x,y));
			}
			iPoints = oPoints;
		}
	}
}
