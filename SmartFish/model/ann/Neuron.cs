using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;



namespace SmartFish
{
	class Neuron
	{
        public int NumInput { get; protected set; }

        protected List<double> mWeights;

        //return read only weight collection
        public IList<double> Weights { 
            get{
                IList<double> roWeights = mWeights.AsReadOnly();
                return roWeights;
            }
        }
		
		public double Bias { get; protected set; }

		private int mID = -1;
		public int ID { get{return mID;} }
		
		public Neuron(int id, int aNumInput)
		{
			mID = id;

            NumInput = aNumInput;
			mWeights = new List<double>();

            for (int i = 0; i < NumInput; i++)
			{
				mWeights.Add(Util.Rand(-1.0,1.0));
			}
            
            /*option 1 
            //Bias = Util.Rand(-1.0, 1.0) + 0.01;
            
            //option 2  
            //Bias = 2 * Util.Rand(-1.0, 1.0);
            //if (Bias > 1) Bias = 1;
            //if (Bias < -1) Bias = -1;
            */
            //option 3
            Bias =  0.0001;
		}

        public void Update(List<double> iWeights, double iBias)
        {
            mWeights = iWeights;
            Bias = iBias;
        }

		private double sigmoid(double netinput, double response)
		{
			return (1 / (1 + Math.Exp(-netinput / response)));
		}

		public double Output(List<double> inputs)
		{
			double netinput = 0;
            for (int i = 0; i < NumInput; i++)
				netinput += inputs[i] * mWeights[i];
			
			netinput += Bias * Config.BiasFactor;
			return sigmoid(netinput, Config.ActivationReponse);
		}
		
		public void WriteToXML(XmlWriter writer)
		{
			writer.WriteStartElement("Neuron");
			writer.WriteAttributeString("ID", Convert.ToString(mID));	
			String weightStr="";
			for (int i = 0; i < mWeights.Count; i++)
			{
				//truncate to 3 decimal places
				weightStr += String.Format("{0:0.000}", mWeights[i]);
				if (i != mWeights.Count - 1)
					weightStr += ", ";
			}
			writer.WriteAttributeString("Weights", weightStr);
			writer.WriteAttributeString("Bias", String.Format("{0:0.000}", Bias));
			writer.WriteEndElement();
			writer.Flush();
		}

	}
}
