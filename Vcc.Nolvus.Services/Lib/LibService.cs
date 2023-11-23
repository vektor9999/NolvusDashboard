using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Aspose.Imaging;
using Aspose.Imaging.Brushes;
using Aspose.Imaging.FileFormats.Webp;


namespace Vcc.Nolvus.Services.Lib
{
    public class LibService : ILibService
    {
        public System.Drawing.Image ResizeKeepAspectRatio(System.Drawing.Image source, int width, int height)
        {
            System.Drawing.Image result = null;

            try
            {
                if (source.Width != width || source.Height != height)
                {
                    // Resize image
                    float sourceRatio = (float)source.Width / source.Height;

                    using (var target = new System.Drawing.Bitmap(width, height))
                    {
                        using (var g = System.Drawing.Graphics.FromImage(target))
                        {
                            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

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

        public System.Drawing.Image SetImageOpacity(System.Drawing.Image Source, float Opacity)
        {
            System.Drawing.Bitmap Bmp = new System.Drawing.Bitmap(Source.Width, Source.Height);

            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(Bmp))
            {
                System.Drawing.Imaging.ColorMatrix matrix = new System.Drawing.Imaging.ColorMatrix();
                matrix.Matrix33 = Opacity;
                System.Drawing.Imaging.ImageAttributes attributes = new System.Drawing.Imaging.ImageAttributes();
                attributes.SetColorMatrix(matrix, System.Drawing.Imaging.ColorMatrixFlag.Default,
                                                  System.Drawing.Imaging.ColorAdjustType.Bitmap);
                g.DrawImage(Source, new System.Drawing.Rectangle(0, 0, Bmp.Width, Bmp.Height),
                                   0, 0, Source.Width, Source.Height,
                                   System.Drawing.GraphicsUnit.Pixel, attributes);
            }

            return Bmp;
        }

        public System.Drawing.Image SetImageGradient(System.Drawing.Image InputImage)
        {
            System.Drawing.Bitmap adjImage = new System.Drawing.Bitmap(InputImage.Width, InputImage.Height);

            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(adjImage);

            System.Drawing.Drawing2D.LinearGradientBrush linearGradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                new System.Drawing.Rectangle(0, 0, adjImage.Width, adjImage.Height),
                System.Drawing.Color.White,
                System.Drawing.Color.Transparent,
                0f);

            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, adjImage.Width, adjImage.Height);
            g.FillRectangle(linearGradientBrush, rect);

            int x;
            int y;
            for (x = 0; x < adjImage.Width; ++x)
            {
                for (y = 0; y < adjImage.Height; ++y)
                {
                    System.Drawing.Color inputPixelColor = (InputImage as System.Drawing.Bitmap).GetPixel(x, y);
                    System.Drawing.Color adjPixelColor = adjImage.GetPixel(x, y);
                    System.Drawing.Color newColor = System.Drawing.Color.FromArgb(adjPixelColor.A, inputPixelColor.R, inputPixelColor.G, inputPixelColor.B);
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

        public System.Drawing.Image GetImageFromWebStream(string ImageUrl)
        {
            System.Drawing.Image Result;

            var WebStream = WebRequest.Create(ImageUrl).GetResponse().GetResponseStream();

            MemoryStream Stream = new MemoryStream();

            byte[] chunk = new byte[4096];
            int bytesRead;
            while ((bytesRead = WebStream.Read(chunk, 0, chunk.Length)) > 0)
            {
                Stream.Write(chunk, 0, bytesRead);
            }

            Stream.Seek(0, System.IO.SeekOrigin.Begin);
           
            using (WebPImage LoadedImage = (WebPImage)Aspose.Imaging.Image.Load(Stream))
            {
                Result = LoadedImage.ToBitmap();                
            }            

            return Result;
        }
    }
}
