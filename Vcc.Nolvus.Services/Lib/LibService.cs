using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;

namespace Vcc.Nolvus.Services.Lib
{
    public class LibService : ILibService
    {
        public Image ResizeKeepAspectRatio(Image source, int width, int height)
        {
            Image result = null;

            try
            {
                if (source.Width != width || source.Height != height)
                {
                    // Resize image
                    float sourceRatio = (float)source.Width / source.Height;

                    using (var target = new Bitmap(width, height))
                    {
                        using (var g = System.Drawing.Graphics.FromImage(target))
                        {
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = SmoothingMode.HighQuality;

                            // Scaling
                            float scaling;
                            float scalingY = (float)source.Height / height;
                            float scalingX = (float)source.Width / width;
                            if (scalingX < scalingY) scaling = scalingX; else scaling = scalingY;

                            int newWidth = (int)(source.Width / scaling);
                            int newHeight = (int)(source.Height / scaling);

                            // Correct float to int rounding
                            if (newWidth < width) newWidth = width;
                            if (newHeight < height) newHeight = height;

                            // See if image needs to be cropped
                            int shiftX = 0;
                            int shiftY = 0;

                            if (newWidth > width)
                            {
                                shiftX = (newWidth - width) / 2;
                            }

                            if (newHeight > height)
                            {
                                shiftY = (newHeight - height) / 2;
                            }

                            // Draw image
                            g.DrawImage(source, -shiftX, -shiftY, newWidth, newHeight);
                        }

                        result = new Bitmap(target);
                    }
                }
                else
                {
                    // Image size matched the given size
                    result = new Bitmap(source);
                }
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }

        public Image SetImageOpacity(Image Source, float Opacity)
        {
            Bitmap Bmp = new Bitmap(Source.Width, Source.Height);

            using (Graphics g = Graphics.FromImage(Bmp))
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix33 = Opacity;
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default,
                                                  ColorAdjustType.Bitmap);
                g.DrawImage(Source, new Rectangle(0, 0, Bmp.Width, Bmp.Height),
                                   0, 0, Source.Width, Source.Height,
                                   GraphicsUnit.Pixel, attributes);
            }

            return Bmp;
        }

        public Image SetImageGradient(Image InputImage)
        {
            Bitmap adjImage = new Bitmap(InputImage.Width, InputImage.Height);

            Graphics g = Graphics.FromImage(adjImage);

            LinearGradientBrush linearGradientBrush = new LinearGradientBrush(
                new Rectangle(0, 0, adjImage.Width, adjImage.Height),
                Color.White,
                Color.Transparent,
                0f);

            Rectangle rect = new Rectangle(0, 0, adjImage.Width, adjImage.Height);
            g.FillRectangle(linearGradientBrush, rect);

            int x;
            int y;
            for (x = 0; x < adjImage.Width; ++x)
            {
                for (y = 0; y < adjImage.Height; ++y)
                {
                    Color inputPixelColor = (InputImage as Bitmap).GetPixel(x, y);
                    Color adjPixelColor = adjImage.GetPixel(x, y);
                    Color newColor = Color.FromArgb(adjPixelColor.A, inputPixelColor.R, inputPixelColor.G, inputPixelColor.B);
                    adjImage.SetPixel(x, y, newColor);
                }
            }

            return adjImage;
        }

        public string EncryptString(string value)
        {
            string Key = "1F2CAB925HFBBCAE589632FABC547892";
            string IV = "ACtEDg70CcU=";

            TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();
            TripleDES.Key = Convert.FromBase64String(Key);
            TripleDES.IV = Convert.FromBase64String(IV);
            TripleDES.Mode = CipherMode.ECB;
            TripleDES.Padding = PaddingMode.PKCS7;

            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;

            ct = TripleDES.CreateEncryptor(TripleDES.Key, TripleDES.IV);

            byt = Encoding.UTF8.GetBytes(value);

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();

            cs.Close();

            return Convert.ToBase64String(ms.ToArray());


        }

        public string DecryptString(string value)
        {
            string Key = "1F2CAB925HFBBCAE589632FABC547892";
            string IV = "ACtEDg70CcU=";

            TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();
            TripleDES.Key = Convert.FromBase64String(Key);
            TripleDES.IV = Convert.FromBase64String(IV);
            TripleDES.Mode = CipherMode.ECB;
            TripleDES.Padding = PaddingMode.PKCS7;

            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;

            ct = TripleDES.CreateDecryptor(TripleDES.Key, TripleDES.IV);

            byt = Convert.FromBase64String(value);

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();

            cs.Close();

            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}
