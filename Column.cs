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
        private double UltimateMoment { get; set; }
        private double UltimateAxial { get; set; }
        private string rebarSize { get; set; }
        private string ExposedCondition { get; set; }
        private string FiberType { get; set; }
        private double Ce { get { return AssignCe(this.ExposedCondition, this.FiberType); } }
        private double Ffu { get { return AssignFFu(this.rebarSize,this.ExposedCondition,this.FiberType); } }
        private int ConcreteStrength { get; set; }
        private double SteelQuantity { get { return (this.FaceReinforcement + this.SideReinforcement) / (this.ConcreteGrossArea)*100; } }
        private double SteelConcreteCapacityRatio { get { return ((this.Ffu * this.SteelQuantity) / this.ConcreteStrength) / 100; } }
        private double ef { get {return this.Ffu /((double) MOE.MOE); } }
        private double e { get { return this.ef / 0.003; } }
        private double Beta_1 { get { return Math.Min(0.85, Math.Max(1.05 - 0.05 * this.ConcreteStrength, 0.65)); } }
        private double W1 { get; set;  }
        private double BalancedPoint { get { return 1 / (1+this.e); } }
        private double EffectiveDepthCoverRatio { get{ return this.ClearCover / this.EffectiveDepth; } }
        public double InteractionRatio;
        //Material Limit Variables 
        private double ConcreteEffectiveStrength { get { return this.ConcreteGrossArea * this.ConcreteStrength; } }
        private double F1 { get { return this.FaceReinforcement * (double)MOE.MOE; } }
        private double F2 { get { return this.SideReinforcement * (double) MOE.MOE; } }
        private double M_c { get { return this.ConcreteEffectiveStrength * this.ColumnHeight; } }
        private double M_1 { get { return this.F1 * this.ColumnHeight; } }
        private double M_2 { get { return this.F2 * this.ColumnHeight; } }
        //Interaction Step Variables 
        private double step_1 { get { return (1 - this.BalancedPoint) / 18; } }
        private double step_2 { get ; set; }

        //Step Collections for interaction curve
        private List<double> a = new List<double>();
        private List<double> kc = new List<double>();
        private List<double> k1 = new List<double>();
        private List<double> k2 = new List<double>();
        private List<double> k_prime_c = new List<double>();
        private List<double> k_prime_1 = new List<double>();
        private List<double> k_prime_2 = new List<double>();
        private List<double> C = new List<double>();
        private List<double> T1 = new List<double>();
        private List<double> T2 = new List<double>();
        private List<double> Mc = new List<double>();
        private List<double> M1 = new List<double>();
        private List<double> M2 = new List<double>();
        private List<double> Mn = new List<double>();
        private List<double> Pn = new List<double>();
        private List<double> Phi = new List<double>();
        private List<double> Phi_Mn = new List<double>();
        private List<double> Phi_Pn = new List<double>();

        public int Ef { get; set; }
        public Column(double ColumnHeight, double ColumnWidth, double ClearCover, 
            double FaceReinforcement, double SideReinforcement,
            int ConcreteStrength, string rebarSize,string ExposedCondition,
            string FiberType,double UltimateMoment,double UltimateAxial)
        {
            this.ColumnHeight = ColumnHeight;
            this.ColumnWidth = ColumnWidth;
            this.ClearCover = ClearCover;
            this.FaceReinforcement = FaceReinforcement;
            this.SideReinforcement = SideReinforcement;
            this.ConcreteStrength = ConcreteStrength;
            this.rebarSize = rebarSize;
            this.ExposedCondition = ExposedCondition;
            this.FiberType = FiberType;
            this.UltimateMoment = UltimateMoment;
            this.UltimateAxial = UltimateAxial;
        }
 
      
        public void GenerateInteractionTable()
        {
            double neutralAxisLimit = ColumnHeight / EffectiveDepth;
            Console.WriteLine("Ffu " + this.Ffu);
            Console.WriteLine("neutral axis " + neutralAxisLimit);
            Console.WriteLine("Balance point" + this.BalancedPoint);
            int index = 0;
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
                a.Add(a[index] + step_1);
                kc.Add(a[index] * Beta_1 * EffectiveDepthRatio);
                k1.Add((1 - a[index]) / (a[index]) * 0.003);
                k2.Add(this.EffectiveDepthRatio/(2*this.EffectiveDepthRatio-1)*((Math.Pow(1-a[index],2)/a[index]*0.003)));
                k_prime_c.Add(kc[index] * (1 - a[index] * this.Beta_1 * this.EffectiveDepthRatio) / 2);
                k_prime_1.Add(k1[index] * (this.EffectiveDepthRatio - 0.5));
                k_prime_2.Add(k2[index] * (2 + a[index] * EffectiveDepthRatio - 0.5));
                C.Add(kc[index] * this.ConcreteEffectiveStrength);
                T1.Add(k1[index] * this.F1);
                T2.Add(k2[index] *this. F2);
                Mc.Add(k_prime_c[index] * M_c);
                M1.Add(k_prime_1[index] * M_1);
                M2.Add(k_prime_2[index] * M_2);
                Mn.Add((Mc[index] + M1[index] + M2[index]) / 12);
                Pn.Add((C[index] - T1[index] - T2[index]));
                Phi.Add(Math.Min(Math.Max(0.55, (0.3 + 0.25 * e * (1 + e) * Math.Pow(a[index], 2)) / (1 - a[index])), 0.65));
                Phi_Mn.Add(Phi[index] * Mn[index]);
                Phi_Pn.Add(Phi[index] * Pn[index]);
                index++;

            }
        }
        public void ColumnDebug()
        {
            Console.WriteLine(a.Count+" Interaction Curve Size");
            Console.WriteLine(this.Ffu+"FRP Strength");
            Console.WriteLine(this.ConcreteEffectiveStrength + "Fc");
            Console.WriteLine(this.F1 + "F1");
            Console.WriteLine(this.F2 + "F2");
            for (int i = 0; i < a.Count; i++)
            {   
                /*Console.WriteLine(a[i] + " " +
                    kc[i] + "KC " +
                    k1[i] + " K1" +
                    k2[i] + " K2" +
                    k_prime_c[i] + "K'C " +
                    k_prime_1[i] + "K'1 " +
                    k_prime_2[i] + "K'2 " +
                    C[i] + "C "  +
                   
                    T1[i] + " " +
                    T2[i] + " " +
                    Mc[i] + " " +
                    M1[i] + " " +
                    M2[i] + " "
                    );*/
               
                Console.WriteLine(Phi[i]+" "+
                    Phi_Mn[i] + " " +
                    Phi_Pn[i]);
            }
        }
        private bool CheckColumnFailure(double MomentDemand, double AxialDemand)
        {
            double Max_Moment = Phi_Mn.Max<double>();
            double Max_Axial = Phi_Pn.Max<double>();
            Console.WriteLine(Max_Moment + " Max Moment");
            if (MomentDemand > Max_Moment || AxialDemand>Max_Axial ) 
            {
                return true;
            }
            return false;
        }
        public string CalculateInteractionRatio(double MomentDemand,double AxialDemand)
        {
            //TODO
            double ClosestMoment;
            double ClosestAxial;
            double min_distance=100000;
            //Checks if the load demand point is within the interaction curve
            if (CheckColumnFailure(MomentDemand, AxialDemand))
            {
                return "Column failed for interaction stress";
            }
            else
            {
                for(int i = 0; i < Phi_Mn.Count; i++)
                {
                    double distance = Math.Sqrt(Math.Pow(MomentDemand - Phi_Mn[i], 2) +
                        Math.Pow(AxialDemand + Phi_Pn[i], 2));
                    if (distance < min_distance)
                    {
                        min_distance = distance;
                        ClosestMoment = Phi_Mn[i];
                        ClosestAxial = Phi_Pn[i];
                        this.InteractionRatio = (ClosestMoment / MomentDemand);

                    }
                   
                }
                
            }
            Console.WriteLine(this.InteractionRatio + " Interaction Ratio");
            return this.InteractionRatio.ToString();
        }






    }
}
