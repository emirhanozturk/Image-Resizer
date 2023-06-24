using HtmlAgilityPack;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageResizer.WebAPI.Services
{
    public class ResizeImageFileService : IResizeImageFileService
    {

        public void ResizeImageFile(string url)
        {
            #region Get Image Attributes
            using WebClient client = new WebClient();
            var htmlContent = client.DownloadString(url);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);
            HtmlNodeCollection collection = doc.DocumentNode.SelectNodes("//script[@type='application/ld+json']");
            var imageSource = collection[1].InnerHtml;
            JObject jsonObject = JObject.Parse(imageSource);
            string imageUrl = (string)jsonObject["image"];
            var urlParts = imageUrl.Split(new[] { '/' });
            var fileName = urlParts[urlParts.Length - 1];
            Console.WriteLine(fileName);
            #endregion

            #region DownloadFile
            client.DownloadFile(imageUrl, fileName);
            Console.WriteLine("İşlem tamam.");
            #endregion

            #region Resize Image

            try
            {
                using (Bitmap imageFile = new Bitmap(fileName))
                {
                    

                    using (Bitmap resizedImage = new Bitmap(150, 150))
                    {
                        resizedImage.SetResolution(imageFile.HorizontalResolution, imageFile.VerticalResolution);

                        using (Graphics graphics = Graphics.FromImage(resizedImage))
                        {
                            graphics.Clear(Color.Transparent);
                            graphics.DrawImage(imageFile, 0, 0, 150, 150);
                        }

                        resizedImage.Save("resized_" + fileName, ImageFormat.Png);
                        Console.WriteLine("Resim başarıyla yeniden boyutlandırıldı ve kaydedildi.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata oluştu: " + ex.Message);
            }
            #endregion
            return;
        }
    }
}
