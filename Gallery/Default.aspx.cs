using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gallery
{
    public partial class Default : System.Web.UI.Page
    {
        
        private Model.Gallery _gallery;
        public Model.Gallery Gallery
        {
            get { return _gallery ?? ( _gallery = new Model.Gallery()); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            {
                //BigImage.Visible = false;
                if (Session["UploadSuccess"] as bool? == true)  // fick hjälp med denna sats då jag inte visste alls hur man skulle göra detta!
                {
                    SuccessUpload.Visible = true;
                    SuccessLabel.Text = "Upload of - " + Session["FileName"] + "Was a success";
                    Session["UploadSuccess"] = false;
                }
                
                string BiggerSizePicture = Request.Url.ToString();
                string bigPictureName = Path.GetFileName(BiggerSizePicture);

                //string BiggerSizePicture = Request.QueryString["name"];
                if (BiggerSizePicture != "Default.aspx")
                {
                    BigImage.ImageUrl = "~/Content/Pictures/" + bigPictureName;
                }  // hit!
            }
        }
        public IEnumerable<string> PictureRepeater_GetData()
        {
            return Gallery.GetImageName();
        }
        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if(IsValid)
            {
                try
                {
                    Session["FileName"] = Gallery.SaveImage(FileUpload.FileContent, FileUpload.FileName);
                    Session["UploadSuccess"] = true;
                    Response.Redirect("http://localhost:53566/Default.aspx?name=/" + Session["FileName"]);
                }
                catch
                {
                    CustomValidator customvalidator = new CustomValidator();
                    customvalidator.IsValid = false;
                    customvalidator.ErrorMessage = "Failure";
                    Page.Validators.Add(customvalidator);
                }
            }
        }
        protected void CloseImageButton_Click(object sender, ImageClickEventArgs e)
        {
            SuccessUpload.Visible = false;
        }

        //SKa finnas en till knapp här!
    }
}