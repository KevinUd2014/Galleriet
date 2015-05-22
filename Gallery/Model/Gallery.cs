using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Gallery.Model
{
    public class Gallery
    {
        //#region regex Vet inte om denna ska användas!
        //msdn.microsoft.com/en-us/library/9a1ybwek.aspx

        private readonly Regex ApprovedExtenstions; //= new Regex(@"^.*\.(gif|jpg|png)$");  // ^.*.(gif|jpg|png)$ ska användas för att ta reda på så det är en gif, jpg eller en png som angetts Reguljära uttryck!
        private string PhysicalUploadedImagesPath;  // ska man använda , RegexOptions.IgnoreCase?????
        private readonly Regex SantizePath;
        private static string PhysicalUploadedThumbnailPath;

        //AppDomain.CurrentDomain.GetData("APPBASE").ToString();
        //path.combine

        public Gallery()  // kunde inte ha static här!! och ite på de andra heller vet inte vad felet var/är!
        {
            PhysicalUploadedImagesPath = Path.Combine(AppDomain.CurrentDomain.GetData("APPBASE").ToString(), "Content", "Pictures");
            // Skapar den vägen som bilderna ska hämtas ifrån!

            PhysicalUploadedThumbnailPath = Path.Combine(PhysicalUploadedImagesPath, "ThumbnailPics");

            ApprovedExtenstions = new Regex(@"^.*\.(gif|jpg|png)$", RegexOptions.IgnoreCase);
            // skapar en godkänd variael som accepterar rätt bildsort!

            var invalidChars = new string(Path.GetInvalidFileNameChars());  //  SKapar en variabel med dom icke godkända filnamnen!
            SantizePath = new Regex(string.Format("[{0}]", Regex.Escape(invalidChars)));
            //Ska fixa så att bilden kommer dit där den hör hemma!
        }

        public IEnumerable<string> GetImageName()
        {
            var directory = new DirectoryInfo(PhysicalUploadedThumbnailPath);

            FileInfo[] fileInfo = directory.GetFiles();  // denna kan vara fel!!

            return fileInfo.Select(image => image.Name).Where(filename => ApprovedExtenstions.IsMatch(filename)).ToList();  // fick hjälp med denna!
        }

        public bool ImageExists(string name)
        {
            return File.Exists(Path.Combine(PhysicalUploadedImagesPath, name));// om ett namn redan finns så ska denna köras
        }

        private bool IsValidImage(Image image)// denna ska köras om bilden godkännes!!
        {
            return image.RawFormat.Guid == ImageFormat.Jpeg.Guid ||
                image.RawFormat.Guid == ImageFormat.Gif.Guid ||
                image.RawFormat.Guid == ImageFormat.Png.Guid;

            //kör denna om bilden är ok!
        }

        public string SaveImage(Stream stream, string fileName)  // denna ska ändra namnet på bilden om det behövs!
        {
            var picture = Image.FromStream(stream);  // skapar en massa nya variablar som ska användas senare i metoden!
            if (IsValidImage(picture))
            {
                throw new ArgumentException("Invalid type of file.");
            }

            var changeFileName = SantizePath.Replace(fileName, "");
            var wholeFileName = Path.GetFileNameWithoutExtension(changeFileName);
            var extension = Path.GetExtension(changeFileName);
            var count = 0;

            picture = System.Drawing.Image.FromStream(stream);  // fick denna i pdfen!
            var thumbnail = picture.GetThumbnailImage(60, 45, null, System.IntPtr.Zero);

            while (ImageExists(changeFileName))
            {
                changeFileName = string.Format(wholeFileName + "({0}){1}", ++count, extension);  // denna lägger in de nya namn "verktygen" till bilden!
            }

            thumbnail.Save(Path.Combine(PhysicalUploadedThumbnailPath, changeFileName)); // path -> fullständig fysisk filnamn inklusive sökväg
            picture.Save(Path.Combine(PhysicalUploadedImagesPath, changeFileName));

            return changeFileName;

            //kör denna och ska spara bilden!
        }
    }
}