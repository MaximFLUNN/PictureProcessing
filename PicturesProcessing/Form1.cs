using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace PicturesProcessing
{
    public partial class Form1 : Form
    {
        Bitmap img1;
        Bitmap img2;
        public Form1()
        {
            InitializeComponent();
        }

        private void ванToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files | *.png; *.jpg; *.bmp | All files (*.*) | *.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                img1 = new Bitmap(dialog.FileName);
                pictureBox1.Image = img1;
                pictureBox1.Refresh();
            }
        }

        private void туToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files | *.png; *.jpg; *.bmp | All files (*.*) | *.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                img2 = new Bitmap(dialog.FileName);
                pictureBox2.Image = img2;
                pictureBox2.Refresh();
            }
        }

        static double CalculateMSE(Bitmap image1, Bitmap image2)
        {
            int width = image1.Width;
            int height = image1.Height;
            double mse = 0;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color pixelImage1 = image1.GetPixel(i, j);
                    Color pixelImage2 = image2.GetPixel(i, j);

                    double redError = pixelImage1.R - pixelImage2.R;
                    double greenError = pixelImage1.G - pixelImage2.G;
                    double blueError = pixelImage1.B - pixelImage2.B;

                    double pixelError = (redError * redError) + (greenError * greenError) + (blueError * blueError);
                    mse += pixelError;
                }
            }

            mse /= (width * height * 3);
            return mse;
        }

        static double CalculateUIQI(Bitmap image1, Bitmap image2)
        {
            int width = image1.Width;
            int height = image1.Height;
            int totalPixels = width * height;
            double C1 = 6.5025;
            double C2 = 58.5225;

            double muX = 0;
            double muY = 0;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    muX += image1.GetPixel(i, j).R;
                    muY += image2.GetPixel(i, j).R;
                }
            }

            muX /= totalPixels;
            muY /= totalPixels;

            double sigmaX = 0;
            double sigmaY = 0;
            double covarianceXY = 0;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int x = image1.GetPixel(i, j).R;
                    int y = image2.GetPixel(i, j).R;

                    double diffX = x - muX;
                    double diffY = y - muY;
                    sigmaX += diffX * diffX;
                    sigmaY += diffY * diffY;
                    covarianceXY += diffX * diffY;
                }
            }

            sigmaX = Math.Sqrt(sigmaX / totalPixels);
            sigmaY = Math.Sqrt(sigmaY / totalPixels);
            covarianceXY /= totalPixels;

            double qualityIndex =
                (covarianceXY / (sigmaX * sigmaY)) *
                ((2 * muX * muY + C1) / (muX * muX + muY * muY + C1)) *
                ((2 * sigmaX * sigmaY + C2) / (sigmaX * sigmaX + sigmaY * sigmaY + C2));

            return qualityIndex;
        }

        public static List<Bitmap> SplitBitmap(int w, int h, Bitmap inputImage)
        {
            int blockWidth = inputImage.Width / w;
            int blockHeight = inputImage.Height / h;

            List<Bitmap> outputImages = new List<Bitmap>();

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    Rectangle destRect = new Rectangle(0, 0, blockWidth, blockHeight);
                    Bitmap block = new Bitmap(blockWidth, blockHeight);
                    using (Graphics g = Graphics.FromImage(block))
                    {
                        Rectangle srcRect = new Rectangle(j * blockWidth, i * blockHeight, blockWidth, blockHeight);
                        g.DrawImage(inputImage, destRect, srcRect, GraphicsUnit.Pixel);
                    }
                    outputImages.Add(block);
                }
            }

            return outputImages;
        }

        private void mSEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img1.Width != img2.Width || img1.Height != img2.Height)
            {
                label1.Text = "MSE: err size!";
                return;
            }

            double mse = CalculateMSE(img1, img2);
            label1.Text = "MSE: " + mse.ToString();
        }

        private void uIQIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img1.Width != img2.Width || img1.Height != img2.Height)
            {
                label2.Text = "MSE average: err size!";
                return;
            }

            double uiqi = CalculateUIQI(img1, img2);
            label2.Text = "UIQI: " + uiqi.ToString();
        }

        private void mSEAverageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img1.Width != img2.Width || img1.Height != img2.Height)
            {
                label3.Text = "UIQI average: err size!";
                return;
            }

            int h = 3;
            int w = 3;
            List<Bitmap> listIMG1 = SplitBitmap(w, h, img1);
            List<Bitmap> listIMG2 = SplitBitmap(w, h, img2);
            double sum = 0;
            for (int i = 0; i < w * h; i++)
            {
                sum += CalculateMSE(listIMG1[i], listIMG2[i]);
            }

            label3.Text = "MSE average: " + (sum / (w * h)).ToString();
        }

        private void uIQIAverageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img1.Width != img2.Width || img1.Height != img2.Height)
            {
                label4.Text = "UIQI average: err size!";
                return;
            }

            int h = 3;
            int w = 3;
            List<Bitmap> listIMG1 = SplitBitmap(w, h, img1);
            List<Bitmap> listIMG2 = SplitBitmap(w, h, img2);
            double sum = 0;
            for (int i = 0; i < w * h; i++)
            {
                sum += CalculateUIQI(listIMG1[i], listIMG2[i]);
            }

            label4.Text = "UIQI average: " + (sum / (w * h)).ToString();
        }
        static Bitmap ApplyGaussianNoise(Bitmap image, double stdDev)
        {
            Bitmap noisyImage = new Bitmap(image.Width, image.Height);
            Random rand = new Random();

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixel = image.GetPixel(x, y);

                    int r = Clamp(pixel.R + (int)(NextGaussian(rand) * stdDev + 0.5), 0, 255);
                    int g = Clamp(pixel.G + (int)(NextGaussian(rand) * stdDev + 0.5), 0, 255);
                    int b = Clamp(pixel.B + (int)(NextGaussian(rand) * stdDev + 0.5), 0, 255);

                    noisyImage.SetPixel(x, y, Color.FromArgb(pixel.A, r, g, b));
                }
            }

            return noisyImage;
        }
        static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static double NextGaussian(Random rand, double mean = 0, double stdDev = 1)
        {
            double u1 = 1.0 - rand.NextDouble();
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return mean + stdDev * randStdNormal;
        }

        private void перваяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap img_1 = ApplyGaussianNoise(img1, 50);
            img1 = img_1;
            pictureBox1.Image = img1;
            pictureBox1.Refresh();
        }

        private void втораяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap img_2 = ApplyGaussianNoise(img2, 50);
            img1 = img_2;
            pictureBox2.Image = img2;
            pictureBox2.Refresh();
        }

        int[] CalculateNoiseHistogram(Bitmap img1, Bitmap img2 = null)
        {
            int[] noiseHistogram = new int[256];
            int width = img1.Width;
            int height = img1.Height;

            if (img2 != null) // два изображения
            {
                if (img1.Width != img2.Width || img1.Height != img2.Height)
                    throw new ArgumentException("Изображения должны иметь один размер");

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Color pixel1 = img1.GetPixel(x, y);
                        Color pixel2 = img2.GetPixel(x, y);
                        int diff = Math.Abs(pixel1.R - pixel2.R) +
                                   Math.Abs(pixel1.G - pixel2.G) +
                                   Math.Abs(pixel1.B - pixel2.B);
                        noiseHistogram[Math.Min(diff, 255)]++;
                    }
                }
            }

            else // одно изображение
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width - 1; x++)
                    {
                        Color pixel1 = img1.GetPixel(x, y);
                        Color pixel2 = img1.GetPixel(x + 1, y);
                        int diff = Math.Abs(pixel1.R - pixel2.R) +
                                   Math.Abs(pixel1.G - pixel2.G) +
                                   Math.Abs(pixel1.B - pixel2.B);
                        noiseHistogram[Math.Min(diff, 255)]++;
                    }
                }
            }
            return noiseHistogram;
        }

        Bitmap CreateHistogramImage(int[] histogram, int width, int height)
        {
            Bitmap histImage = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(histImage))
            {
                g.Clear(Color.White);
                Pen pen = new Pen(Color.Black);
                int maxHistValue = histogram.Max();

                for (int i = 0; i < histogram.Length; i++)
                {
                    float x1 = (float)width / histogram.Length * i;
                    float y1 = height;
                    float x2 = (float)width / histogram.Length * (i + 1);
                    float y2 = height - height * ((float)histogram[i] / maxHistValue);
                    g.DrawLine(pen, x1, y1, x2, y2);
                }
            }

            return histImage;
        }

        public static void BuildHistogram(Bitmap inputImage, string outputHistogramPath)
        {
            var image = inputImage;

            var histogram_ = new int[256];

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    int pixel = (int)(255*image.GetPixel(i, j).GetBrightness());
                    histogram_[pixel]++;
                }
            }

            var histogram = new Bitmap(256, 200);

            var penR = new Pen(Color.Red, 1);

            using (var g = Graphics.FromImage(histogram))
            {
                for (int i = 0; i < 256; i++)
                {
                    int rHeight = (int)((histogram_[i] / (double)histogram_.Max()) * 200);

                    g.DrawLine(penR, new Point(i, histogram.Height - rHeight), new Point(i, histogram.Height));
                }
            }

            histogram.Save(outputHistogramPath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        public static Bitmap MedianFilter(Bitmap sourceBitmap,
                                                int matrixSize,
                                                  int bias = 0,
                                         bool grayscale = false)
        {
            BitmapData sourceData =
                       sourceBitmap.LockBits(new Rectangle(0, 0,
                       sourceBitmap.Width, sourceBitmap.Height),
                       ImageLockMode.ReadOnly,
                       PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride *
                                          sourceData.Height];

            byte[] resultBuffer = new byte[sourceData.Stride *
                                           sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0,
                                       pixelBuffer.Length);

            sourceBitmap.UnlockBits(sourceData);

            if (grayscale == true)
            {
                float rgb = 0;

                for (int k = 0; k < pixelBuffer.Length; k += 4)
                {
                    rgb = pixelBuffer[k] * 0.11f;
                    rgb += pixelBuffer[k + 1] * 0.59f;
                    rgb += pixelBuffer[k + 2] * 0.3f;


                    pixelBuffer[k] = (byte)rgb;
                    pixelBuffer[k + 1] = pixelBuffer[k];
                    pixelBuffer[k + 2] = pixelBuffer[k];
                    pixelBuffer[k + 3] = 255;
                }
            }

            int filterOffset = (matrixSize - 1) / 2;
            int calcOffset = 0;

            int byteOffset = 0;

            List<int> neighbourPixels = new List<int>();
            byte[] middlePixel;

            for (int offsetY = filterOffset; offsetY <
                sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX <
                    sourceBitmap.Width - filterOffset; offsetX++)
                {
                    byteOffset = offsetY *
                                 sourceData.Stride +
                                 offsetX * 4;

                    neighbourPixels.Clear();

                    for (int filterY = -filterOffset;
                        filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset;
                            filterX <= filterOffset; filterX++)
                        {

                            calcOffset = byteOffset +
                                         (filterX * 4) +
                                         (filterY * sourceData.Stride);

                            neighbourPixels.Add(BitConverter.ToInt32(
                                             pixelBuffer, calcOffset));
                        }
                    }

                    neighbourPixels.Sort();

                    middlePixel = BitConverter.GetBytes(
                                       neighbourPixels[filterOffset]);

                    resultBuffer[byteOffset] = middlePixel[0];
                    resultBuffer[byteOffset + 1] = middlePixel[1];
                    resultBuffer[byteOffset + 2] = middlePixel[2];
                    resultBuffer[byteOffset + 3] = middlePixel[3];
                }
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width,
                                             sourceBitmap.Height);

            BitmapData resultData =
                       resultBitmap.LockBits(new Rectangle(0, 0,
                       resultBitmap.Width, resultBitmap.Height),
                       ImageLockMode.WriteOnly,
                       PixelFormat.Format32bppArgb);

            Marshal.Copy(resultBuffer, 0, resultData.Scan0,
                                       resultBuffer.Length);

            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }

        public static double ComputeGeometricMean(Bitmap image)
        {
            if (image == null)
            {
                throw new ArgumentException("Input image should not be null.");
            }
            int width = image.Width;
            int height = image.Height;

            double redSum = 0;
            double greenSum = 0;
            double blueSum = 0;
            double totalPixels = width * height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixel = image.GetPixel(x, y);
                    redSum += Math.Log(pixel.R + 1);
                    greenSum += Math.Log(pixel.G + 1);
                    blueSum += Math.Log(pixel.B + 1);
                }
            }

            double redMean = Math.Exp(redSum / totalPixels) - 1;
            double greenMean = Math.Exp(greenSum / totalPixels) - 1;
            double blueMean = Math.Exp(blueSum / totalPixels) - 1;

            return (redMean + greenMean + blueMean) / 3.0;
        }

        private void перваяToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BuildHistogram(img1, "test11.jpg");
            //Bitmap img_1 = CreateHistogramImage(CalculateNoiseHistogram(img1), img1.Width, img1.Height);
            //img_1.Save("gistoFirst.jpg", ImageFormat.Jpeg);
        }

        private void втораяToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BuildHistogram(img2, "test22.jpg");
            //Bitmap img_2 = CreateHistogramImage(CalculateNoiseHistogram(img2), img2.Width, img2.Height);
            //img_2.Save("gistoSecond.jpg", ImageFormat.Jpeg);
        }

        private void обеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BuildHistogram(img1, "test1.jpg");
            BuildHistogram(img2, "test2.jpg");
            //Bitmap img = CreateHistogramImage(CalculateNoiseHistogram(img1, img2), img1.Width, img1.Height);
            //img.Save("gistoDuo.jpg", ImageFormat.Jpeg);
        }

        private void перваяToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Bitmap img_1 = img1;
            pictureBox1.Image = MedianFilter(img_1, 4, 1);
            pictureBox1.Refresh();
        }

        private void втораяToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Bitmap img_2 = img2;
            pictureBox2.Image = MedianFilter(img_2, 4, 1);
            pictureBox2.Refresh();
        }

        private void перваяToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            double geometricMean = ComputeGeometricMean(img1);
            label5.Text = "GEOM Median: " + geometricMean.ToString();
        }

        private void втораяToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            double geometricMean = ComputeGeometricMean(img2);
            label5.Text = "GEOM Median: " + geometricMean.ToString();
        }
    }
}
