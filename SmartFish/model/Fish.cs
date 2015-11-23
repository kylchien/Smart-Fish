using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Xml;
using System.IO;


namespace SmartFish
{
	public delegate void MovingEventHandler(object fish, EventArgs args);
		
	class Fish
	{
		
		public event MovingEventHandler MovingEvent;
		
		protected void OnMovingEvent(object fish, EventArgs args)
		{
			if(MovingEvent != null)
				MovingEvent(fish,args);
		}
		
		private NeuralNet mBrain;
		
		public NeuralNet Brain { get{return mBrain;}}
		
		static protected double mSwimSpeed  = 0;

		// current fish position, the centroid of the polygon
		private Point mCenter;

		// the center point of fish body
		public Point CurPosition
		{
			get{ return mCenter;}
			//set{ mCenter = value;}
		}//Center

		
		// the index of the closest bait to fish's position 
		private int mClosestBaitIndex=-1;
		public int ClosestBaitIndex { get{return mClosestBaitIndex;} }
	
		
		//direction fish is facing
		private Point mLookAt= new Point(0, 0);

		//the amount of rotation
		private double mRotation;
		public double Rotation { get { return mRotation; }  }

		private double mSpeed;

		//to store output from the ANN
		private double mLTrack, mRTrack;

		public Genome FishGenome = new Genome();
		
		
		private int mID = -1;
		public int ID { get{return mID;} }
		
		public Fish(int id) 
		{
			mID = id;
			Reset();
			WriteToXML();
		}
		
		public void Relocate()
		{
			mCenter = new Point( 
				Util.Rand() * Config.WindowWidth, 
				Util.Rand() * Config.WindowHeight);
			
			WrappingWithinWindow(ref mCenter);
		}
		
		public void WeightsToGenes()
		{
			FishGenome.Genes = mBrain.GetWeights();
		}
		
		public void GenesToWeights()
		{
			mBrain.SetWeights(FishGenome.Genes);
		}
		
		public void Reset()
		{
			mBrain = new NeuralNet();
			Relocate();
			WeightsToGenes();	
			FishGenome.Fitness = 0;

			mLTrack = 30;
			mRTrack = 30;
			mRotation = Util.Rand() * (Math.PI * 2);
		}

		
		protected static double MIN_SWIM_SPEED = 0;
		protected static double MAX_SWIM_SPEED= 5;
		protected static double SWIM_SPEED_STEP = 0.25;

		public static bool reachMaxSwimSpeed()
		{
			if ( Math.Abs(MAX_SWIM_SPEED - mSwimSpeed) < 0.000001)
				return true;
			return false;
		}

		public static bool reachMinSwimSpeed()
		{
			if (Math.Abs(MIN_SWIM_SPEED - mSwimSpeed) < 0.000001)
				return true;
			return false;
		}

		public static void updateSwimSpeed(char sign)
		{
			switch (sign)
			{
				case '+':
					if (reachMaxSwimSpeed() )
						return;
					mSwimSpeed += SWIM_SPEED_STEP;
					break;
				case '-':
					if (reachMinSwimSpeed() )
						return;
					mSwimSpeed -= SWIM_SPEED_STEP;
					break;
			}
			Console.Out.WriteLine(mSwimSpeed);
		}
		
		//wrap around window limits
		public static void WrappingWithinWindow(ref Point p)
		{
			// The max(10, 17), the rectangle boundary of fish 
			// 17 /2 = 8.5
			if (p.X + 8.5 > Config.WindowWidth) p.X = 8.5;
			if (p.X - 8.5 < 0) p.X = Config.WindowWidth - 8.5;
			if (p.Y + 8.5 > Config.WindowHeight) p.Y = 8.5;
			if (p.Y - 8.5 < 0) p.Y = Config.WindowHeight - 8.5;
		}
		

		// return the index of the bait which is eaten by the fish
		// return -1 otherwise
		public int EatingBait(ref List<Bait> baits)
		{
			Point baitPos = baits[mClosestBaitIndex].Center;
			//double dist = Util.Distance(ref mCenter, ref baitPos);
            double dist = Util.Distance(mCenter, baitPos);
			if (dist < Config.EatingDistance)
				return mClosestBaitIndex;
			return -1;
		}
		
