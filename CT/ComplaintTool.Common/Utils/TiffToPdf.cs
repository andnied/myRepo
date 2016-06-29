using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace ComplaintTool.Common.Utils
{
    public static class TiffToPdf
    {
        public static bool ThumbnailCallback()
        {
            return false;
        }

        public static void TiffToPDF(string tiffPath, string pdfDestination)
        {
            string[] pngPaths;
            var doc = new PdfDocument();
            using (var bmp = new Bitmap(tiffPath))
            {
                // Get pages in bitmap
                var frames = bmp.GetFrameCount(FrameDimension.Page);
                pngPaths = new string[frames];
                for (var i = 0; i < frames; i++)
                {
                    bmp.SelectActiveFrame(FrameDimension.Page, i);
                    if (bmp.PixelFormat != PixelFormat.Format1bppIndexed)
                    {
                        Debug.WriteLine(bmp.PixelFormat);
                    }
                    using (var bmp2 = new Bitmap(bmp.Width, bmp.Height))
                    {
                        bmp2.Palette = bmp.Palette;
                        bmp2.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);

                        // create graphics object for new bitmap
                        using (var g = Graphics.FromImage(bmp2))
                        {
                            g.DrawImageUnscaled(bmp, 0, 0);

                        }
                        var pngPath = string.Format("{0}_{1}{2}", tiffPath.Substring(0, tiffPath.LastIndexOf(".", StringComparison.Ordinal)),
                                        i.ToString(CultureInfo.InvariantCulture), ".png");
                        if (File.Exists(pngPath))
                            File.Delete(pngPath);
                        bmp2.Save(pngPath, ImageFormat.Png);
                        pngPaths[i] = pngPath;
                    }
                }


            }
            for (var i = 0; i < pngPaths.Length; i++)
            {
                var pdfPage = new PdfPage();

                #region PreDef Page

                var size = PageSizeConverter.ToSize(PageSize.A4);
                pdfPage.Orientation = PageOrientation.Portrait;
                pdfPage.Width = size.Width;
                pdfPage.Height = size.Height;
                pdfPage.TrimMargins.Top = 0;
                pdfPage.TrimMargins.Right = 0;
                pdfPage.TrimMargins.Bottom = 0;
                pdfPage.TrimMargins.Left = 0;

                #endregion

                //.Portrait OR .Landscape
                var img = XImage.FromFile(pngPaths[i]);
                if (img.PixelWidth > img.PixelHeight)
                    pdfPage.Orientation = PageOrientation.Landscape;
                doc.Pages.Add(pdfPage);
                var xgr = XGraphics.FromPdfPage(pdfPage);
                xgr.DrawImage(img, 0, 0, pdfPage.Width, pdfPage.Height);
                //xgr.DrawImage(img, 0, 0, img.Size.Width, img.Size.Height);
                img.Dispose();
            }
            doc.Save(pdfDestination);
            doc.Close();
            foreach (var path in pngPaths)
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        public static void TiffToPDFBase(string tiffPath, string pdfDestination)
        {
            var pngPaths = new string[0];
            var doc = new PdfDocument();
            using (var tiffStream = new FileStream(tiffPath, FileMode.Open, FileAccess.Read))
            {
                //PreservePixelFormat
                var tifDecoder = new TiffBitmapDecoder(tiffStream, BitmapCreateOptions.PreservePixelFormat,
                    BitmapCacheOption.Default);
                if (tifDecoder.Frames.Count > 0)
                {
                    #region MultiPage

                    pngPaths = new string[tifDecoder.Frames.Count];

                    for (var i = 0; i < tifDecoder.Frames.Count; i++)
                    {
                        var pngEncoder = new PngBitmapEncoder();
                        pngEncoder.Frames.Add(tifDecoder.Frames[i]);

                        var pngPath = string.Format("{0}_{1}{2}",
                            tiffPath.Substring(0, tiffPath.LastIndexOf(".", StringComparison.Ordinal)),
                            i.ToString(CultureInfo.InvariantCulture), ".png");
                        pngPaths[i] = pngPath;

                        using (var pngStream = new FileStream(pngPath, FileMode.Create, FileAccess.Write))
                        {
                            pngEncoder.Save(pngStream);
                        }
                    }

                    for (var i = 0; i < pngPaths.Length; i++)
                    {
                        var pdfPage = new PdfPage();

                        #region PreDef Page

                        var size = PageSizeConverter.ToSize(PageSize.A4);
                        pdfPage.Orientation = PageOrientation.Portrait;
                        pdfPage.Width = size.Width;
                        pdfPage.Height = size.Height;
                        pdfPage.TrimMargins.Top = 0;
                        pdfPage.TrimMargins.Right = 0;
                        pdfPage.TrimMargins.Bottom = 0;
                        pdfPage.TrimMargins.Left = 0;

                        #endregion

                        //.Portrait OR .Landscape
                        var img = XImage.FromFile(pngPaths[i]);
                        if (img.PixelWidth > img.PixelHeight)
                            pdfPage.Orientation = PageOrientation.Landscape;
                        doc.Pages.Add(pdfPage);
                        var xgr = XGraphics.FromPdfPage(pdfPage);
                        xgr.DrawImage(img, 0, 0, img.Size.Width, img.Size.Height);
                        img.Dispose();
                    }

                    #endregion
                }
                else
                {
                    var pdfPage = new PdfPage();

                    #region PreDef Page

                    var size = PageSizeConverter.ToSize(PageSize.A4);
                    pdfPage.Orientation = PageOrientation.Portrait;
                    pdfPage.Width = size.Width;
                    pdfPage.Height = size.Height;
                    pdfPage.TrimMargins.Top = 0;
                    pdfPage.TrimMargins.Right = 0;
                    pdfPage.TrimMargins.Bottom = 0;
                    pdfPage.TrimMargins.Left = 0;

                    #endregion

                    var img = XImage.FromFile(tiffPath);
                    if (img.PixelWidth > img.PixelHeight)
                        pdfPage.Orientation = PageOrientation.Landscape;
                    doc.Pages.Add(pdfPage);
                    var xgr = XGraphics.FromPdfPage(pdfPage);
                    xgr.DrawImage(img, 0, 0, img.Size.Width, img.Size.Height);
                    img.Dispose();
                }
            }


            doc.Save(pdfDestination);
            doc.Close();
            foreach (var path in pngPaths)
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }
    }
}
