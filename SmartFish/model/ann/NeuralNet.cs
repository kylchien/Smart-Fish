using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace SmartFish
{

	public class NeuralNet
	{
		protected int mNumInputs;
        protected int mNumOutputs;
        protected int mNumHiddenLayers;
        protected int mNeuronsPerHiddenLayer;

		//storage for each layer of neurons including the output layer
		List<NeuronLayer> mLayers = new List<NeuronLayer>();

		public NeuralNet()
		{
			mNumInputs = Config.NumInput;
			mNumOutputs = Config.NumOutput;
			mNumHiddenLayers = Config.NumHiddenLayer;
			mNeuronsPerHiddenLayer = Config.NumNeuronsPerHiddenLayer;

			Initialize();
		}

		/// <summary>
		/// Create each NeuronLayer
		/// </summary>
		public void Initialize()
		{	
			//create the layers of the network
			if (mNumHiddenLayers > 0)
			{
				int id = 0;
				
				//create first hidden layer
				mLayers.Add(new NeuronLayer(id, mNeuronsPerHiddenLayer, mNumInputs));
				
				for (int i = 0; i < mNumHiddenLayers - 1; i++)
					mLayers.Add(new NeuronLayer(++id,mNeuronsPerHiddenLayer, mNeuronsPerHiddenLayer));
		
				//create output layer
				mLayers.Add(new NeuronLayer(++id,mNumOutputs, mNeuronsPerHiddenLayer));
			}
			else  //create output layer
				mLayers.Add(new NeuronLayer(0, mNumOutputs, mNumInputs));
		}

		/// <summary>
		/// Construct a vector that contains all the weights and bias from neurons
		/// Layout like the following:
		/// [weights1][bias1][weights2][bias2] [weights3][bias3]...
		/// </summary>
        public List<double> GetWeights()
		{
			List<double> oWeights = new List<double>();

			//for each layer
			for (int i = 0; i < mNumHiddenLayers + 1; i++)
			{
				//for each neuron
				for (int j = 0; j < mLayers[i].NumNeurons; j++)
				{
                    foreach (double d in mLayers[i].Neurons[j].Weights)
                        oWeights.Add(d);
                    
                    oWeights.Add(mLayers[i].Neurons[j].Bias);
				}
			}
			return oWeights;
		}
	
		public void SetWeights(List<double> iWeights)
		{
			int index = 0;
			//for each layer
			for (int i = 0; i < mNumHiddenLayers + 1; i++)
			{
				//for each neuron
				for (int j = 0; j < mLayers[i].NumNeurons; j++)
				{
					List<double> weights = new List<double>();
					for (int k = 0; k < mLayers[i].Neurons[j].NumInput; k++)
					{
						weights.Add(iWeights[index++]);
					}
					double bias = iWeights[index++];

                    //update weights and bias
                    mLayers[i].Neurons[j].Update(weights, bias);

				}//for
			}//for
		}

		public List<double> Output(List<double> inputs)
		{
			List<double> outputs = new List<double>();

			//first check that we have the correct amount of inputs
			if (inputs.Count != mNumInputs)
                return outputs; //just return an empty vector if incorrect.

			//For each layer....
			for (int i = 0; i< mNumHiddenLayers + 1; ++i)
			{	
	            //after first layer, 
                //current layer inputs are upper layer's outputs
				if ( i > 0 ) inputs = outputs;
				outputs = mLayers[i].Output(inputs);
			}//end for
			return outputs;
		}

		public void WriteToXML(XmlWriter writer)
		{
			writer.WriteStartElement("NeuralNet");
			writer.WriteAttributeString("NumInputs", Convert.ToString(mNumInputs));
			writer.WriteAttributeString("NumOutputs", Convert.ToString(mNumOutputs));
			writer.WriteAttributeString("NumHiddenLayers", Convert.ToString(mNumHiddenLayers));
			foreach (NeuronLayer layer in mLayers)
				layer.WriteToXML(writer);
			writer.WriteEndElement();
			writer.Flush();
		}


	}//class

}//namespace

