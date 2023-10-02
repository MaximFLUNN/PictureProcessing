using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    }
}
