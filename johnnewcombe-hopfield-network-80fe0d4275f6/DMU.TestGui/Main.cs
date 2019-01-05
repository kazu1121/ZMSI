using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DMU.Math;

namespace DMU.Samples
{
    public partial class Main : Form
    {

        public Main()
        {
            InitializeComponent();
        }


        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //show about box
        }

        private void Main_Load(object sender, EventArgs e)
        {
        }
        private void x2MatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hopfield frm = new Hopfield();
            frm.Cols = 2;
            frm.Rows = 2;
            frm.MdiParent = this;
            frm.Show();

        }
        private void x3MatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hopfield frm = new Hopfield();
            frm.Cols = 3;
            frm.Rows = 3;
            frm.MdiParent = this;
            frm.Show();
        }
        private void x4MatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hopfield frm = new Hopfield();
            frm.Cols = 4;
            frm.Rows = 4;
            frm.MdiParent = this;
            frm.Show();
        }
        private void x8MatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hopfield frm = new Hopfield();
            frm.Cols = 8;
            frm.Rows = 8;
            frm.MdiParent = this;
            frm.Show();

        }

        private void x16MatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hopfield frm = new Hopfield();
            frm.Cols = 16;
            frm.Rows = 16;
            frm.MdiParent = this;
            frm.Show();

        }
        private void x32MatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hopfield frm = new Hopfield();
            frm.Cols = 32;
            frm.Rows = 32;
            frm.MdiParent = this;
            frm.Show();

        }

        private void x64MatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hopfield frm = new Hopfield();
            frm.Cols = 64;
            frm.Rows = 64;
            frm.MdiParent = this;
            frm.Show();

        }
        private void saveWeightDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //open save dialogue
            string fileName = string.Empty;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Training Data (*.csv)|*.csv|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                fileName = saveFileDialog.FileName;
            }

            //need to get active form
            Hopfield frm = (Hopfield)this.ActiveMdiChild;
            Matrix data = frm.Pattern;

            //StringBuilder csv = new StringBuilder();
            string csv = data.ToString("f0", ",", "\r\n");

            //Write csv to file
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                StreamWriter file = new System.IO.StreamWriter(fileName);
                file.Write(csv);
                file.Close();
            }

        }

        private void loadTrainingDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = string.Empty;
            int rowCount = 0;
            int columnCount = 0;
            double[] patternData = new double[0];
            string line;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Training Data (*.csv)|*.csv|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                fileName = openFileDialog.FileName;
            }


            //check the filename
            if (!string.IsNullOrWhiteSpace(fileName))
            {

                try
                {
                    //need to get the line length, this is a bit of a fudge but is acceptable for now
                    //read the first line of the file
                    //StreamReader file = new System.IO.StreamReader(fileName);
                    //string line = file.ReadLine();
                    //lineLength = line == null ? 0 : line.Length;
                    //file.Close();

                    //read the file line by line
                    StreamReader file = new System.IO.StreamReader(fileName);
                    while ((line = file.ReadLine()) != null)
                    {
                        if (line.Length > 0)
                        {
                            rowCount++;
                            //this gets updated each time but should be the same on each iteration

                            //store the line length as this allows us to create the form dynamically from the data
                            double[] cells = ConvertToDoubleArray(line.Split(",".ToCharArray()));
                            columnCount = cells.Length;//this gets updated each time but should be the same on each iteration

                            //TODO: need to concatenate this array or build up a multidimensional one
                            // use rowCount as the index etc.
                            double[] temp = patternData;
                            patternData = new double[patternData.Length + columnCount];

                            //copy pattern data back in
                            Array.Copy(temp, patternData, temp.Length);

                            //add the new data
                            Array.Copy(cells, 0, patternData, (columnCount * rowCount) - columnCount, cells.Length);

                        }

                    }
                    file.Close();
                }

                catch (Exception ex)
                {
                    throw new ApplicationException("Data not valid.", ex);
                }

                //assume matrix is square
                Matrix trainingData = Matrix.Transpose(new Matrix(patternData, rowCount, columnCount));

                Hopfield frm = new Hopfield();
                frm.Cols = (int)System.Math.Sqrt(columnCount);
                frm.Rows = frm.Cols;
                frm.TrainingData = trainingData;
                Matrix weights = frm.Weights;
                frm.MdiParent = this;
                frm.Show();

            }
        }
        private double[] ConvertToDoubleArray(string[] data)
        {
            double[] result = new double[data.Length];

            for(int cell =0; cell < data.Length; cell++)
            {
                if (!double.TryParse(data[cell], out result[cell]))
                {
                    throw new ApplicationException("Data not valid.");
                }
            }

            return result;
        }

    }
}
