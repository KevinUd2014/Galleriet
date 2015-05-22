<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gallery.Default" ViewStateMode="Disabled"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Script/JavaScript.js"></script>
    <link href="CSS/CSS.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>

        <%--Denna första diven kommer skriva ut om uppladdningen lyckades eller inte! --%>
        <asp:PlaceHolder ID="SuccessUpload" runat="server" 
            Visible="false">
        
            <div id="UploadSuccess">
            
                <asp:ImageButton ID="CloseImageButton" runat="server" 
                    ImageUrl="~/Content/Close.png" 
                    CssClass="ImgButtonSize" 
                    OnClick="CloseImageButton_Click" 
                    CausesValidation="false"/> 

                <asp:Label ID="SuccessLabel" runat="server" 
                    Text=""></asp:Label>

            </div>

        </asp:PlaceHolder>

        <%-- Den stora bilden kommer att vissas i denna image tagg! --%>
        
        <div>
            <asp:Image ID="BigImage" runat="server" CssClass="BigImage" />

        </div>
        <%-- Skapar en repeater vör menyn av bilder! --%>
        
        <div id="ThumbnailPictures">

            <asp:Repeater ID="PictureRepeater" runat="server" 
                SelectMethod="PictureRepeater_GetData"
                ItemType="System.String">
                
                <ItemTemplate> 

                    <asp:HyperLink runat="server"
                        ID="PictureHyperLink"
                        CssClass ="ThumbNailBackground"
                        ImageUrl ='<%#"~/Content/Pictures/ThumbnailPics/"+ Item %>'
                        NavigateURL='<%#"?name=/" + Item %>'> 
                    </asp:HyperLink>
                     
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <%-- Buttons and validators for uploading --%>
        <div id="ButtonBox">

            <asp:ValidationSummary ID="ValidationSummary" runat="server" 
                CssClass="Validation-Errors"/>

            <%-- Här har vi kanppern som ska lada upp bilden! --%>
            <asp:FileUpload ID="FileUpload" runat="server"/>
            
            <%-- En validator som ska validera uppladdnings knappen! Man måste ange en bild! --%>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator" 
                runat="server" 
                ErrorMessage="You have to choose a File!" 
                ControlToValidate="FileUpload" 
                Display="Dynamic" >*</asp:RequiredFieldValidator>

            <%-- Denna validator ska fixa så att man bara kan ladda upp gif, jpeg eller png bilder! --%>
            <asp:RegularExpressionValidator 
                ID="RegularExpressionValidator1" 
                runat="server" 
                ErrorMessage="Wrong File format! Only .JPEG, .GIF and .PNG are accepted" 
                ControlToValidate="FileUpload" 
                Display="Dynamic" 
                ValidationExpression="(.*?)\.(jpg|jpeg|png|gif)$">*</asp:RegularExpressionValidator> 
        </div>
        <div>

            <%-- Denna knapp ska iniialiseras så att man kan ladda upp om alla valideringar går igenom! --%>
            <asp:Button ID="UploadButton" 
                runat="server" 
                Text="Upload" 
                OnClick="UploadButton_Click" />

        </div>
    </div>

    </form>
</body>
</html>
