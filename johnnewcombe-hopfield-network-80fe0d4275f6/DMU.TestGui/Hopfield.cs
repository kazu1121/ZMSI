using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DMU.Math;

namespace DMU.Samples
{
    public partial class Hopfield : Form
    {

        private Matrix _wieghts;
        private double[] _pattern;

        private int _cellSize;

        DMU.NeuralNetwork.Hopfield _network = new DMU.NeuralNetwork.Hopfield();

        public Hopfield()
        {
            InitializeComponent();
        }

        #region Public Properties

        public int Cols { set; get; }
        public int Rows { set; get; }

        public Matrix Pattern
        {
            get
            {
                return new Matrix(_pattern, true);
            }
        }
        public Matrix Weights
        {
            get
            {
                return _wieghts;
            }
        }
        public Matrix TrainingData
        {
            set
            {
                _wieghts = _network.Train(value);
            }
        }

        #endregion

        private void Hopfield_Paint(object sender, PaintEventArgs e)
        {
            DrawGrid(e.Graphics);
        }

        private void Hopfield_Load(object sender, EventArgs e)
        {

            //sort out the cell size
            _cellSize = 400 / this.Cols;

            //resize the form and position the controls
            this.Width = this.Cols * _cellSize + 10;
            this.Height = this.Rows * _cellSize + 150; //leave enough room for a few buttons

            //position the buttons dynamically so that we can have different grid sizes
            btnTrain.Top = this.Rows * _cellSize + 20;
            btnTrain.Left = Convert.ToInt32(this.Width / 2) - btnTrain.Width - 5;
            btnTest.Top = btnTrain.Top;
            btnTest.Left = btnTrain.Left + btnTrain.Width + 10;
            btnClear.Top = btnTest.Top + btnTest.Height + 10;
            btnClear.Left = btnTrain.Left;
            btnClear.Width = btnTest.Left + btnTest.Width - btnTrain.Left;
            chkPostProcess.Left = btnClear.Left + (btnClear.Width / 2) - (chkPostProcess.Width /2);
            chkPostProcess.Top = btnClear.Top + btnClear.Height + 10;

            _pattern = new double[this.Cols * this.Rows];

        }
        /// <summary>
        /// Draws the grid to the Forms Graphics object.
        /// </summary>
        /// <param name="graphics"></param>
        private void DrawGrid(Graphics graphics)
        {
            //draw the grid to the forms Graphics object
            SolidBrush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(Color.Black);

            //loop through the x and y co-ordinates
            for (int y = 0; y < this.Rows; y++)
            {
                for (int x = 0; x < this.Cols; x++)
                {
                    if (_pattern[(y * this.Cols) + x] > 0)
                    {
                        //draw a filled in box
                        graphics.FillRectangle(brush, (x * _cellSize), this.Margin.Top + (y * _cellSize), _cellSize, _cellSize);
                    }
                    else
                    {
                        //draw an empty square
                        graphics.DrawRectangle(pen, (x * _cellSize), this.Margin.Top + (y * _cellSize), _cellSize, _cellSize);
                    }
                }
            }

        }

        /// <summary>
        /// This method provides post processing of the output. 
        /// This is typically used to detect and correct inversions.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private double[] PostProcess(double[] data)
        {
            int size = data.Count();
            int numberSet = data.Count(i => i == 1);

            //if more than half of the elements are set then invert them
            if (numberSet > size / 2)
            {
                //loop through the x and y co-ordinates
                for (int y = 0; y < this.Rows; y++)
                {
                    for (int x = 0; x < this.Cols; x++)
                    {
                        int index = (y * this.Cols) + x;
                        if (_pattern[index] > 0)
                            _pattern[index] = 0;
                        else
                            _pattern[index] = 1.0;
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// Converts the mouse click (down) co-ordinates to grid co-ordinates and toggles the
        /// cell of the grid as appropriate.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hopfield_MouseDown(object sender, MouseEventArgs e)
        {

            //get the x/y co-ordinates
            int x = Convert.ToInt32(e.X / _cellSize);
            int y = Convert.ToInt32(e.Y / _cellSize);

            //make sure the co-ordinates are withing the grid
            if (e.Button == MouseButtons.Right)
            {
                if (x >= 0 && x < this.Cols && y >= 0 & y < this.Rows)
                {
                    int index = (y * this.Cols) + x;
                    if (_pattern[index] > 0)
                        _pattern[index] = 0;
                    else
                        _pattern[index] = 1.0;

                    this.Invalidate();
                }
            }
        }

        private void Hopfield_MouseMove(object sender, MouseEventArgs e)
        {
            //get the x/y co-ordinates
            int x = Convert.ToInt32(e.X / _cellSize);
            int y = Convert.ToInt32(e.Y / _cellSize);

            if (e.Button == MouseButtons.Left)
            {

                if (x >= 0 && x < this.Cols && y >= 0 & y < this.Rows)
                {
                    int index = (y * this.Cols) + x;
                    _pattern[index] = 1.0;
                    this.Invalidate();
                }
            }
        }
        private void btnTrain_Click(object sender, EventArgs e)
        {
            btnTrain.Enabled = false;
            Matrix weights = _network.Train(new Matrix(_pattern, true));
            btnTrain.Enabled = true;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            btnTest.Enabled = false;

            Matrix output = _network.Test(new Matrix(_pattern, true));

            //post process if set
            if(chkPostProcess.Checked)
                _pattern = PostProcess(output.ToArray());
            else
                _pattern = output.ToArray();
            
            this.Invalidate();

            btnTest.Enabled = true;

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            btnClear.Enabled = false;

            //loop through the x and y co-ordinates
            for (int y = 0; y < this.Rows; y++)
            {
                for (int x = 0; x < this.Cols; x++)
                {
                    int index = (y * this.Cols) + x;
                    _pattern[index] = 0;
                }
            }
            this.Invalidate();

            btnClear.Enabled = true;
        }

    }
}