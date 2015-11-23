using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;



namespace SmartFish
{
	class NeuronLayer
	{
		private int mNumNeurons;
		public int NumNeurons { get { return mNumNeurons;  } }

		private int mNumInputsPerNeuron;
		public int NumInputsPerNeuron { get { return mNumInputsPerNeuron; } }

		private List<Neuron> mNeurons;
		public List<Neuron> Neurons { get { return mNeurons; }  }

		private int mID = -1;
		public int ID { get{return mID;} }
		
		public NeuronLayer(int id, int numNeurons, int numInputsPerNeuron)
		{
			mID = id;
			mNumNeurons = numNeurons;
			mNumInputsPerNeuron = numInputsPerNeuron;

			mNeurons = new List<Neuron>();
			for (int i = 0; i < mNumNeurons; i++)
				mNeurons.Add(new Neuron(i,mNumInputsPerNeuron));
		}
		
		public List<double> Output(List<double> inputs)
		{
			//Console.Out.WriteLine("inputs.count: {0}, NumInputsPerNeuron: {1}", inputs.Count, mNumInputsPerNeuron);
			Debug.Assert(inputs.Count == mNumInputsPerNeuron);
			
			List<double> outputs = new List<double>();
			foreach(Neuron n in mNeurons)
			{
				outputs.Add(n.Output(inputs));
			}
			return outputs;
		}


		public void WriteToXML(XmlWriter writer)
		{
			writer.WriteStartElement("NeuronLayer");
			writer.WriteAttributeString("ID", Convert.ToString(mID));
			writer.WriteAttributeString("NumNeurons", Convert.ToString(mNumNeurons));
			writer.WriteAttributeString("NumInputsPerNeuron", Convert.ToString(mNumInputsPerNeuron));
			writer.Flush();
			foreach (Neuron n in mNeurons)
				n.WriteToXML(writer);
			writer.WriteEndElement();
			writer.Flush();
		}


	}
}
