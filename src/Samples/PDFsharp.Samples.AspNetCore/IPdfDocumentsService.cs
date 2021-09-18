using System.Threading.Tasks;
using PdfSharp;

namespace PDFsharp.Samples.AspNetCore
{
    public interface IPdfDocumentsService
    {
        Task<byte[]> FillFormFields(System.Collections.Generic.IReadOnlyDictionary<string, dynamic> data, string filePath, int x = 0, int y = 0, int width = 0, int height = 0, int x2 = 0, int y2 = 0, int width2 = 0, int height2 = 0);
        Task<byte[]> GeneratePdfFromHTML(string htmlString);
        Task<byte[]> GeneratePdfFromHTML(string htmlString, PageOrientation orientation, PageSize pageSize);
        Task<byte[]> GeneratePdfFromHTML(string htmlString, PageOrientation orientation, PageSize pageSize,
            int marginBottom, int marginRight, int marginLeft, int marginTop);

    }
}