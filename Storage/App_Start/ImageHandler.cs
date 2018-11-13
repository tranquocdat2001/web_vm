using System.IO;
using System.Web;
using System.Web.Routing;
using System.Drawing.Imaging;
using System.Drawing;
using System;
using System.Drawing.Drawing2D;
using System.Web.Caching;
using System.IO.Compression;
using Utilities;

namespace Storage.App_Start
{
    public class ImageRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            string pathnoimage = requestContext.HttpContext.Server.MapPath("~/noimage.png");
            string path = requestContext.RouteData.Values["path"].ToStringEx();


            string paramResizeWidth = requestContext.RouteData.Values["resizeWidth"].ToStringEx();
            string paramResizeHeight = requestContext.RouteData.Values["resizeHeight"].ToStringEx();
            string paramWidth = requestContext.RouteData.Values["width"].ToStringEx();
            string paramHeight = requestContext.RouteData.Values["height"].ToStringEx();

            int resizeWidth = paramResizeWidth == "-" ? 0 : paramResizeWidth.ToInt();
            int resizeHeight = paramResizeHeight == "-" ? 0 : paramResizeHeight.ToInt();

            bool isResize = resizeWidth > 0 || resizeHeight > 0;

            int width = paramWidth == "-" ? 0 : paramWidth.ToInt();
            int height = paramHeight == "-" ? 0 : paramHeight.ToInt();


            string policy = string.Concat(paramWidth, "x", paramHeight);

            if (isResize)
            {
                width = resizeWidth;
                height = resizeHeight;
                policy = string.Concat(paramResizeWidth, "x", paramResizeHeight);
            }

            if (!CheckPolicy(policy))
            {
                // return a 404 HttpHandler here
                requestContext.HttpContext.Response.Status = "Unsupported MediaType";
                requestContext.HttpContext.Response.StatusCode = 415;
                requestContext.HttpContext.Response.End();
                return null;
            }

            string originFilePath = requestContext.HttpContext.Server.MapPath("~/" + path.TrimStart('/'));

            string dirPath = Path.GetDirectoryName(path);
            //string fileName = Path.GetFileNameWithoutExtension(path);
            string fileExtension = Path.GetExtension(path);
            //string filePath = string.Format("{0}.{1}.{2}{3}", path, width, height, fileExtension);

            string filePath = $"{path}.{width}.{height}{fileExtension}";

            if (isResize)
            {
                //filePath = string.Format("{0}.resize.{1}.{2}{3}", path, width, height, fileExtension);
                filePath = $"{path}.resize.{width}.{height}{fileExtension}";
            }

            if (string.IsNullOrEmpty(filePath))
            {
                // return a 404 HttpHandler here
                requestContext.HttpContext.Response.Status = "404 not found";
                requestContext.HttpContext.Response.StatusCode = 404;
                requestContext.HttpContext.Response.End();
            }
            else
            {


                filePath = requestContext.HttpContext.Server.MapPath("~/" + filePath.TrimStart('/'));

                if (!File.Exists(filePath))
                {
                    if (isResize)
                    {
                        System.Drawing.Image img = ResizeImage(originFilePath, width, height);

                        img.Save(filePath, GetImageFormat(originFilePath));
                    }
                    else
                    {
                        Bitmap resizeBeforeCrop;

                        resizeBeforeCrop = ResizeImage(originFilePath, width, 0);

                        if (width == height)
                        {
                            if (resizeBeforeCrop.Width > resizeBeforeCrop.Height)
                            {
                                resizeBeforeCrop = ResizeImage(originFilePath, 0, width);
                            }
                            else if(resizeBeforeCrop.Width < resizeBeforeCrop.Height)
                            {
                                resizeBeforeCrop = ResizeImage(originFilePath, width, 0);
                            }
                        }

                        System.Drawing.Image img = CropImage(resizeBeforeCrop, width, height);

                        img.Save(filePath, GetImageFormat(originFilePath));
                    }
                }

                requestContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.Public);
                requestContext.HttpContext.Response.Cache.SetLastModified(DateTime.Now);
                requestContext.HttpContext.Response.Cache.SetExpires(DateTime.Now.AddDays(30));
                requestContext.HttpContext.Response.Cache.SetValidUntilExpires(true);

                requestContext.HttpContext.Response.Clear();
                requestContext.HttpContext.Response.ContentType = GetContentType(path);

