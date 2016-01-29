using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace JQMApp.Models
{
    public class Scaler
    {
        private int GetWidth(int oldWidth, int oldHeight, int newHeight)
        {
            if (((oldWidth <= 0) || (oldHeight <= 0)) || (newHeight <= 0))
            {
                return oldWidth;
            }
            double num = ((double)newHeight) / ((double)oldHeight);
            int num2 = (int)Math.Round((double)(num * oldWidth));
            if (num2 < 1)
            {
                num2 = 1;
            }
            return num2;

        }

        private int GetHeight(int oldWidth, int oldHeight, int newWidth)
        {
            if (((oldWidth <= 0) || (oldHeight <= 0)) || (newWidth <= 0))
            {
                return oldHeight;
            }
            double num = ((double)newWidth) / ((double)oldWidth);
            int num2 = (int)Math.Round((double)(num * oldHeight));
            if (num2 < 1)
            {
                num2 = 1;
            }
            return num2;
        }
        public byte[] ScaleImage(HttpPostedFileBase postedFile, int width, string orientation)
        {
            int height = 0;
            byte[] data;
            Image image = Image.FromStream(postedFile.InputStream);

            if (orientation == "portrait")
            {
                image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }

            height = this.GetHeight(image.Width, image.Height, width);

            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawImage(image, 0, 0, width, height);
            }

            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                data = stream.ToArray();
            }
            return data;
        }

        public byte[] CreateThumbNail(HttpPostedFileBase postedFile, int height)
        {
            byte[] data;
            Image image = Image.FromStream(postedFile.InputStream);
            int width = this.GetWidth(image.Width, image.Height, height);
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawImage(image, 0, 0, width, height);
            }

            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                data = stream.ToArray();
            }
            return data;
        }

        public byte[][] CreatePreviewThumbs(HttpPostedFileBase postedFile, int width)
        {
            int height = 0;
            var thumbs = new byte[2][];
            Image image = Image.FromStream(postedFile.InputStream);
            height = this.GetHeight(image.Width, image.Height, width);

            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawImage(image, 0, 0, width, height);
            }

            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                thumbs[0] = stream.ToArray();
            }

            // Create second thumbnail
            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            height = this.GetHeight(image.Width, image.Height, width);

            bitmap = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawImage(image, 0, 0, width, height);
            }

            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                thumbs[1] = stream.ToArray();
            }

            return thumbs;

        }
    }
}