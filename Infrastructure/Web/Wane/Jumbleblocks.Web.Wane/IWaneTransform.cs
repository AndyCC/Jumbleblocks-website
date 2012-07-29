using System.Web;

namespace Jumbleblocks.Web.Wane
{
    /// <summary>
    /// Interface for Wane transformation to HTML
    /// </summary>
    public interface IWaneTransform
    {
        /// <summary>
        /// Transforms Wane to html
        /// </summary>
        /// <param name="waneText">text to transform to HTML</param>
        /// <param name="shouldHtmlEncode">determines if waneText should be HTML encoded before the transformation to HTML occurs</param>
        /// <returns>IHtmlString</returns>
        IHtmlString TransformToHtml(string waneText, bool shouldHtmlEncode = true);

        /// <summary>
        /// Transforms Wane to html
        /// </summary>
        /// <param name="waneText">text to transform to HTML</param>
        /// <param name="shouldHtmlEncode">determines if waneText should be HTML encoded before the transformation to HTML occurs</param>
        /// <returns>String</returns>
        string TransformToRawHtml(string waneText, bool shouldHtmlEncode = true);
    }
}
