using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TouchSignature
{

    public class SignatureControl : Control
    {

        #region Properties
        public Bitmap BackGroundImage;
        private Pen SignaturePen = new Pen(Color.Black, 1);
        private Point LastMouseCoordinates = new Point(0, 0);
        private bool CaptureMouseCoordinates = false;
        private int areaWidth;
        private int areaHeight;

        private struct LineToDraw
        {
            public int StartX;
            public int StartY;
            public int EndX;
            public int EndY;
        }
        #endregion

        #region Constructor
        public SignatureControl(int width, int height)
        {
            areaWidth = width;
            areaHeight = height;
        }
        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(BackGroundImage, 0, 0, BackGroundImage.Size.Width, BackGroundImage.Size.Height);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {

        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (CaptureMouseCoordinates) { return; }

            CaptureMouseCoordinates = true;
            LastMouseCoordinates.X = e.X;
            LastMouseCoordinates.Y = e.Y;

        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            CaptureMouseCoordinates = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!CaptureMouseCoordinates) { return; }

            LineToDraw l = new LineToDraw();

            l.StartX = LastMouseCoordinates.X;
            l.StartY = LastMouseCoordinates.Y;
            l.EndX = e.X;
            l.EndY = e.Y;

            // Draw line into the buffer
            using (Graphics bufferGrph = Graphics.FromImage(BackGroundImage))
            {
                bufferGrph.DrawLine(SignaturePen, l.StartX, l.StartY, l.EndX, l.EndY);
            }

            LastMouseCoordinates.X = l.EndX;
            LastMouseCoordinates.Y = l.EndY;

            Invalidate();

        }

        public void Save(string FileName)
        {

            try
            {
                var bmpString = ConvertBitmapToString(BackGroundImage);

                File.WriteAllText(FileName, bmpString);

            }
            catch (Exception) { throw; }
        }

        public void Load(string FileName)
        {

            try
            {
                var base64String = File.ReadAllText(FileName);
                BackGroundImage = ConvertStringToBmp(base64String);

            }
            catch (Exception) { throw; }
        }

        public void Clear(bool ClearCapturedLines)
        {
            try
            {
                LoadBackgroundImage();
                Invalidate();
                if (ClearCapturedLines == false) { return; }

                using (Graphics bufferGrph = Graphics.FromImage(BackGroundImage))
                {
                    bufferGrph.Clear(Color.White);
                }

            }
            catch (Exception) { throw; }
        }

        private void LoadBackgroundImage()
        {
            try
            {
                Bitmap newBuffer = new Bitmap(areaWidth, areaHeight);
                if (BackGroundImage != null)
                {
                    using (Graphics bufferGrph = Graphics.FromImage(newBuffer))
                    {
                        bufferGrph.DrawImageUnscaled(BackGroundImage, Point.Empty);
                    }
                }

                BackGroundImage = newBuffer;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string ConvertBitmapToString(Bitmap bmp)
        { 
            var stream = new System.IO.MemoryStream();

            bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

            var strData = Convert.ToBase64String(stream.ToArray());

            return strData;
        }

        private Bitmap ConvertStringToBmp(string bmp)
        {
            byte[] byteBuffer = Convert.FromBase64String(bmp);
            MemoryStream memoryStream = new MemoryStream(byteBuffer);
            memoryStream.Position = 0;

            Bitmap bitmapImage = new Bitmap(memoryStream);

            return bitmapImage;
        }
    }
}
