using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace SmartFish
{
	

	
	public class GA
	{
		private int mPopSize;
		private int mNumGenes;
		private List<Genome> mPop;
		public List<Genome> Genomes { get { return mPop; } }

		private double mBestFitness = 0;
		private double mAvgFitness = 0;
		private double mWorstFitness = 9999999;
		public double BestFitness { get { return mBestFitness; }  }
		public double AvergeFitness { get { return mAvgFitness; }  }
		public double WorstFitness { get { return mWorstFitness; }  }

		private double mMutationRate;
		private double mCrossoverRate;
		private double mPerturbation;
		private int mNumElites;
		private List<Genome> mNewBloods;
		
		private int mGeneration = 1;

		public GA(int aPopSize, int aNumGenes, int aNumElites, 
		          double aMutRate, double aCrossRate, double aPerturbation)
		{
			mPopSize = aPopSize;
			mNumGenes = aNumGenes;
			mMutationRate = aMutRate;
			mCrossoverRate = aCrossRate;
			mPerturbation = aPerturbation;
			mPop = new List<Genome>();
			mNewBloods = new List<Genome>();
			mNumElites = aNumElites;
			Debug.Assert(mNumElites <= mPopSize);
			FitnessReset();
		}

		public void FitnessReset()
		{
			foreach (Genome g in mPop)	
				g.Fitness = 0;
			mBestFitness = 0;
			mAvgFitness = 0;
			mWorstFitness = 9999999;
		}

		private void Mutate(Genome genome)
		{
			for (int j = 0; j < mNumGenes; j++)
			{
				if (Util.Rand() < Config.MutationRate)
					genome.Genes[j] += (Util.Rand(-1.0, 1.0) * mPerturbation);
			}//end for
		} 

		private void Crossover(List<Genome> list, out Genome child1, out Genome child2)
		{
			int index1 = Util.Rand(0, list.Count);
			int index2 = index1;
			
			while(index2 == index1)
				index2 = Util.Rand(0, list.Count);
			
			//deep copy 
			child1 = Genome.Copy(list[index1]);
			child2 = Genome.Copy(list[index2]);

			//exchage each gene by Pr(CrossoverRate)
			for (int i = 0; i < mNumGenes; i++)
			{
				if (Util.Rand() < Config.CrossoverRate)
				{
					child1.Genes[i] = list[index2].Genes[i];
					child2.Genes[i] = list[index1].Genes[i];
				}
			}
		}

		public void AddNewBloods(List<Genome> newBloods)
		{
			mNewBloods = newBloods;
		}
		
		private double CalcFitnessAvg()
		{
			double avg = 0;
			foreach(Genome g in mPop)
				avg += g.Fitness;
			avg /= mPop.Count;
			return avg;
		}
		
		public void Evolve()
		{
			//Get elites
			mPop.Sort(new Genome.DescendComparer());

			List<Genome> elites = new List<Genome>();
			for (int i = 0; i < mNumElites; i++)
				elites.Add(mPop[i]);

			//output messages
			double best = mPop[0].Fitness;
			double worst = mPop[mPopSize-1].Fitness;
			double avg =CalcFitnessAvg();

			Console.Out.WriteLine();
			Console.Out.WriteLine("End Generation: {0}", mGeneration);
			Console.Out.WriteLine("Best Fitness: {0}", best);
			Console.Out.WriteLine("Average Fitness: {0}", avg);
			Console.Out.WriteLine("Worst Fitness: {0}", worst);
			Console.Out.WriteLine();
			
			List<Genome> newPopulation = new List<Genome>();
			
			int numOffspring = mPopSize - mNumElites - mNewBloods.Count;
			Debug.Assert( mNumElites + mNewBloods.Count + numOffspring == mPopSize);
			
			while(newPopulation.Count < numOffspring)
			{
				Genome child1, child2;
				Crossover(elites, out child1, out child2);
				Mutate(child1);
				Mutate(child2);
				newPopulation.Add(child1);
				newPopulation.Add(child2);
			}
			
			// if numOffspring is not even
			// we may have one extra child insereted in to list
			if(newPopulation.Count > numOffspring)
				newPopulation.RemoveAt(newPopulation.Count-1);
			
			foreach (Genome g in elites)
				newPopulation.Add(g);
			
			foreach (Genome g in mNewBloods)
				newPopulation.Add(g);		
			
			Debug.Assert(mPop.Count == newPopulation.Count);

			mPop.Clear();
			mPop = newPopulation;
			
			mGeneration++;
			
			//reset Fitness
			foreach (Genome g in mPop)	
				g.Fitness = 0;
		}
		 
		//test purpose only
		public void Display()
		{
			mPop.Sort(new Genome.AscendComparer());
			double sum = 0;
			foreach (Genome genome in mPop)
			{
				sum += genome.Fitness;
				Console.Out.WriteLine(genome.Fitness);
			}
			sum /= mPopSize;
			Console.Out.WriteLine("Avg:{0}", sum);
		}

		//test purpose only
		public void FillGenomeRandomly()
		{
			mPop.Clear();
			for (int i = 0; i < mPopSize; i++)
			{
				List<double> genes = new List<double>();
				double fitness = 0;
				for (int j = 0; j < mNumGenes; j++)
				{
					double val = Util.Rand(-10.0, 10.0);
					genes.Add(val);
				}
				mPop.Add(new Genome(genes, fitness));
			}
		}

	}//class GA
	 
}//namespace SmartFish
