using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SmartFish
{
	public static class Config
	{
		public static double WindowHeight = 480;
		public static double WindowWidth = 720;
		public static uint NumTicks = 1200;

		// GA Parameters
		public static int NumElites = 10; 	
		public static int NumNewBloods = 5;		
		public static double CrossoverRate = 0.5;
		public static double MutationRate = 0.2;
		public static double Perturbation = 0.3;

		// Neural Network Parameters
		public static int NumInput = 4;
		public static int NumOutput = 2;
		public static int NumHiddenLayer = 1;
		public static int NumNeuronsPerHiddenLayer = 6;
		public static double BiasFactor = -1;
		public static double ActivationReponse = 1; //for tweeking the sigmoid function

		//Fish and Baits 
		public static bool FishRelocate = true;
		public static double MaxTurnRate = 1;//0.3;
		public static double FishScale = 1;
		public static double BaitScale = 1;
		public static int NumFishes = 40;
		public static int NumBaits = 120;
		public static double EatingDistance = 5;
	}
}
