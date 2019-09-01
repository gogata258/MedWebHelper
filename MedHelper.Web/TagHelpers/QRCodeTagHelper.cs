using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Drawing.Imaging;
using System.IO;
using ZXing.QrCode;

namespace MedHelper.Web.TagHelpers
{
	[HtmlTargetElement("qrcode")]
	public class QRCodeTagHelper : TagHelper
	{
		private const int QR_DIMENTION = 250;
		private const int QR_MARGIN = 0;
		public string QrUrl { get; set; }
		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			var qrCodeWriter = new ZXing.BarcodeWriterPixelData
			{
				Format = ZXing.BarcodeFormat.QR_CODE,
				Options = new QrCodeEncodingOptions
				{
					Height = QR_DIMENTION,
					Width = QR_DIMENTION,
					Margin = QR_MARGIN
				}
			};
			ZXing.Rendering.PixelData pixelData = qrCodeWriter.Write(QrUrl);
			// creating a bitmap from the raw pixel data; if only black and white colors are used it makes no difference    
			// that the pixel data ist BGRA oriented and the bitmap is initialized with RGB    
			using (var bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
			using (var ms = new MemoryStream( ))
			{
				BitmapData bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
				try
				{
					// we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image    
					System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
				}
				finally
				{
					bitmap.UnlockBits(bitmapData);
				}
				// save to stream as PNG    
				bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
				output.TagName = "img";
				output.Attributes.Clear( );
				output.Attributes.Add("width", QR_DIMENTION);
				output.Attributes.Add("height", QR_DIMENTION);
				output.Attributes.Add("alt", QrUrl);
				output.Attributes.Add("src", string.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray( ))));
			}
		}
	}
}

