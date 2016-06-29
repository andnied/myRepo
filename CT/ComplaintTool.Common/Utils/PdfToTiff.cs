using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using ComplaintTool.Common.Properties;
using Ghostscript.NET;
using Ghostscript.NET.Viewer;

namespace ComplaintTool.Common.Utils
{
    public static class PdfToTiff
    {
        public static void ExportPdfToTiff(string sourcePdfFileName, string destinationTiffFolder)
        {
            var viewer = new GhostscriptViewer();
            viewer.Open(sourcePdfFileName);

            var dev = new GhostscriptPngDevice(GhostscriptPngDeviceType.PngMono)
            {
                GraphicsAlphaBits = GhostscriptImageDeviceAlphaBits.V_4,
                TextAlphaBits = GhostscriptImageDeviceAlphaBits.V_4,
                ResolutionXY = new GhostscriptImageDeviceResolution(300, 300)
            };
            dev.InputFiles.Add(sourcePdfFileName);
            dev.Pdf.FirstPage = viewer.FirstPageNumber;//2;
            dev.Pdf.LastPage = viewer.LastPageNumber;//4;
            dev.CustomSwitches.Add("-dDOINTERPOLATE");
            dev.OutputPath = destinationTiffFolder;
            dev.Process();

            viewer.Close();

            //var tempDirectory = Path.GetDirectoryName(@"C:\test\temp\dok001_%03d.png");

            //string[] fileList = Directory.GetFiles(tempDirectory);

            //foreach (var file in fileList)
            //{
            //    Console.WriteLine(file);
            //    string tiffFile = @"C:\test\temp\" + Path.GetFileNameWithoutExtension(file) + ".tif";
            //    using (var png = new Bitmap(file)) {
            //        if(File.Exists(tiffFile))
            //            File.Delete(tiffFile);

            //        png.Save(tiffFile, ImageFormat.Tiff);
            //    }

            //    File.Delete(file);
            //}

        }

        public static bool SaveMultipage(string[] fileList, string location, string type)
        {
            var codecInfo = GetCodecForstring(type);
            var bmp2 = new List<Image>();
            //int count = 0;
            foreach (var file in fileList)
            {
                using (var png = new Bitmap(file))
                {
                    Image tempImage = ConvertToBitonal(png);
                    bmp2.Add(tempImage);
                }
            }
            if (bmp2.Count == 1)
            {
                var iparams = new EncoderParameters(1);
                var iparam = Encoder.Compression;
                var iparamPara = new EncoderParameter(iparam, (long)(EncoderValue.CompressionCCITT4));
                iparams.Param[0] = iparamPara;
                bmp2[0].Save(location, codecInfo, iparams);
            }
            else if (bmp2.Count > 1)
            {
                var encoderParams = new EncoderParameters(2);

                var saveEncoder = Encoder.SaveFlag;
                var compressionEncoder = Encoder.Compression;

                // Save the first page (frame).
                var saveEncodeParam = new EncoderParameter(saveEncoder, (long)EncoderValue.MultiFrame);
                var compressionEncodeParam = new EncoderParameter(compressionEncoder,
                    (long)EncoderValue.CompressionCCITT4);
                encoderParams.Param[0] = compressionEncodeParam;
                encoderParams.Param[1] = saveEncodeParam;

                File.Delete(location);
                bmp2[0].Save(location, codecInfo, encoderParams);


                for (var i = 1; i < bmp2.Count; i++)
                {
                    if (bmp2[i] == null)
                        break;

                    saveEncodeParam = new EncoderParameter(saveEncoder, (long)EncoderValue.FrameDimensionPage);
                    compressionEncodeParam = new EncoderParameter(compressionEncoder,
                        (long)EncoderValue.CompressionCCITT4);
                    encoderParams.Param[0] = compressionEncodeParam;
                    encoderParams.Param[1] = saveEncodeParam;
                    bmp2[0].SaveAdd(bmp2[i], encoderParams);

                }

                saveEncodeParam = new EncoderParameter(saveEncoder, (long)EncoderValue.Flush);
                encoderParams.Param[0] = saveEncodeParam;
                bmp2[0].SaveAdd(encoderParams);
            }
            return true;
        }

        private static ImageCodecInfo GetCodecForstring(string type)
        {
            var info = ImageCodecInfo.GetImageEncoders();

            for (var i = 0; i < info.Length; i++)
            {
                var enumName = type;
                if (info[i].FormatDescription.Equals(enumName))
                {
                    return info[i];
                }
            }

            return null;

        }

        private static Bitmap ConvertToBitonal(Bitmap original)
        {
            Bitmap source;
            if (original.PixelFormat != PixelFormat.Format32bppArgb)
            {
                source = new Bitmap(original.Width, original.Height, PixelFormat.Format32bppArgb);
                source.SetResolution(original.HorizontalResolution, original.VerticalResolution);
                using (var g = Graphics.FromImage(source))
                {
                    g.DrawImageUnscaled(original, 0, 0);
                }
            }
            else
            {
                source = original;
            }
            var sourceData = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var imageSize = sourceData.Stride * sourceData.Height;
            var sourceBuffer = new byte[imageSize];
            Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, imageSize);
            source.UnlockBits(sourceData);
            var destination = new Bitmap(source.Width, source.Height, PixelFormat.Format1bppIndexed);
            var destinationData = destination.LockBits(new Rectangle(0, 0, destination.Width, destination.Height), ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);
            imageSize = destinationData.Stride * destinationData.Height;
            var destinationBuffer = new byte[imageSize];

            var height = source.Height;
            var width = source.Width;
            const int threshold = 500;
            for (var y = 0; y < height; y++)
            {
                var sourceIndex = y * sourceData.Stride;
                var destinationIndex = y * destinationData.Stride;
                byte destinationValue = 0;
                var pixelValue = 128;
                for (var x = 0; x < width; x++)
                {
                    var pixelTotal = sourceBuffer[sourceIndex + 1] + sourceBuffer[sourceIndex + 2] + sourceBuffer[sourceIndex + 3];
                    if (pixelTotal > threshold)
                    {
                        destinationValue += (byte)pixelValue;
                    }
                    if (pixelValue == 1)
                    {
                        destinationBuffer[destinationIndex] = destinationValue;
                        destinationIndex++;
                        destinationValue = 0;
                        pixelValue = 128;
                    }
                    else
                    {
                        pixelValue >>= 1;
                    }
                    sourceIndex += 4;
                }
                if (pixelValue != 128)
                {
                    destinationBuffer[destinationIndex] = destinationValue;
                }
            }
            Marshal.Copy(destinationBuffer, 0, destinationData.Scan0, imageSize);
            destination.UnlockBits(destinationData);
            return destination;
        }
    }
}
