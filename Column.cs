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
        public double step_2 { get ; set; }

        //Step Collections for interaction curve
        public List<double> a = new List<double>();
        public List<double> kc = new List<double>();
        public List<double> k1 = new List<double>();
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
        public List<double> Phi = new List<double>();
        public List<double> Phi_Mn = new List<double>();
        public List<double> Phi_Pn = new List<double>();

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
            double neutralAxisLimit = ColumnHeight / EffectiveDepth;
            int index = 1;
            a.Add(BalancedPoint);
            while (a[index] < neutralAxisLimit)
            {
                a.Add(a[index - 1] + step_1);
                kc.Add(a[index] * Beta_1 * EffectiveDepthRatio);
                k1.Add((1 - a[index]) / (a[index] * 0.003));
                k2.Add(((EffectiveDepthRatio) / (2 * EffectiveDepthRatio - 1)) * Math.Pow(1 - a[index], 2) / (a[index] * 0.003));
                k_prime_c.Add(kc[index] * (1 - a[index] * Beta_1 * EffectiveDepthRatio) / 2);
                k_prime_1.Add(k1[index] * (EffectiveDepthRatio - 0.5));
                k_prime_2.Add(k2[index] * (2 + a[index] * EffectiveDepthRatio - 0.5));
                C.Add(kc[index] * ConcreteEffectiveStrength);
                T1.Add(k1[index] * F1);
                T2.Add(k2[index] * F2);
                Mc.Add(k_prime_c[index] * M_c);
                M1.Add(k_prime_1[index] * M_1);
                M2.Add(k_prime_2[index] * M_2);
                Mn.Add((Mc[index] + M1[index] + M2[index]) / 12);
                Pn.Add((C[index] - T1[index] - T2[index]));
                Phi.Add(Math.Min(Math.Max(0.55, (0.3 + 0.25 * e * (1 + e) * Math.Pow(a[index], 2)) / (1 - a[index])), 0.65));
                Phi_Mn.Add(Phi[index] * Mn[index]);
                Phi_Pn.Add(Phi[index] * Mn[index]);

            }
        }





    }
}