                requestContext.HttpContext.Response.WriteFile(filePath);
                requestContext.HttpContext.Response.End();

            }
            return null;
        }

        static public void RemovedCallback(String k, Object item, CacheItemRemovedReason r)
        {
            ((Bitmap)item).Dispose();
            //LogMessage("Callback");
        }

        private static string GetContentType(string path)
        {
            switch (Path.GetExtension(path))
            {
                case ".bmp": return "image/bmp";
                case ".gif": return "image/gif";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png": return "image/png";
                default: break;
            }
            return "";
        }

        private static ImageFormat GetImageFormat(string path)
        {
            switch (Path.GetExtension(path))
            {
                case ".bmp": return ImageFormat.Bmp;
                case ".gif": return ImageFormat.Gif;
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".png": return ImageFormat.Png;
                default: break;
            }

            return ImageFormat.Jpeg;
        }

        private ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] encoders;

            encoders = ImageCodecInfo.GetImageEncoders();

            for (int j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)

                    return encoders[j];
            }

            return null;

        }

        private static bool CheckPolicy(string dim)
        {
            string dimArray = AppSettings.Instance.GetString("PolicyOfDimArray", "|120x120|");

            if (!dimArray.StartsWith("|")) dimArray = string.Concat("|", dimArray);
            if (!dimArray.EndsWith("|")) dimArray = string.Concat(dimArray, "|");

            return dimArray.Contains(string.Concat("|", dim, "|"));
        }


        private static Bitmap CropImage(string filePath, int width, int height, bool fromCenter = true)
        {
            Bitmap imageSource = System.Drawing.Image.FromFile(filePath) as Bitmap;

            if (width <= 0 && height <= 0) return imageSource;

            return CropImage(imageSource, width, height, fromCenter);
        }

        private static Bitmap CropImage(Bitmap imageSource, int width, int height, bool fromCenter = true)
        {
            if (width <= 0 && height <= 0) return imageSource;

            double ratio = 0;

            if (width == 0)
            {
                if (height > imageSource.Height)
                {
                    height = imageSource.Height;
                    width = imageSource.Width;
                }
                else
                {
                    ratio = (double)height / (double)imageSource.Height;
                    width = (int)(imageSource.Width * ratio);
                }
            }
            else if (height == 0)
            {
                if (width > imageSource.Width)
                {
                    height = imageSource.Height;
                    width = imageSource.Width;
                }
                else
                {
                    ratio = (double)width / (double)imageSource.Width;
                    height = (int)(imageSource.Height * ratio);
                }
            }
            else
            {
                if (width > imageSource.Width && height > imageSource.Height)
                {
                    height = imageSource.Height;
                    width = imageSource.Width;
                }
                else if (height > imageSource.Height)
                {
                    height = imageSource.Height;
                }
                else if (width > imageSource.Width)
                {
                    width = imageSource.Width;
                }
            }

            int x = 0;
            int y = 0;

            if (fromCenter)
            {
                x = (int)((imageSource.Width - width) / 2);
                y = (int)((imageSource.Height - height) / 2);
            }

            Rectangle destinationRectang = new Rectangle(x, y, width, height);
            Rectangle sourceRectang = new Rectangle(x, y, imageSource.Width, imageSource.Height);

            // Create new bitmap:
            Bitmap target = imageSource.Clone(destinationRectang, imageSource.PixelFormat);

            return target;
        }

        private static Bitmap ResizeImage(string filePath, int width, int height)
        {
            Bitmap imageSource = System.Drawing.Image.FromFile(filePath) as Bitmap;

            return ResizeImage(imageSource, width, height);
        }

        private static Bitmap ResizeImage(Bitmap imageSource, int width, int height)
        {
            if (width <= 0 && height <= 0) return imageSource;

            double ratio = 0;

            if (width == 0)
            {
                if (height > imageSource.Height)
                {
                    height = imageSource.Height;
                    width = imageSource.Width;
                }
                else
                {
                    ratio = (double)height / (double)imageSource.Height;
                    width = (int)(imageSource.Width * ratio);
                }
            }
            else if (height == 0)
            {
                if (width > imageSource.Width)
                {
                    height = imageSource.Height;
                    width = imageSource.Width;
                }
                else
                {
                    ratio = (double)width / (double)imageSource.Width;
                    height = (int)(imageSource.Height * ratio);
                }
            }
            else
            {
                // Nếu là ảnh ngang:
                if (imageSource.Width > imageSource.Height)
                {
                    if (width > imageSource.Width && height > imageSource.Height)
                    {
                        height = imageSource.Height;
                        width = imageSource.Width;
                    }
                    else if (height > imageSource.Height)
                    {
                        height = imageSource.Height;
                    }
                    else if (width > imageSource.Width)
                    {
                        width = imageSource.Width;
                    }
                }
                // Nếu là ảnh dài
                else
                {
                    ratio = (double)width / (double)imageSource.Width;
                    height = (int)(imageSource.Height * ratio);
                }
            }


            Size newSize = new Size(width, height);

            Bitmap bmp = new Bitmap(imageSource, newSize);

            return bmp;
        }
    }
}