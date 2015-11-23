/*
 * Created by SharpDevelop.
 * User: kchien
 * Date: 20/04/2012
 * Time: 10:21 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Diagnostics;
using System.Xml;
using System.Windows.Media;

namespace SmartFish
{
	/// <summary>
	/// Description of World.
	/// </summary>
	public class World
	{
		private uint mTick = 0;
		private uint mGeneration = 1;
		public uint Generation { get{return mGeneration;} }
		
		private List<Fish> mFishes = new List<Fish>();
		private List<FishView> mFishViews = new List<FishView>();
		private List<Bait> mBaits = new List<Bait>();
		private Canvas mCanvas;
		
		private GA mGA;
		
		private void createFishes()
		{
			for (int i = 0; i < Config.NumFishes; i++)
			{
				Fish fish = new Fish(i);
				FishView fishView = new FishView(fish);
				mFishes.Add(fish);
				mFishViews.Add(fishView);
				//mCanvas.Children.Add(fishView.FishPolygon);
			}
		}
		
		private void createBaits()
		{	
			for(int i =0; i< Config.NumBaits; i++)
			{
				Bait bait = new Bait();
				mBaits.Add(bait);
				//mCanvas.Children.Add(bait.BaitPolygon);
			}
		}

		private WorldView worldView;
		
		public World(ref Canvas aCanvas)
		{
			mCanvas = aCanvas;
			worldView = new WorldView();
			
			createFishes();
			createBaits();
			
			int numGenes = mFishes[0].Brain.GetWeights().Count;
			
			mGA= new GA(Config.NumFishes, numGenes, 
			            Config.NumElites,
			            Config.MutationRate,Config.CrossoverRate, Config.Perturbation);

			//mCanvas.Children.Add(worldView);
			mCanvas.Children.Insert(0, worldView);
		}

		private void updateWorldView()
		{
			worldView.Clear();
			worldView.AddBackgroundImage();
			foreach (FishView fv in mFishViews)
				worldView.AddPathGeometryVisual(fv.FishPolygon, Brushes.Orange);
			foreach (Bait b in mBaits)
				worldView.AddPathGeometryVisual(b.BaitPolygon, Brushes.ForestGreen);
		}

		
		public void Update()
		{			
			if(++mTick < Config.NumTicks)
			{
				for(int i=0; i<mFishes.Count; i++)
				{
					mFishes[i].Move(ref mBaits);
					int baitIndex = mFishes[i].EatingBait(ref mBaits);
					if (baitIndex >= 0)
					{
						
						mFishes[i].FishGenome.Fitness++;
						mBaits[baitIndex].Reset();
					}
				}
				updateWorldView();
			}
			else
			{
				mGA.Genomes.Clear();
				foreach(Fish f in mFishes)
				{
					mGA.Genomes.Add(f.FishGenome);
				}
				
				mGA.Evolve();
				
				for(int i=0; i<Config.NumFishes;i++)
				{
					
					Genome genome = Genome.Copy(mGA.Genomes[i]);
					genome.Fitness = 0;
					mFishes[i].FishGenome = genome;
					mFishes[i].GenesToWeights();
					mFishes[i].Relocate();
				}

				mGeneration++;
				mTick = 0;

			}//end else

		}//end Update()


        private static int numSave = 1;
		public void SaveToFile()
		{
            string fileName = numSave.ToString() + ".txt";

            //Creating XmlWriter Settings
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.NewLineOnAttributes = false;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            using (XmlWriter writer = XmlWriter.Create(fileName, settings))
            {
                foreach (Fish f in mFishes)
                {
                    f.Brain.WriteToXML(writer);
                }
            }
            numSave++;
		}

	}//end class
}//namespace
