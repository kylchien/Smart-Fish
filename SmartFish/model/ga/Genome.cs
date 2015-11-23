/*
 * Created by SharpDevelop.
 * User: kchien
 * Date: 30/04/2012
 * Time: 4:27 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SmartFish
{
	public class Genome
	{

		public double Fitness { get; set; }

		public List<double> Genes { get; set; }

		public Genome()
		{
			Fitness = 0;
			Genes = new List<double>();
		}

		public Genome(List<double> aGenes, double aFitness)
		{
			Genes = aGenes;
			Fitness = aFitness;
		}
		
		public static Genome Copy(Genome src)
		{
			Genome copy = new Genome();
			foreach(double d in src.Genes)
				copy.Genes.Add(d);
			copy.Fitness = src.Fitness;
			return copy;
		}
		
		//Arrange Genome from lowest fintess to highest
		public class AscendComparer : IComparer<Genome>
		{
			public int Compare(Genome g1, Genome g2)
			{
				if (g1.Fitness > g2.Fitness)
					return 1;
				else if (g1.Fitness < g2.Fitness)
					return -1;
				return 0;
			}
		}//AscendComparer

		//Arrange Genome from highest fintess to lowest
		public class DescendComparer : IComparer<Genome>
		{
			public int Compare(Genome g1, Genome g2)
			{
				if (g1.Fitness > g2.Fitness)
					return -1;
				else if (g1.Fitness < g2.Fitness)
					return 1;
				return 0;
			}
		}//DescendComparer

	}//Class Genome
}
