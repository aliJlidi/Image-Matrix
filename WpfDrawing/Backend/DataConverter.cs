using MathNet.Numerics.LinearAlgebra;
using System;

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Point = System.Windows.Point;

namespace WpfDrawing.Backend
{
    class DataConverter
    {
        public static double[,] imgPixelsMatrix; 
        public DataConverter()
        {

        }

        /// <summary>
        /// Get A matrix from a bitmap image .
        /// </summary>
        /// <param name="image">The bitmap image to convert .</param>
        /// <param name="pixels">An array of pixels to store grayscale value.</param>
        /// <returns>Returns the two dimentional pixel array of grayscale values</returns>
        public double[,] ConvertToMatrix(Bitmap image)
        {
            double[,] pixels = new double[image.Width, image.Height];
            for (int i = 0; i < image.Width; i++)
                for (int j = 0; j < image.Height; j++)
                {
                    Color cl = image.GetPixel(i, j);
                    int rl = cl.R;
                    int gl = cl.G;
                    int b1 = cl.B;
                    // get the grayshade of a pixel 
                    float gray = (float)(.299 * rl + .587 * gl + .114 * b1);
                    //if the pixel is white get 0 else get a value between 0 excluded and 1 
                    pixels[j, i] = (gray == 255) ? 0 : (gray / 256);

                }
            return pixels;
        }

        /// <summary>
        /// convert an Image to bitmap image 
        /// </summary>
        /// <param name="img"> the Image to be converted </param>
        /// <returns> a bitmapImage type </returns>

        public BitmapImage ConvertToBitmapImage(Image img)
        {
            using (var memory = new MemoryStream())
            {
                img.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        /// <summary>
        /// convert A inkcanvas control element to BitmapSource 
        /// </summary>
        /// <param name="inkCanvasElement"> the inkcanvas element to be rendered as bitmap source</param>
        /// <returns> BitmapSource type </returns>
        public BitmapSource ConvertToBitmapSource(UIElement inkCanvasElement)
        {
            var target = new RenderTargetBitmap((int)inkCanvasElement.RenderSize.Width, (int)inkCanvasElement.RenderSize.Width, 96, 96, PixelFormats.Pbgra32);
            var brush = new VisualBrush(inkCanvasElement);

            var visual = new DrawingVisual();
            var drawingContext = visual.RenderOpen();


            drawingContext.DrawRectangle(brush, null, new Rect(new Point(0, 0),
                new Point((int)inkCanvasElement.RenderSize.Width, (int)inkCanvasElement.RenderSize.Width)));

            drawingContext.Close();

            target.Render(visual);

            return target;
        }
        /// <summary>
        /// Convert A bitmapSource to Bitmap type
        /// </summary>
        /// <param name="bitmapSource">the bitmap source to be copied as bitmap</param>
        /// <returns></returns>
        public Bitmap ConvertToBitmap(BitmapSource bitmapSource)
        {
            Bitmap bmp = new Bitmap(
              bitmapSource.PixelWidth,
              bitmapSource.PixelHeight,
              PixelFormat.Format32bppPArgb);
            BitmapData data = bmp.LockBits(
              new Rectangle(new System.Drawing.Point(0, 0), bmp.Size),
              ImageLockMode.WriteOnly,
              PixelFormat.Format32bppPArgb);
            bitmapSource.CopyPixels(
              Int32Rect.Empty,
              data.Scan0,
              data.Height * data.Stride,
              data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }

       /// <summary>
       /// convert a bitmap to ImageSource with specific widthPixel and specific heighpixel
       /// </summary>
       /// <param name="bmp">The bitmap to be converted</param>
       /// <param name="width"> the new widthPixels to get </param>
       /// <param name="heigh"> the new heighPixels to get </param>
       /// <returns></returns>
        public ImageSource ConvertToImageSource(Bitmap bmp, int width, int heigh)
        {
            var handle = bmp.GetHbitmap();
            
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(width, heigh));
            
          
        }

        /// <summary>
        /// Convert ImageSource to bitmap
        /// </summary>
        /// <param name="img">the image source to be converted to bitmap </param>
        /// <returns></returns>
        public Bitmap ImageSourceToBitmap( ImageSource img)
        {
            var d = new DataObject(DataFormats.Bitmap, img, true);
            var bmp = d.GetData("System.Drawing.Bitmap") as System.Drawing.Bitmap;
            bmp.Save("res.bmp");
            return bmp;
        }


        public  Vector<double> getVector()
        {
            int index = 0;
            double[] arrayOfPixels = new double[imgPixelsMatrix.Length];
            for (int i = 0; i < imgPixelsMatrix.GetLength(1); i++)
            {
                for (int j = 0; j < imgPixelsMatrix.GetLength(0); j++)
                {
                    //
                    arrayOfPixels[index] = imgPixelsMatrix[i, j];
                    index++;
                }
            }

            var V = Vector<double>.Build;
           
            var v = V.DenseOfArray(arrayOfPixels);

            return v; 
            
        }


 
    }
}
