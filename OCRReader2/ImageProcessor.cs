using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using Tesseract;

static class ImageProcessor
{
    public static string[] ResizeAndPerformOCR(string imagePath)
    {
        List<string> ocrResults = new List<string>();

        using (Image originalImage = Image.FromFile(imagePath))
        {
            int[] widths = { 300, 600, 900 };

            foreach (int width in widths)
            {
                int newHeight = (int)(originalImage.Height * ((float)width / originalImage.Width));

                using (Image resizedImage = ResizeImage(originalImage, width, newHeight))
                {
                    string ocrText = PerformOCR(resizedImage);
                    ocrResults.Add(ocrText);
                }
            }
        }

        return ocrResults.ToArray();
    }

    public static Image ResizeImage(Image image, int width, int height)
    {
        Bitmap resizedBitmap = new Bitmap(width, height);

        using (Graphics graphics = Graphics.FromImage(resizedBitmap))
        {
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(image, 0, 0, width, height);
        }

        return resizedBitmap;
    }

    public static string PerformOCR(Image image)
    {
        using (var bitmap = new Bitmap(image))
        {
            string tessDataPath = @"./tessdata";

            using (var engine = new TesseractEngine(tessDataPath, "eng", EngineMode.Default))
            {
                using (var pixImage = PixConverter.ToPix(bitmap))
                {
                    using (var page = engine.Process(pixImage))
                    {
                        var extractedText = page.GetText();
                        return extractedText;
                    }
                }
            }
        }
    }
}
