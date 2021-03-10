using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FRP_Calculator_V0._0
{
    public partial class Form1 : Form
    {
        //Beam State variables 
        int ConcreteStrength;
        int BeamWidth;
        int BeamHeight;
        double ClearCover;
        double BendingReinf;
        double ShearReinf;
        int StirrupSpacing;
        string RebarSize;
        double UltimateMoment;
        double UltimateShear;
        private TextBox[] beaminputFields;
        //Column State variables 
        int Col_ConcreteStrength;
        string Col_RebarSize;
        double Col_ClearCover;
        double ColumnWidth;
        double ColumnHeight;
        double FaceReinforcement;
        double SideReinforcement;
        private TextBox[] columninputFields;
        
    
        Beam currentBeam;
        Column currentColumn;
        public Form1()
        {
            InitializeComponent();
            beaminputFields = new TextBox[] {inputConcreteStrength,
                                         inputBeamWidth,
                                         inputBeamHeight,
                                         inputClearCover,
                                         inputConcreteStrength,
                                         inputBendingReinf,
                                         inputShearReinf,
                                         inputStirrupSpacing,
                                         bendingMomentInput,
                                         shearForceInput};
            columninputFields = new TextBox[] {inputConcreteStrengthColumn,
                                               inputColumnHeight,
                                               inputColumnWidth,
                                               inputColumnFaceReinforcement,
                                               inputColumnSideReinforcement};
        }
        private void initializeBeam()
        {

            ConcreteStrength = Int32.Parse(inputConcreteStrength.Text);
            BeamWidth = Int32.Parse(inputBeamWidth.Text);
            BeamHeight = Int32.Parse(inputBeamHeight.Text);
            ClearCover = double.Parse(inputClearCover.Text);
            BendingReinf = double.Parse(inputBendingReinf.Text);
            ShearReinf = double.Parse(inputShearReinf.Text);
            StirrupSpacing = Int32.Parse(inputStirrupSpacing.Text);
            UltimateMoment = double.Parse(bendingMomentInput.Text);
            UltimateShear = double.Parse(shearForceInput.Text);

            RebarSize = rebarComboBox.Text;
            currentBeam = new Beam(ConcreteStrength, BeamWidth, BeamHeight, ClearCover, BendingReinf, ShearReinf, StirrupSpacing,RebarSize,UltimateMoment,UltimateShear);
           
        }
        private void initializeColumn()
        {
            Col_ConcreteStrength = Int32.Parse(inputConcreteStrengthColumn.Text);
            ColumnWidth = double.Parse(inputColumnWidth.Text);
            ColumnHeight = double.Parse(inputColumnHeight.Text);
            Col_ClearCover = double.Parse(inputClearCoverColumn.Text);
            FaceReinforcement = double.Parse(inputColumnFaceReinforcement.Text);
            SideReinforcement = double.Parse(inputColumnSideReinforcement.Text);
            Col_RebarSize = rebarComboBoxColumn.Text;
            currentColumn = new Column(ColumnHeight, ColumnWidth, Col_ClearCover, FaceReinforcement, SideReinforcement, Col_ConcreteStrength,Col_RebarSize);

        }
        private bool checkParameters()
        {
            for(int i = 0; i < beaminputFields.Length; i++)
            {
                if (beaminputFields[i].Text == "")
                {
                    Console.WriteLine("Accesed");
                    MessageBox.Show("Input all beam parameters", "Missing data");
                    return false;
                }
            }
            
            return true;
           
        }
        private bool checkColumnParameters()
        {
            for(int i=0; i < columninputFields.Length; i++)
            {
                if (columninputFields[i].Text == "")
                {
                    MessageBox.Show("Input all column parameters", "Missing data");
                    return false;
                }
            }
            return true;
        }
        
        private void updateForm()
        {
            if (currentBeam != null)
            {
                beamMomentResult.Text = currentBeam.NominalMoment.ToString() + " kip-ft";
                beamShearResult.Text = currentBeam.NominalShear.ToString() + " kips";
                beamMomentRatio.Text = currentBeam.BendingRatio.ToString();
                shearRatio.Text = currentBeam.ShearRatio.ToString();
            }
            if (currentColumn != null)
            {
                interactionResult.Text = currentColumn.CalculateInteractionRatio();
            }
           
        }

        private void checkBeamButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Pressed");
            if (checkParameters())
            {
                initializeBeam();
                currentBeam.BeamBendingCalculations();
                currentBeam.BeamShearCalculations();
                updateForm();
            }
        }

        private void checkColumnButton_Click(object sender, EventArgs e)
        {
            if (checkColumnParameters())
            {
                initializeColumn();
                currentColumn.CalculateInitialParameters();
                currentColumn.GenerateInteractionTable();
            }
        }
    }
}