		public Point ClosestBait(ref List<Bait> baits)
		{
			double shortestDist = 99999;
			for (int i = 0; i < baits.Count; i++ )
			{
				//Bait bait = baits[i];
				//double dist = Util.Distance(ref mCenter, ref bait.Center);
				double dist = Util.Distance(mCenter, baits[i].Center);
				if(dist < shortestDist)
				{
					shortestDist = dist;
					mClosestBaitIndex = i; //update mClosestBaitIndex
				}
			}
			return baits[mClosestBaitIndex].Center;
		}


		//-------------------------------Move()--------------------------------
		//
		//	First we take sensor readings and feed these into the sweepers brain.
		//
		//	The inputs are:
		//	
		//	A vector to the closest mine (x, y)
		//	The sweepers 'look at' vector (x, y)
		//
		//	We receive two outputs from the brain.. lTrack & rTrack.
		//	So given a force for each track we calculate the resultant rotation 
		//	and acceleration and apply to current velocity vector.
		//
		//-----------------------------------------------------------------------
		public void Move(ref List<Bait> baits)
		{
			//this will store all the inputs for the NN
			List<double> inputs = new List<double>();

			Point closestBaitPos = ClosestBait(ref baits);
			
			//get vector to closest mine
			Point closestBaitVec = 
				new Point(	closestBaitPos.X-mCenter.X,
				         	closestBaitPos.Y-mCenter.Y
				         );

			//normalize it
			Point normP = Util.Normalize(ref closestBaitVec);

			//add in the position of closest bait
			inputs.Add(normP.X);
			inputs.Add(normP.Y);

			//add in fish look at vector
			inputs.Add(mLookAt.X);
			inputs.Add(mLookAt.Y);

			//update the brain and get feedback
			List<double> output = mBrain.Output(inputs);

			//assign the outputs to the sweepers left & right tracks
			mLTrack = output[0];
			mRTrack = output[1];

			//calculate steering forces
			double rotForce = mLTrack - mRTrack;

			//clamp rotation
			if (rotForce > Config.MaxTurnRate)
				rotForce = Config.MaxTurnRate;
			else if (rotForce < -1 * Config.MaxTurnRate)
				rotForce = -1 * Config.MaxTurnRate;
			
			mRotation += rotForce;

			mSpeed = mLTrack + mRTrack + mSwimSpeed;

			//update look at
			mLookAt.X = -1 * Math.Sin(mRotation);
			mLookAt.Y = Math.Cos(mRotation);

			//update position
			mCenter.X = mCenter.X + mLookAt.X * mSpeed;
			mCenter.Y = mCenter.Y + mLookAt.Y * mSpeed;

			WrappingWithinWindow(ref mCenter);
			
			//trigger the update event
			OnMovingEvent(this,null);
		}//end void Update


		public void WriteToXML()
		{
			FileStream fileStream = new FileStream(@"neuron.txt", FileMode.Append, FileAccess.Write, FileShare.Write);
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = "  ";
			settings.NewLineChars = "\r\n";
			settings.NewLineHandling = NewLineHandling.Replace;
			settings.NewLineOnAttributes = false;
			settings.OmitXmlDeclaration = true;

			using (XmlWriter writer = XmlWriter.Create(fileStream, settings))
			{
				writer.WriteStartElement("Fish");
				writer.WriteAttributeString("ID", Convert.ToString(mID) );
				writer.WriteAttributeString("Rotation", String.Format("{0:0.000}", mRotation));
				writer.WriteAttributeString("LTrack", String.Format("{0:0.000}", mLTrack));
				writer.WriteAttributeString("RTrack", String.Format("{0:0.000}", mRTrack));
				mBrain.WriteToXML(writer);
				writer.WriteEndElement();
				writer.Flush();
			}

			// add new line break
			using (StreamWriter writer = new StreamWriter(fileStream))
			{
				writer.Write(Environment.NewLine);
			}
			
			fileStream.Close();
		}

	}//class Fish
}//namespace
