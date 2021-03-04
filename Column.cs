using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRP_Calculator_V0._0
{
    class Column:Reinforcement
    {
        public double ColumnHeight { get; set; }
        public double ColumnWidth { get; set; }
        public double ClearCover { get; set;  }
        public double EffectiveDepth { get; set; }
        public double EffectiveDepthRatio { get; set; }
        public double ConcreteGrossArea { get; set; }
        public double FaceReinforcement { get; set; }
        public double SideReinforcement { get; set; }
        public double Ffu { get; set; }
        public int ConcreteStrength { get; set; }
        public double SteelQuantity { get; set; }
        public double SteelConcreteCapacityRatio { get; set; }
        public double ef { get; set; }
        public double e { get; set; }
        public double Beta_1 { get; set; }
        public double EffectiveDepthCoverRatio { get; set; }
        public double W1 { get; set;  }
        public double BalancedPoint { get { return 1 / (1 / +this.e); } }
        //Material Limit Variables 
        public double ConcreteEffectiveStrength { get; set; }
        public double F1 { get; set; }
        public double F2 { get; set; }
        public double M_c { get; set; }
        public double M_1 { get; set; }
        public double M_2 { get; set; }
        //Interaction Step Variables 
        public double step_1 { get { return (1 - this.BalancedPoint) / 18; } }
        public double step_2 { get; set; }

        //Step Collections for interaction curve
        public List<double> a = new List<double>();
        public List<double> kc = new List<double>();
        public List<double> k2 = new List<double>();
        public List<double> k_prime_c = new List<double>();
        public List<double> k_prime_1 = new List<double>();
        public List<double> k_prime_2 = new List<double>();
        public List<double> C = new List<double>();
        public List<double> T1 = new List<double>();
        public List<double> T2 = new List<double>();
        public List<double> Mc = new List<double>();
        public List<double> M1 = new List<double>();
        public List<double> M2 = new List<double>();
        public List<double> Mn = new List<double>();
        public List<double> Pn = new List<double>();

        public int Ef { get; set; }
        public Column()
        {
            //TODO
        }
 
        public void CalculateInitialParameters()
        {

        }
        public void GenerateInteractionTable()
        {
            //TODO
        }





    }
}
