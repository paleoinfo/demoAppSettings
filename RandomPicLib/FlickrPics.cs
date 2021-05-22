using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace RandomPicLib
{
    public static class FlickrPics
    {
        /// <summary>Gets the random pic.</summary>
        /// <param name="topics">The topics.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async static Task<Image> getRandomPic(List<string> topics = null)
        {
            if (topics == null)
                topics = new List<string> { "dog", "cat", "car", "italy" };

            var rnd = new Random();
            var randomTopic = topics[rnd.Next(0, topics.Count)];

            byte[] data = null;
            using (WebClient client = new WebClient())
            {
                data = await client.DownloadDataTaskAsync(new Uri($@"https://loremflickr.com/320/240/{randomTopic}"));
            }

            Bitmap newBitmap = GetImageFromByteArray(data);

            return newBitmap;
        }

        #region https://stackoverflow.com/questions/3801275/how-to-convert-image-to-byte-array/16576471#16576471
        // ImageConverter object used to convert byte arrays containing JPEG or PNG file images into
        //  Bitmap objects. This is static and only gets instantiated once.
        private static readonly ImageConverter _imageConverter = new ImageConverter();

        /// <summary>
        /// Method that uses the ImageConverter object in .Net Framework to convert a byte array,
        /// presumably containing a JPEG or PNG file image, into a Bitmap object, which can also be
        /// used as an Image object.
        /// </summary>
        /// <param name="byteArray">byte array containing JPEG or PNG file image or similar</param>
        /// <returns>Bitmap object if it works, else exception is thrown</returns>
        public static Bitmap GetImageFromByteArray(byte[] byteArray)
        {
            Bitmap bm = (Bitmap)_imageConverter.ConvertFrom(byteArray);

            if (bm != null && (bm.HorizontalResolution != (int)bm.HorizontalResolution ||
                               bm.VerticalResolution != (int)bm.VerticalResolution))
            {
                // Correct a strange glitch that has been observed in the test program when converting
                //  from a PNG file image created by CopyImageToByteArray() - the dpi value "drifts"
                //  slightly away from the nominal integer value
                bm.SetResolution((int)(bm.HorizontalResolution + 0.5f),
                                 (int)(bm.VerticalResolution + 0.5f));
            }

            return bm;
        }

        /// <summary>
        /// Method to "convert" an Image object into a byte array, formatted in PNG file format, which
        /// provides lossless compression. This can be used together with the GetImageFromByteArray()
        /// method to provide a kind of serialization / deserialization.
        /// </summary>
        /// <param name="theImage">Image object, must be convertable to PNG format</param>
        /// <returns>byte array image of a PNG file containing the image</returns>
        public static byte[] CopyImageToByteArray(Image theImage)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                theImage.Save(memoryStream, ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }
        #endregion

    }
}