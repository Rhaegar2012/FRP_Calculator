using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRP_Calculator_V0._0
{
    class Column:Reinforcement
    {
        private double ColumnHeight { get; set; }
        private double ColumnWidth { get; set; }
        private double ClearCover { get; set;  }
        private double EffectiveDepth { get { return this.ColumnHeight - this.ClearCover; } }
        private double EffectiveDepthRatio { get { return this.EffectiveDepth / this.ColumnHeight; } }
        private double ConcreteGrossArea { get { return this.ColumnHeight * this.ColumnWidth; } }
        private double FaceReinforcement { get; set; }
        private double SideReinforcement { get; set; }
        private string rebarSize { get; set; }
        private double Ffu { get { return AssignFFu(rebarSize); } }
        private int ConcreteStrength { get; set; }
        private double SteelQuantity { get { return (this.FaceReinforcement + this.SideReinforcement) / (this.ConcreteGrossArea)*100; } }
        private double SteelConcreteCapacityRatio { get { return ((this.Ffu * this.SteelQuantity) / this.ConcreteStrength) / 100; } }
        private double ef { get {return this.Ffu /((double) MOE.MOE); } }
        private double e { get { return this.ef / 0.003; } }
        private double Beta_1 { get { return Math.Min(0.85, Math.Max(1.05 - 0.05 * this.ConcreteStrength, 0.65)); } }
        private double W1 { get; set;  }
        private double BalancedPoint { get { return 1 / (1 / +this.e); } }
        private double EffectiveDepthCoverRatio { get{ return this.ClearCover / this.EffectiveDepth; } }
        //Material Limit Variables 
        private double ConcreteEffectiveStrength { get { return this.ConcreteGrossArea * this.ConcreteStrength; } }
        private double F1 { get { return this.FaceReinforcement * (double) MOE.MOE; } }
        private double F2 { get { return this.SideReinforcement * (double) MOE.MOE; } }
        private double M_c { get { return this.ConcreteEffectiveStrength * this.ColumnHeight; } }
        private double M_1 { get { return this.F1 * this.ColumnHeight; } }
        private double M_2 { get { return this.F2 * this.ColumnHeight; } }
        //Interaction Step Variables 
        private double step_1 { get { return (1 - this.BalancedPoint) / 18; } }
        private double step_2 { get ; set; }

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
        public Column(double ColumnHeight, double ColumnWidth, double ClearCover, double FaceReinforcement, double SideReinforcement,int ConcreteStrength, string rebarSize)
        {
            this.ColumnHeight = ColumnHeight;
            this.ColumnWidth = ColumnWidth;
            this.ClearCover = ClearCover;
            this.FaceReinforcement = FaceReinforcement;
            this.SideReinforcement = SideReinforcement;
            this.ConcreteStrength = ConcreteStrength;
            this.rebarSize = rebarSize;
        }
 
      
        public void GenerateInteractionTable()
        {
            double neutralAxisLimit = ColumnHeight / EffectiveDepth;
            int index = 1;
            //Initial Step
            a.Add(BalancedPoint);
            kc.Add(0);
            k1.Add(2 * this.e * 0.003);
            k2.Add(2 * this.e * 0.003);
            k_prime_c.Add(0);
            k_prime_1.Add(0);
            k_prime_2.Add(0);
            C.Add(kc[0] * this.ConcreteEffectiveStrength);
            T1.Add(k1[0] * this.F1);
            T2.Add(k2[0] * this.F2);
            Mc.Add(k_prime_c[0] * M_c);
            M1.Add(k_prime_1[0] * M_1);
            M2.Add(k_prime_2[0] * M_2);
            Mn.Add((Mc[0] + M1[0] + M2[0]) / 12);
            Pn.Add(C[0] - T1[0] - T2[0]);
            Phi.Add(0.55);
            Phi_Mn.Add(Mn[0] * Phi[0]);
            Phi_Pn.Add(Pn[0] * Phi[0]);


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
                index++;

            }
        }
        public void ColumnDebug()
        {
            for (int i = 0; i < a.Count; i++)
            {
                Console.WriteLine(a[i]+" "+
                    kc[i]+" "+
                    k1[i]+" "+
                    k2[i]+" "+
                    k_prime_c[i]+" "+
                    k_prime_1[i]+" "+
                    k_prime_2[i]+" "+
                    C[i]+" "+
                    T1[i]+" "+
                    T2[i]+" "+
                    )
            }
        }
        public string CalculateInteractionRatio()
        {
            //TODO
            double interactionRatio=0.0;
            return interactionRatio.ToString();
        }






    }
}
