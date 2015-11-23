using System;
using System.Windows;

namespace SmartFish
{
	class Util
	{
		private static Random rnd = new Random();

		//return [0.0, 1.0)
		public static double Rand()
		{
			return rnd.NextDouble();
		}

		// min inclusive, max exclusive
		public static int Rand(int min, int max)
		{
			return rnd.Next(min, max);
		}

		//return [min. max)
		public static double Rand(double min, double max)
		{
			double lMax = (min < max) ? (max) : (min);
			double lMin = (min < max) ? (min) : (max);
			return lMin + (lMax - lMin) * (rnd.NextDouble());
		}


		public static double RadianToAngle(double radian)
		{
			return 180 * (radian / Math.PI);
		}


		public static double Distance(Point p1, Point p2)
		{
			return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
		}
		
		
		public static Point Normalize(ref Point p1)
		{
			Point normP = new Point(p1.X, p1.Y);
			double length = Math.Sqrt(p1.X * p1.X + p1.Y * p1.Y);
			normP.X /=  length;
			normP.Y /=  length;
			return normP;
		}

		

		// wrap around window limits
		// expand more so it will not respawn at the border
		static int baitBorder = 8;
		public static void BaitWithinWindow(ref Point p)
		{
			if (p.X + baitBorder > Config.WindowWidth) p.X = baitBorder;
			if (p.X - baitBorder < 0) p.X = Config.WindowWidth - baitBorder;
			if (p.Y + baitBorder > Config.WindowHeight) p.Y = baitBorder;
			if (p.Y - baitBorder < 0) p.Y = Config.WindowHeight - baitBorder;
		}



	}//class util
}
