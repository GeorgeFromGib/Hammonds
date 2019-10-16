using System.Web.Mvc;

namespace MVCHtmlToPdf
{
    public class HtmlToPdfAction
    {
        public ActionResult RenderToAction(string htmlText, string pageTitle)
        {
            var renderer = new StandardPdfRenderer();
            byte[] buffer = renderer.Render(htmlText, pageTitle);

            // Return the PDF as a binary stream to the client.
            return new BinaryContentResult(buffer, "application/pdf");
        }
    }
}