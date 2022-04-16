using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using WpfDrawing.Backend;
using Brushes = System.Windows.Media.Brushes;
using MathNet.Numerics.LinearAlgebra;

namespace WpfDrawing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //initialize the dataconverter object 
        DataConverter dataConverter = new DataConverter();
        public MainWindow()
        {
            InitializeComponent();
            // set drawing color to black 
            MyCanvas.DefaultDrawingAttributes.Color = Colors.Black;
            // activate the ue of customized cursors 
            MyCanvas.UseCustomCursor = true;
            // border indicating the active tool of drawing 
            brushbrd.BorderBrush = Brushes.Blue;
            //initialize the cursor of the inkcanvas to pen cursor
            MyCanvas.Cursor = Cursors.Pen;
         

        }

        private void exitbtn_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void slide_value_change(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            /*Change the brush size from the slider value */
            MyCanvas.DefaultDrawingAttributes.Width = sizeSlide.Value;
            MyCanvas.DefaultDrawingAttributes.Height = sizeSlide.Value;
            sizelbl.Content = sizeSlide.Value.ToString();
        }

        private void brush_btn_down(object sender, MouseButtonEventArgs e)
        {
            /* logic for switching between the button and the eraser */
            brushbrd.BorderBrush = Brushes.Blue;
            MyCanvas.EditingMode = InkCanvasEditingMode.Ink;
            MyCanvas.Cursor = Cursors.Pen;
            erasebrd.BorderBrush = Brushes.Gray;
        }

        private void erase_btn_down(object sender, MouseButtonEventArgs e)
        {
            /* logic for switching between the button and the eraser */
            erasebrd.BorderBrush = Brushes.Blue;
            MyCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
            MyCanvas.Cursor = System.Windows.Input.Cursors.Cross;
            brushbrd.BorderBrush = Brushes.Gray;
        }

        private void clear_btn_click(object sender, RoutedEventArgs e)
        {
            MyCanvas.Strokes.Clear();
        }

        private void _3by3_btn_click(object sender, RoutedEventArgs e)
        {
            GetMatrix(3, 3);
            Console.WriteLine(dataConverter.getVector());
        }
        private void _4by4_btn_click(object sender, RoutedEventArgs e)
        {
            GetMatrix(4, 4);
        }
        private void _5by5_btn_click(object sender, RoutedEventArgs e)
        {
            GetMatrix(5, 5);   
        }
        private void _12by12_btn_click(object sender, RoutedEventArgs e)
        {
            GetMatrix(12, 12);
        
           
        }

        /// <summary>
        /// print the matrix on the interface an ivestigate visualy 
        /// </summary>
        /// <param name="src"> get Image source to show the matrix in the interface </param>
        public void DrawMatrix(ImageSource src)
        {
            try
            {
                Bitmap copy = dataConverter.ImageSourceToBitmap(src);
                double[,] matrix = dataConverter.ConvertToMatrix(dataConverter.ImageSourceToBitmap(src));
                DataConverter.imgPixelsMatrix = matrix;
                for (int i = 0; i < copy.Width; i++)
                {
                    this.txtMatrix.Text = this.txtMatrix.Text + Environment.NewLine;
                    for (int j = 0; j < copy.Height; j++)
                    {
                        this.txtMatrix.Text = this.txtMatrix.Text + matrix[i, j].ToString("0.00") + "|  ";
                    }
                    this.txtMatrix.Text = this.txtMatrix.Text + Environment.NewLine + "__________________________________________________________________________";

                }
            }
            catch (Exception ex)
            {
                txtMatrix.Text = ex.ToString();
            }
        }

        /// <summary>
        /// Get pixel numerical values from an Image converted to bitmap then converted to image source  
        /// </summary>
        /// <param name="width"> width in the unit pixels of the image </param>
        /// <param name="heigh"> width in the unit of pixels of image</param>
        public void GetMatrix(int width , int heigh)
        {
            this.txtMatrix.Text = "";
            BitmapSource img = dataConverter.ConvertToBitmapSource(MyCanvas);
            ImageSource img_src = dataConverter.ConvertToImageSource((dataConverter.ConvertToBitmap(img)), width, heigh);
            imgCanvas.Source = img_src;
            DrawMatrix(img_src);
        }

    
    }
}
