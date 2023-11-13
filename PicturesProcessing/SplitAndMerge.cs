using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

public class SplitAndMerge
{
    private Bitmap image;
    private Bitmap image2;
    private int segmentSize;
    private int tolerance;
    private List<Rectangle> segments = new List<Rectangle>();

    public SplitAndMerge(Bitmap image, Bitmap image2, int segmentSize, int tolerance)
    {
        this.image = image;
        this.image2 = image2;
        this.segmentSize = segmentSize;
        this.tolerance = tolerance;
    }

    public bool IsHomogeneous(int x, int y, int size)
    {
        int sumBrightness = 0;
        int pixelCount = 0;

        for (int i = x; i < x + size; i++)
        {
            for (int j = y; j < y + size; j++)
            {
                Color pixelColor = image.GetPixel(i, j);
                int brightness = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                sumBrightness += brightness;
                pixelCount++;
            }
        }

        int avgBrightness = sumBrightness / pixelCount;

        for (int i = x; i < x + size; i++)
        {
            for (int j = y; j < y + size; j++)
            {
                Color pixelColor = image.GetPixel(i, j);
                int brightness = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                if (Math.Abs(avgBrightness - brightness) > tolerance)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void Process()
    {
        ProcessRegion(0, 0, image.Width, image.Height);
    }

    static int Clamp(int value, int min, int max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    private bool IsHomogeneous(int x, int y, int width, int height)
    {
        double avgBrightness = 0;
        double variance = 0;
        int pixelCount = width * height;

        for (int i = x; i < x + width; i++)
        {
            for (int j = y; j < y + height; j++)
            {
                Color pixel = image.GetPixel(i, j);
                avgBrightness += pixel.GetBrightness();
            }
        }

        avgBrightness /= pixelCount;

        // Вычисляем дисперсию
        for (int i = x; i < x + width; i++)
        {
            for (int j = y; j < y + height; j++)
            {
                Color pixel = image.GetPixel(i, j);
                double brightness = pixel.GetBrightness();
                variance += (brightness - avgBrightness) * (brightness - avgBrightness);
            }
        }

        variance /= pixelCount;

        return variance < 0.0005;
    }

    private void ProcessRegion(int x, int y, int width, int height)
    {
        if (width >= segmentSize && height >= segmentSize)
        {
            if (IsHomogeneous(x, y, width, height))
            {
                // Если блок однородный бы его не делим, TODO переделать а то не красиво
            }
            else
            {
                //Graphics g = Graphics.FromImage(image2);
                //Pen pen = new Pen(Color.Red);
                //g.DrawLine(pen, x, y, Clamp(x + width, 0, image2.Width - 1), y);
                //g.DrawLine(pen, x, y, x, Clamp(y + height, 0, image2.Height - 1));
                //g.DrawLine(pen, Clamp(x + width, 0, image2.Width - 1), y, Clamp(x + width, 0, image2.Width - 1), Clamp(y + height, 0, image2.Height - 1));
                //g.DrawLine(pen, x, Clamp(y + height, 0, image2.Height - 1), Clamp(x + width, 0, image2.Width - 1), Clamp(y + height, 0, image2.Height - 1));

                ProcessRegion(x, y, width / 2, height / 2);
                ProcessRegion(x + width / 2, y, width / 2, height / 2);
                ProcessRegion(x, y + height / 2, width / 2, height / 2);
                ProcessRegion(x + width / 2, y + height / 2, width / 2, height / 2);
            }
        }
    }

    public void SmoothEdges(int radius)
    {
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                List<Color> neighboringPixels = new List<Color>();
                for (int dy = -radius; dy <= radius; dy++)
                {
                    for (int dx = -radius; dx <= radius; dx++)
                    {
                        int newX = x + dx;
                        int newY = y + dy;
                        if (newX >= 0 && newX < image.Width && newY >= 0 && newY < image.Height)
                        {
                            neighboringPixels.Add(image.GetPixel(newX, newY));
                        }
                    }
                }
                neighboringPixels.Sort((c1, c2) => (c1.R + c1.G + c1.B).CompareTo(c2.R + c2.G + c2.B));
                image.SetPixel(x, y, neighboringPixels[neighboringPixels.Count / 2]);
            }
        }
    }

    public Bitmap CreateFinalImage(int size)
    {
        //Bitmap finalImage = new Bitmap(image.Width, image.Height);

        //for (int i = 0; i < image.Width; i += size)
        //{
        //    for (int j = 0; j < image.Height; j += size)
        //    {
        //        int rSum = 0, gSum = 0, bSum = 0;
        //        int count = 0;

        //        for (int x = i; x < i + size && x < image.Width; x++)
        //        {
        //            for (int y = j; y < j + size && y < image.Height; y++)
        //            {
        //                Color pixel = image.GetPixel(x, y);
        //                rSum += pixel.R;
        //                gSum += pixel.G;
        //                bSum += pixel.B;
        //                count++;
        //            }
        //        }

        //        Color avgColor = Color.FromArgb(rSum / count, gSum / count, bSum / count);

        //        for (int x = i; x < i + size && x < image.Width; x++)
        //        {
        //            for (int y = j; y < j + size && y < image.Height; y++)
        //            {
        //                finalImage.SetPixel(x, y, avgColor);
        //            }
        //        }
        //    }
        //}

        //return finalImage;
        return image;
    }

    public Bitmap CreateFinalImage2()
    {
        return image2;
    }

    public void FindSegments()
    {
        FindAndAppendRegion(0, 0, image.Width, image.Height);
    }

    private void FindAndAppendRegion(int x, int y, int width, int height)
    {
        if (width >= segmentSize && height >= segmentSize)
        {
            if (IsHomogeneous(x, y, width, height))
            {
                segments.Add(new Rectangle(x, y, width, height));
            }
            else
            {
                FindAndAppendRegion(x, y, width / 2, height / 2);
                FindAndAppendRegion(x + width / 2, y, width / 2, height / 2);
                FindAndAppendRegion(x, y + height / 2, width / 2, height / 2);
                FindAndAppendRegion(x + width / 2, y + height / 2, width / 2, height / 2);
            }
        }
    }

    private List<Rectangle> MergeSegments(List<Rectangle> inputSegments)
    {
        List<Rectangle> mergedSegments = new List<Rectangle>(inputSegments);
        bool isMerged;

        do
        {
            isMerged = false;

            for (int i = 0; i < mergedSegments.Count - 1; i++)
            {
                for (int j = i + 1; j < mergedSegments.Count; j++)
                {
                    Rectangle r1 = mergedSegments[i];
                    Rectangle r2 = mergedSegments[j];

                    // Проверка на соседство по горизонтали
                    if ((r1.Left == r2.Right || r1.Right == r2.Left) && r1.Top == r2.Top && r1.Height == r2.Height)
                    {
                        int newWidth = r1.Width + r2.Width;
                        int newX = Math.Min(r1.Left, r2.Left);

                        if (IsHomogeneous(newX, r1.Top, newWidth, r1.Height))
                        {
                            mergedSegments[i] = new Rectangle(newX, r1.Top, newWidth, r1.Height);
                            mergedSegments.RemoveAt(j);
                            isMerged = true;
                            break;
                        }
                    }

                    // Проверка на соседство по вертикали
                    else if ((r1.Top == r2.Bottom || r1.Bottom == r2.Top) && r1.Left == r2.Left && r1.Width == r2.Width)
                    {
                        int newHeight = r1.Height + r2.Height;
                        int newY = Math.Min(r1.Top, r2.Top);

                        if (IsHomogeneous(r1.Left, newY, r1.Width, newHeight))
                        {
                            mergedSegments[i] = new Rectangle(r1.Left, newY, r1.Width, newHeight);
                            mergedSegments.RemoveAt(j);
                            isMerged = true;
                            break;
                        }
                    }
                }
            }
        }
        while (isMerged);

        return mergedSegments;
    }

    private Color GetAverageColor(int x, int y, int width, int height)
    {
        int avgR = 0;
        int avgG = 0;
        int avgB = 0;

        int pixelCount = width * height;

        for (int i = x; i < x + width; i++)
        {
            for (int j = y; j < y + height; j++)
            {
                Color pixel = image.GetPixel(i, j);
                avgR += pixel.R;
                avgG += pixel.G;
                avgB += pixel.B;
            }
        }

        avgR /= pixelCount;
        avgG /= pixelCount;
        avgB /= pixelCount;

        return Color.FromArgb(avgR, avgG, avgB);
    }

    public static Color ColorFromAhsb(int a, float h, float s, float b)
    {
        if (0 > a || 255 < a)
        {
            throw new ArgumentOutOfRangeException("a", a, "InvalidAlpha");
        }
        if (0f > h || 360f < h)
        {
            throw new ArgumentOutOfRangeException("h", h, "InvalidHue");
        }
        if (0f > s || 1f < s)
        {
            throw new ArgumentOutOfRangeException("s", s, "InvalidSaturation");
        }
        if (0f > b || 1f < b)
        {
            throw new ArgumentOutOfRangeException("b", b, "InvalidBrightness");
        }

        if (0 == s)
        {
            return Color.FromArgb(a, Convert.ToInt32(b * 255), Convert.ToInt32(b * 255), Convert.ToInt32(b * 255));
        }

        float fMax, fMid, fMin;
        int iSextant, iMax, iMid, iMin;

        if (0.5 < b)
        {
            fMax = b - (b * s) + s;
            fMin = b + (b * s) - s;
        }
        else
        {
            fMax = b + (b * s);
            fMin = b - (b * s);
        }

        iSextant = (int)Math.Floor(h / 60f);
        if (300f <= h)
        {
            h -= 360f;
        }
        h /= 60f;
        h -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
        if (0 == iSextant % 2)
        {
            fMid = h * (fMax - fMin) + fMin;
        }
        else
        {
            fMid = fMin - h * (fMax - fMin);
        }

        iMax = Convert.ToInt32(fMax * 255);
        iMid = Convert.ToInt32(fMid * 255);
        iMin = Convert.ToInt32(fMin * 255);

        switch (iSextant)
        {
            case 1:
                return Color.FromArgb(a, iMid, iMax, iMin);
            case 2:
                return Color.FromArgb(a, iMin, iMax, iMid);
            case 3:
                return Color.FromArgb(a, iMin, iMid, iMax);
            case 4:
                return Color.FromArgb(a, iMid, iMin, iMax);
            case 5:
                return Color.FromArgb(a, iMax, iMin, iMid);
            default:
                return Color.FromArgb(a, iMax, iMid, iMin);
        }
    }

    private double CalculateBrightness(Color color)
    {
        return (color.R + color.G + color.B) / (3.0 * 255);
    }

    private Color ConvertToBrightColor(Color color)
    {
        int h = (int)(color.GetHue() * 240 / 360);
        int s = (int)(Math.Max(color.GetSaturation(), 0.5f) * 255);
        double brightness = CalculateBrightness(color);
        int b = (int)(Math.Max(brightness, 0.5f) * 255);

        return Color.FromArgb(255, ColorFromAhsb(255, h, s, b));
    }

    private void FillBlockWithAverageColor(int x, int y, int width, int height)
    {
        Color avgColor = GetAverageColor(x, y, width, height);

        using (Graphics g = Graphics.FromImage(image))
        {
            using (SolidBrush brush = new SolidBrush(avgColor))
            {
                g.FillRectangle(brush, x, y, width, height);
            }
        }
    }

    private void FillBlockWithBrightColor(int x, int y, int width, int height)
    {
        Color avgColor = GetAverageColor(x, y, width, height);
        Color brightColor = ConvertToBrightColor(avgColor);

        using (Graphics g = Graphics.FromImage(image))
        {
            using (SolidBrush brush = new SolidBrush(brightColor))
            {
                g.FillRectangle(brush, x, y, width, height);
            }
        }
    }

    public Bitmap ProcessImage(Bitmap img1)
    {
        image = img1;

        FindSegments();

        List<Rectangle> mergedSegments = MergeSegments(segments);

        foreach (Rectangle block in mergedSegments)
        {
            DrawBlockOutline(image2, block.X, block.Y, block.Width, block.Height);
        }

        foreach (Rectangle block in mergedSegments)
        {
            FillBlockWithAverageColor(block.X, block.Y, block.Width, block.Height);
            //FillBlockWithBrightColor(block.X, block.Y, block.Width, block.Height);
        }

        return image;
    }

    private void DrawBlockOutline(Bitmap outputImage, int x, int y, int width, int height)
    {
        using (Graphics g = Graphics.FromImage(outputImage))
        {
            using (Pen pen = new Pen(Color.Red))
            {
                g.DrawLine(pen, x, y, Clamp(x + width, 0, outputImage.Width - 1), y);
                g.DrawLine(pen, x, y, x, Clamp(y + height, 0, outputImage.Height - 1));
                g.DrawLine(pen, Clamp(x + width, 0, outputImage.Width - 1), y, Clamp(x + width, 0, outputImage.Width - 1), Clamp(y + height, 0, outputImage.Height - 1));
                g.DrawLine(pen, x, Clamp(y + height, 0, outputImage.Height - 1), Clamp(x + width, 0, outputImage.Width - 1), Clamp(y + height, 0, outputImage.Height - 1));
            }
        }
    }

    bool AreNeighbors(Rectangle a, Rectangle b, int tolerance)
    {
        Rectangle biggerA = new Rectangle(a.X - tolerance, a.Y - tolerance, a.Width + 2 * tolerance, a.Height + 2 * tolerance);
        return biggerA.IntersectsWith(b);
    }

    //bool AreNeighbors(Rectangle a, Rectangle b, int tolerance)
    //{
    //    Rectangle biggerA = new Rectangle(a.X - tolerance, a.Y - tolerance, a.Width + 2 * tolerance, a.Height + 2 * tolerance);
    //    bool intersect = biggerA.IntersectsWith(b);

    //    double areaA = a.Width * a.Height;
    //    double areaB = b.Width * b.Height;
    //    double areaDifference = Math.Abs(areaA - areaB) / Math.Min(areaA, areaB);

    //    return intersect && areaDifference <= 0.5f;
    //}

    public List<List<Rectangle>> FindObjects()
    {
        //segments.Sort((a, b) =>
        //{
        //    int deltaY = a.Y - b.Y;
        //    return deltaY != 0 ? deltaY : a.X - b.X;
        //});

        List<List<Rectangle>> objects = new List<List<Rectangle>>();

        foreach (Rectangle block in segments)
        {
            List<Rectangle> currentNeighbors = new List<Rectangle>();
            List<int> intersectedObjectsIndexes = new List<int>();

            for (int i = 0; i < objects.Count; i++)
            {
                List<Rectangle> objectBlocks = objects[i];
                bool isNeighbor = objectBlocks.Any(existingBlock => AreNeighbors(existingBlock, block, tolerance));

                if (isNeighbor)
                {
                    currentNeighbors.Add(block);
                    intersectedObjectsIndexes.Add(i);
                }
            }

            if (currentNeighbors.Count == 0)
            {
                List<Rectangle> newObject = new List<Rectangle> { block };
                objects.Add(newObject);
            }
            else
            {
                if (intersectedObjectsIndexes.Count == 1)
                {
                    objects[intersectedObjectsIndexes[0]].Add(block);
                }
                else
                {
                    List<Rectangle> mergedObject = new List<Rectangle>();
                    foreach (int index in intersectedObjectsIndexes.OrderByDescending(x => x))
                    {
                        mergedObject.AddRange(objects[index]);
                        objects.RemoveAt(index);
                    }
                    mergedObject.Add(block);
                    objects.Add(mergedObject);
                }
            }
        }

        //Graphics g = Graphics.FromImage(image2);
        //Pen pen = new Pen(Color.Red);
        //g.DrawLine(pen, x, y, Clamp(x + width, 0, image2.Width - 1), y);
        //g.DrawLine(pen, x, y, x, Clamp(y + height, 0, image2.Height - 1));
        //g.DrawLine(pen, Clamp(x + width, 0, image2.Width - 1), y, Clamp(x + width, 0, image2.Width - 1), Clamp(y + height, 0, image2.Height - 1));
        //g.DrawLine(pen, x, Clamp(y + height, 0, image2.Height - 1), Clamp(x + width, 0, image2.Width - 1), Clamp(y + height, 0, image2.Height - 1));
        //List<Rectangle> firstObject1 = objects[1];
        //List<Rectangle> firstObject2 = objects[2];
        //List<Rectangle> firstObject3 = objects[3];
        //List<Rectangle> firstObject4 = objects[4];
        //List<Rectangle> firstObject5 = objects[5];
        //drawRect(firstObject1, Color.Red);
        //drawRect(firstObject2, Color.Purple);
        //drawRect(firstObject3, Color.Pink);
        //drawRect(firstObject4, Color.PapayaWhip);
        //drawRect(firstObject5, Color.PaleGreen);
        //foreach (Rectangle rectangle in firstObject)
        //{

        //    //Graphics g = Graphics.FromImage(image2);
        //    //Pen pen = new Pen(Color.Red);
        //    //g.DrawLine(pen, rectangle.X, rectangle.Y, Clamp(rectangle.X + rectangle.Width, 0, image2.Width - 1), rectangle.Y);
        //    //g.DrawLine(pen, rectangle.X, rectangle.Y, rectangle.X, Clamp(rectangle.Y + rectangle.Height, 0, image2.Height - 1));
        //    //g.DrawLine(pen, Clamp(rectangle.X + rectangle.Width, 0, image2.Width - 1), rectangle.Y, Clamp(rectangle.X + rectangle.Width, 0, image2.Width - 1), Clamp(rectangle.Y + rectangle.Height, 0, image2.Height - 1));
        //    //g.DrawLine(pen, rectangle.X, Clamp(rectangle.Y + rectangle.Height, 0, image2.Height - 1), Clamp(rectangle.X + rectangle.Width, 0, image2.Width - 1), Clamp(rectangle.Y + rectangle.Height, 0, image2.Height - 1));

        //}

        return objects;
    }

    private void drawRect(List<Rectangle> firstObject, Color col)
    {
        foreach (Rectangle rectangle in firstObject)
        {
            Graphics g = Graphics.FromImage(image2);
            Pen pen = new Pen(col);
            g.DrawLine(pen, rectangle.X, rectangle.Y, Clamp(rectangle.X + rectangle.Width, 0, image2.Width - 1), rectangle.Y);
            g.DrawLine(pen, rectangle.X, rectangle.Y, rectangle.X, Clamp(rectangle.Y + rectangle.Height, 0, image2.Height - 1));
            g.DrawLine(pen, Clamp(rectangle.X + rectangle.Width, 0, image2.Width - 1), rectangle.Y, Clamp(rectangle.X + rectangle.Width, 0, image2.Width - 1), Clamp(rectangle.Y + rectangle.Height, 0, image2.Height - 1));
            g.DrawLine(pen, rectangle.X, Clamp(rectangle.Y + rectangle.Height, 0, image2.Height - 1), Clamp(rectangle.X + rectangle.Width, 0, image2.Width - 1), Clamp(rectangle.Y + rectangle.Height, 0, image2.Height - 1));
        }
    }
}