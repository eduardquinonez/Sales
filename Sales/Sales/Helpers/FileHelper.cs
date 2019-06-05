namespace Sales.Helpers
{
    using System.IO;

    public class FileHelper
    {
        public static byte[] Readfully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
