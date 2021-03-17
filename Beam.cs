using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRP_Calculator_V0._0
{
    class Beam:Reinforcement
    {
        //Input Variables
        public int Fc { get; set; }
        public int Ef { get; set; }
        public string RebarSize { get; set; }
        public string ExposedCondition { get; set; }
        public string FiberType { get; set; }
        private double Ce { get { return AssignCe(this.ExposedCondition, this.FiberType); } }
        public double Ffu { get { return AssignFFu(RebarSize); } }
        public int BeamWidth { get; set; }
        public int BeamHeight { get; set; }
        public double ClearCover { get; set; }
        public double BendingReinf { get; set; }
        public double ShearReinf { get; set; }
        public int StirrupSpacing { get; set; }
        public double UltimateMoment { get; set; }
        public double UltimateShear { get; set; }
        public double BendingRatio { get; set; }
        public double ShearRatio { get; set;}
        //public int StirrupSpacing { get; set; }
        public double ShearReinforcementStrength { get { return Ffu * 0.8; }  }
        private double BendingCapacity { get; set; }
        private const double Phi_shear = 0.75;
      
        //Constructor
        public Beam(int Fc, int BeamWidth,int BeamHeight, double ClearCover,
            double BendingReinf, double ShearReinf, int StirrupSpacing,
            string RebarSize,double UltimateMoment,double UltimateShear,string ExposedCondition,string FiberType)
        {
            this.Fc = Fc;
            this.Ef = (int)MOE.MOE;
            this.BeamWidth = BeamWidth;
            this.BeamHeight = BeamHeight;
            this.ClearCover = ClearCover;
            this.BendingReinf = BendingReinf;
            this.ShearReinf = ShearReinf;
            this.StirrupSpacing = StirrupSpacing;
            this.RebarSize = RebarSize;
            this.UltimateMoment = UltimateMoment;
            this.UltimateShear = UltimateShear;
            this.ExposedCondition = ExposedCondition;
            this.FiberType = FiberType;

        }

        //State Variables 
        private double Beta_1 { get
            {
                double value_1 = 1.05 - 0.05 * this.Fc;
                double value_2 = 0.85;
                double value_3 = 0.65;
                return (Math.Max(Math.Min(value_1, value_2), value_3));

            } 
        }
        private double EffectiveDepth
        {
            get
            {
                
                return this.BeamHeight - this.ClearCover;
            }
        }
        private double BendingSteelQuantity
        {
            get
            {
                return (this.BendingReinf) / (this.BeamWidth * this.EffectiveDepth);
            }
        }

        private double ReinforcementIndex
        {
            get
            {
                return (this.BendingSteelQuantity * this.Ffu / this.Fc);
            }
        }
        private double UltimateTensileStrain
        {
            get
            {
                return (this.Ffu / this.Ef);
            }
        }
        private double TensileStrainRatio
        {
            get
            {
                return (this.UltimateTensileStrain / 0.003);
            }
        }
        private double TensileReinforcementIndex
        {
            get
            {
                
                return (0.85 * (this.Beta_1) / (1 + this.TensileStrainRatio));
            }
        }
        private double ReinforcementIndexRatio
        {
            get
            {
                return (this.ReinforcementIndex / this.TensileReinforcementIndex);
            }
        }
        private double TensileStressRatio
        {
            get
            {
                double value_1 =(1/(2*TensileStrainRatio))*(Math.Sqrt( 1 + 3.4 * this.Beta_1 * (this.TensileStrainRatio / this.ReinforcementIndex))-1);
                double value_2 = 1;
                return Math.Min(value_1, value_2);
                
            }
        }
        private double FRPReinforcementFinalRatio
        {
            get
            {
                
                return TensileStressRatio * ReinforcementIndex;
            }
        }
        private double MomentLeveltoReinforcementRatio
        {
            get
            {
                
                double value_1 = (1 - FRPReinforcementFinalRatio/1.7) ;
                double value_2 = (1 - TensileReinforcementIndex/1.7) ;
                return (Math.Min(value_1, value_2));
            }
        }
        private double Phi_bending
        {
            get
            {
                return Math.Min(Math.Max(0.3 + 0.25 * this.ReinforcementIndexRatio, 0.55), 0.65);
            }
        }

        //State variables-Shear
        private double ConcreteModulusOfElasticity {
            get {
                return 57*Math.Sqrt((Fc) * 1000);
            }
        }
        private double ModulusOfElasticityRatio { 
            get 
            {
                return Ef/ConcreteModulusOfElasticity ;
            } 
        }
        private double ModulusOfElasticityRatioTimesQuantity {
            get 
            {
                Console.WriteLine("Concrete modulus of Elasticity" + ConcreteModulusOfElasticity);
                Console.WriteLine("Modulus of Elasticity ratio" + ModulusOfElasticityRatio);
                Console.WriteLine("Steel Quantity" + BendingSteelQuantity);
                return ModulusOfElasticityRatio * BendingSteelQuantity;
            }
        }
        private double RatioNeutralAxis { 
            get 
            {
                return Math.Sqrt(Math.Pow(ModulusOfElasticityRatioTimesQuantity, 2) +
                    2 * ModulusOfElasticityRatioTimesQuantity) -
                    ModulusOfElasticityRatioTimesQuantity;
                
            } 
        }
        private double ConcreteShearStrength { 
            get 
            {
                Console.WriteLine(this.RatioNeutralAxis);
                return 2.5 * this.EffectiveDepth * this.BeamWidth * this.RatioNeutralAxis *
                    2 * Math.Sqrt(this.Fc * 1000) / 1000;
            } 
        }
        private double SteelShearStrength { 
            get
            {
                return 0.004 * this.Ef;
            }
        }
        private double StrengthOfTieBent
        {
            get
            {
                return 0.45 * this.ShearReinforcementStrength;
            }
        }
        private double FRPTensileStrengthInShear
        {
            get
            {
                return Math.Min(this.StrengthOfTieBent, this.SteelShearStrength);
            }
        }
        private double SectionShearStrength
        {
            get
            {
                return (this.FRPTensileStrengthInShear * this.ShearReinf * this.EffectiveDepth) / (this.StirrupSpacing);
            }
        }

        //Output Variables
        public double NominalMoment { get; set; }
        public double NominalShear { get; set; }
        

        public void BeamShearCalculations()
        {
            NominalShear = Math.Round(Phi_shear * (this.SectionShearStrength + this.ConcreteShearStrength),2);
            ShearRatio = Math.Round(UltimateShear / NominalShear,2);
        }
        public void BeamBendingCalculations()
        {
       
           double MomentCapacity = (this.Fc * this.BeamWidth *Math.Pow( this.EffectiveDepth,2) * this.FRPReinforcementFinalRatio *
                this.MomentLeveltoReinforcementRatio) / 12;
            NominalMoment =Math.Round( Phi_bending * MomentCapacity,2);
            BendingRatio = Math.Round(UltimateMoment /NominalMoment ,2);
        }
     
        
        public void GenerateReport()
        {
           
        }

    }
}
