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

        public static Bitmap ApplyGaussianBlur(Bitmap inputImage, int kernelSize = 3, double sigma = 1.0)
        {
            if (kernelSize % 2 == 0)
            {
                throw new ArgumentException("Kernel size must be an odd number.");
            }

            Bitmap blurredImage = new Bitmap(inputImage.Width, inputImage.Height);
            int kernelOffset = (kernelSize - 1) / 2;

            double[,] kernel = GenerateGaussianKernel(kernelSize, sigma);

            using (Graphics g = Graphics.FromImage(blurredImage))
            {
                for (int x = 0; x < inputImage.Width; x++)
                {
                    for (int y = 0; y < inputImage.Height; y++)
                    {
                        double rSum = 0;
                        double gSum = 0;
                        double bSum = 0;
                        double weightSum = 0;

                        for (int kx = -kernelOffset; kx <= kernelOffset; kx++)
                        {
                            for (int ky = -kernelOffset; ky <= kernelOffset; ky++)
                            {
                                int pixelX = x + kx;
                                int pixelY = y + ky;

                                if (pixelX < 0 || pixelY < 0 || pixelX >= inputImage.Width || pixelY >= inputImage.Height)
                                {
                                    continue;
                                }

                                Color pixelColor = inputImage.GetPixel(pixelX, pixelY);

                                rSum += kernel[kx + kernelOffset, ky + kernelOffset] * pixelColor.R;
                                gSum += kernel[kx + kernelOffset, ky + kernelOffset] * pixelColor.G;
                                bSum += kernel[kx + kernelOffset, ky + kernelOffset] * pixelColor.B;
                                weightSum += kernel[kx + kernelOffset, ky + kernelOffset];
                            }
                        }

                        int r = Convert.ToInt32(rSum / weightSum);
                        int _g = Convert.ToInt32(gSum / weightSum);
                        int b = Convert.ToInt32(bSum / weightSum);

                        r = Math.Max(0, Math.Min(255, r));
                        _g = Math.Max(0, Math.Min(255, _g));
                        b = Math.Max(0, Math.Min(255, b));

                        blurredImage.SetPixel(x, y, Color.FromArgb(r, _g, b));
                    }
                }
            }

            return blurredImage;
        }

        private static double[,] GenerateGaussianKernel(int size, double sigma)
        {
            int offset = (size - 1) / 2;
            double[,] kernel = new double[size, size];
            double sigma2 = 2 * sigma * sigma;
            double sum = 0;

            for (int x = -offset; x <= offset; x++)
            {
                for (int y = -offset; y <= offset; y++)
                {
                    double gaussianValue = (1.0 / Math.Sqrt(Math.PI * sigma2)) * Math.Exp(-(x * x + y * y) / sigma2);
                    kernel[x + offset, y + offset] = gaussianValue;
                    sum += gaussianValue;
                }
            }

            // Normalize the kernel
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    kernel[x, y] /= sum;
                }
            }

            return kernel;
        }

        public static Bitmap ComputeGradient(Bitmap inputImage, out double[,] gradientDirections, out double[,] gradientStrengths)
        {
            int width = inputImage.Width;
            int height = inputImage.Height;
            gradientStrengths = new double[width, height];
            gradientDirections = new double[width, height];

            int[,] gx = new int[,]
            {
        { -1, 0, 1 },
        { -2, 0, 2 },
        { -1, 0, 1 }
            };

            int[,] gy = new int[,]
            {
        { -1, -2, -1 },
        { 0, 0, 0 },
        { 1, 2, 1 }
            };

            Bitmap gradientImage = new Bitmap(width, height);

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    double gradientX = 0.0;
                    double gradientY = 0.0;

                    for (int kx = -1; kx <= 1; kx++)
                    {
                        for (int ky = -1; ky <= 1; ky++)
                        {
                            Color pixelColor = inputImage.GetPixel(x + kx, y + ky);
                            int gray = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;

                            gradientX += gx[kx + 1, ky + 1] * gray;
                            gradientY += gy[kx + 1, ky + 1] * gray;
                        }
                    }

                    double gradientMagnitude = Math.Sqrt(gradientX * gradientX + gradientY * gradientY);
                    double gradientDirection = Math.Atan2(gradientY, gradientX);

                    int gradientColor = (int)Math.Min(255, gradientMagnitude);
                    gradientImage.SetPixel(x, y, Color.FromArgb(gradientColor, gradientColor, gradientColor));

                    gradientStrengths[x, y] = gradientMagnitude;
                    gradientDirections[x, y] = gradientDirection;
                }
            }

            return gradientImage;
        }

        public static Bitmap NonMaximumSuppression(Bitmap inputImage, double[,] gradientDirections, double[,] gradientStrengths)
        {
            int width = inputImage.Width;
            int height = inputImage.Height;

            Bitmap suppressedImage = new Bitmap(width, height);

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    double direction = gradientDirections[x, y];
                    double strength = gradientStrengths[x, y];

                    int[] dx = { 1, 1, 0, -1, -1, -1, 0, 1 };
                    int[] dy = { 0, 1, 1, 1, 0, -1, -1, -1 };

                    int index = (int)(direction / Math.PI * 8 + 8) % 8;

                    int x1 = x + dx[index];
                    int y1 = y + dy[index];
                    int x2 = x - dx[index];
                    int y2 = y - dy[index];

                    if ((x1 >= 0 && y1 >= 0 && x1 < width && y1 < height && strength <= gradientStrengths[x1, y1]) ||
                        (x2 >= 0 && y2 >= 0 && x2 < width && y2 < height && strength <= gradientStrengths[x2, y2]))
                    {
                        suppressedImage.SetPixel(x, y, Color.Black);
                    }
                    else
                    {
                        int value = (int)Math.Min(255, strength);
                        suppressedImage.SetPixel(x, y, Color.FromArgb(value, value, value));
                    }
                }
            }

            return suppressedImage;
        }

        public static Bitmap ApplyDoubleThreshold(Bitmap inputImage, double[,] gradientStrengths, double lowThreshold, double highThreshold)
        {
            int width = inputImage.Width;
            int height = inputImage.Height;

            Bitmap thresholdedImage = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double strength = gradientStrengths[x, y];

                    if (strength >= highThreshold)
                    {
                        thresholdedImage.SetPixel(x, y, Color.White); // сильные границы
                    }
                    else if (strength >= lowThreshold)
                    {
                        thresholdedImage.SetPixel(x, y, Color.FromArgb((int)strength, (int)strength, (int)strength)); // слабые границы
                    }
                    else
                    {
                        thresholdedImage.SetPixel(x, y, Color.Black); // остальные пиксели
                    }
                }
            }

            return thresholdedImage;
        }

        public static Bitmap ApplyHysteresis(Bitmap inputImage)
        {
            int width = inputImage.Width;
            int height = inputImage.Height;

            Bitmap finalImage = new Bitmap(width, height);
            int[] dx = { -1, 0, 1, 1, 1, 0, -1, -1 };
            int[] dy = { -1, -1, -1, 0, 1, 1, 1, 0 };

            Stack<Point> strongEdges = new Stack<Point>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color pixel = inputImage.GetPixel(x, y);

                    if (pixel.R == 255) // сильные границы
                    {
                        strongEdges.Push(new Point(x, y));
                    }
                }
            }

            while (strongEdges.Count > 0)
            {
                Point currentEdge = strongEdges.Pop();

                for (int i = 0; i < 8; i++)
                {
                    int newX = currentEdge.X + dx[i];
                    int newY = currentEdge.Y + dy[i];

                    if (newX >= 0 && newX < width && newY >= 0 && newY < height)
                    {
                        Color neighborPixel = inputImage.GetPixel(newX, newY);

                        if (neighborPixel.R != 255 && neighborPixel.R != 0) // слабые границы
                        {
                            strongEdges.Push(new Point(newX, newY));
                            inputImage.SetPixel(newX, newY, Color.White);
                        }
                    }
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color pixel = inputImage.GetPixel(x, y);

                    if (pixel.R == 255)
                    {
                        finalImage.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        finalImage.SetPixel(x, y, Color.Black);
                    }
                }
            }

            return finalImage;
        }

        public static Bitmap ApplyCannyFilter(Bitmap inputImage, int gaussianKernelSize = 5, double gaussianSigma = 1.0,
                                      double lowThreshold = 100.0, double highThreshold = 100.0)
        {
            // 1. Преобразование изображения в оттенки серого (если требуется)
            Bitmap grayImage = ConvertToGrayscale(inputImage);

            // 2. Фильтрация шума (размытие Гаусса)
            Bitmap blurredImage = ApplyGaussianBlur(grayImage, gaussianKernelSize, gaussianSigma);

            // 3. Вычисление градиента яркости (операторы Собеля)
            Bitmap gradientImage;
            double[,] gradientDirections;
            double[,] gradientStrengths;
            gradientImage = ComputeGradient(blurredImage, out gradientDirections, out gradientStrengths);

            // 4. Подавление немаксимумов
            Bitmap suppressedImage = NonMaximumSuppression(gradientImage, gradientDirections, gradientStrengths);

            // 5. Применение двойного порога
            Bitmap thresholdedImage = ApplyDoubleThreshold(suppressedImage, gradientStrengths, lowThreshold, highThreshold);

            // 6. Гистерезис
            Bitmap cannyImage = ApplyHysteresis(thresholdedImage);

            return cannyImage;
        }

        private static Bitmap ConvertToGrayscale(Bitmap inputImage)
        {
            Bitmap grayImage = new Bitmap(inputImage.Width, inputImage.Height);

            using (Graphics g = Graphics.FromImage(grayImage))
            {
                ColorMatrix colorMatrix = new ColorMatrix(
                    new float[][]
                    {
                new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
                new float[] { 0.587f, 0.587f, 0.587f, 0, 0 },
                new float[] { 0.114f, 0.114f, 0.114f, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { 0, 0, 0, 0, 1 },
                    });
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);
                g.DrawImage(inputImage, new Rectangle(0, 0, inputImage.Width, inputImage.Height),
                            0, 0, inputImage.Width, inputImage.Height, GraphicsUnit.Pixel, attributes);
            }

            return grayImage;
        }

        private void кенниToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap i1 = ApplyCannyFilter(img1);
            img1 = i1;
            pictureBox1.Image = i1;
            pictureBox1.Refresh();
        }

        public static int ZeroMoment(Bitmap binaryImage)
        {
            int m00 = 0;
            for (int x = 0; x < binaryImage.Width; x++)
            {
                for (int y = 0; y < binaryImage.Height; y++)
                {
                    Color pixel = binaryImage.GetPixel(x, y);
                    if (pixel.R == 255 && pixel.G == 255 && pixel.B == 255) // Если пиксель белый (единица)
                    {
                        m00++;
                    }
                }
            }
            return m00;
        }

        public static (int m10, int m01) FirstMoment(Bitmap binaryImage)
        {
            int m10 = 0, m01 = 0;
            for (int x = 0; x < binaryImage.Width; x++)
            {
                for (int y = 0; y < binaryImage.Height; y++)
                {
                    Color pixel = binaryImage.GetPixel(x, y);
                    if (pixel.R == 255 && pixel.G == 255 && pixel.B == 255) // Если пиксель белый (единица)
                    {
                        m10 += x;
                        m01 += y;
                    }
                }
            }
            return (m10, m01);
        }

        public static (double X_c, double Y_c) CenterOfMass(Bitmap binaryImage, double m00, double m10, double m01)
        {
            double X_c = (double)m10 / m00;
            double Y_c = (double)m01 / m00;
            return (X_c, Y_c);
        }

        public static (double mu20, double mu02, double mu11) CentralMoments(Bitmap binaryImage, double m00, double X_c, double Y_c)
        {
            double mu20 = 0, mu02 = 0, mu11 = 0;

            for (int x = 0; x < binaryImage.Width; x++)
            {
                for (int y = 0; y < binaryImage.Height; y++)
                {
                    Color pixel = binaryImage.GetPixel(x, y);
                    if (pixel.R == 255 && pixel.G == 255 && pixel.B == 255) // Если пиксель белый (единица)
                    {
                        mu20 += Math.Pow((x - X_c), 2);
                        mu02 += Math.Pow((y - Y_c), 2);
                        mu11 += (x - X_c) * (y - Y_c);
                    }
                }
            }

            return (mu20, mu02, mu11);
        }

        public static (double nu20, double nu02, double nu11) NormalizedCentralMoments(Bitmap binaryImage, double m00, double mu20, double mu02, double mu11)
        {
            double scalingFactor = Math.Pow(m00, 2.0 / 3); // Здесь степень равна 2/3, так как ранг 2

            double nu20 = mu20 / scalingFactor;
            double nu02 = mu02 / scalingFactor;
            double nu11 = mu11 / scalingFactor;

            return (nu20, nu02, nu11);
        }

        private void моментыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double m00 = ZeroMoment(img1);
            (double m10, double m01) = FirstMoment(img1);
            (double X_c, double Y_c) = CenterOfMass(img1, m00, m10, m01);
            (double mu20, double mu02, double mu11) = CentralMoments(img1, m00, X_c, Y_c);
            (double nu20, double nu02, double nu11) = NormalizedCentralMoments(img1, m00, mu20, mu02, mu11);
            MessageBox.Show(
                "Zero moment: " +
                m00.ToString() +
                " | " +
                ((int)(img1.Width * img1.Height)).ToString() +
                "\n" +
                "First moment: [" +
                m10.ToString() +
                " | " +
                m01.ToString() +
                " ]" +
                "\n" +
                "Center of mass: x(" +
                X_c.ToString() +
                "), y(" +
                Y_c.ToString() +
                ")\n" +
                "Central moments: [" +
                mu20.ToString() +
                " | " +
                mu02.ToString() +
                " | " +
                mu11.ToString() +
                "]\n" +
                "Normolaized central moments: [" +
                nu20.ToString() +
                " | " +
                nu02.ToString() +
                " | " +
                nu11.ToString(), 
                "Moments!",
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information
            );
        }

        private void ванToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //int initialSegmentSize = 1;
            //int tolerance = 10;
            //SplitAndMerge processor = new SplitAndMerge(img1, img2, initialSegmentSize, tolerance);
            //processor.Process();
            ////int radius = 2;
            ////processor.SmoothEdges(radius);
            //Bitmap final_img = processor.CreateFinalImage(initialSegmentSize);
            //Bitmap final_img2 = processor.CreateFinalImage2();
            //pictureBox1.Image = final_img;
            //pictureBox2.Image = final_img2;
            //pictureBox1.Refresh();
            //pictureBox2.Refresh();
            int initialSegmentSize = 1;
            int tolerance = 10;
            SplitAndMerge processor = new SplitAndMerge(img1, img2, initialSegmentSize, tolerance);
            pictureBox1.Image = processor.ProcessImage(img1);
            pictureBox1.Refresh();
            //processor.Process();
            Bitmap final_img2 = processor.CreateFinalImage2();
            pictureBox2.Image = final_img2;
            pictureBox2.Refresh();

            List<List<Rectangle>> list = processor.FindObjects();
            MessageBox.Show(
                (list.Count).ToString(),
                " | Count!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void туToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int initialSegmentSize = 4;
            int tolerance = 3;
            SplitAndMerge processor = new SplitAndMerge(img1, img2, initialSegmentSize, tolerance);
            pictureBox1.Image = processor.ProcessImage(img1);
            pictureBox1.Refresh();
            processor.Process();

            List<List<Rectangle>> list = processor.FindObjects();
            MessageBox.Show(
                (list.Count).ToString(),
                " | Count!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            Bitmap final_img2 = processor.CreateFinalImage2();
            pictureBox2.Image = final_img2;
            pictureBox2.Refresh();
        }
    }


}
