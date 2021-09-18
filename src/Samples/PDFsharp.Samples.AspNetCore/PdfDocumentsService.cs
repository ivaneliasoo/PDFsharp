using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.AcroForms;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlRenderer.PdfSharp;

namespace PDFsharp.Samples.AspNetCore
{
    public class PdfDocumentsService : IPdfDocumentsService
    {
        //private readonly IOptions<StorageOptions> _storageOptions;

        //TODO: En este servicio solo tendremos la logica de creacion, modificacion y cualqeuir actividad relacionada especificamente con los pdfs
        // no debemos tener aqui ninguna dependencias a repositorio, servicios, html, etc

        //public PdfDocumentsService(IOptions<StorageOptions> storageOptions)
        //{
        //    _storageOptions = storageOptions ?? throw new ArgumentNullException(nameof(storageOptions));
        //}

        public async Task<byte[]> FillFormFields(IReadOnlyDictionary<string, dynamic> data, string filePath, int x = 0, int y = 0, int width = 0, int height = 0, int x2 = 0, int y2 = 0, int width2 = 0, int height2 = 0)
        {
            byte[] bytes = null;
            try
            {
                if (File.Exists(filePath))
                {

                    //var nfilePath = "D:/PDFs/" + Guid.NewGuid().ToString() + '_' + Path.GetFileName(filePath);
                    //File.Copy(filePath, nfilePath);

                    string logo = string.Empty;
                    string codigoBarra = string.Empty;

                    //filePath = nfilePath;
                    PdfDocument document = PdfReader.Open(filePath, PdfDocumentOpenMode.Modify);

                    // Get the root object of all interactive form fields
                    PdfAcroForm form = document.AcroForm;

                    // Get all form fields of the whole document
                    PdfAcroField.PdfAcroFieldCollection fields = form.Fields;

                    // Get all form fields of the whole document
                    string[] names = fields.Names;
                    names = fields.DescendantNames;

                    // Fill some value in each field
                    for (int idx = 0; idx < names.Length; idx++)
                    {
                        string fqName = names[idx];
                        PdfAcroField field = fields[fqName];

                        string campo = fqName.Substring(0, fqName.IndexOf('.') > 0 ? fqName.IndexOf('.') : fqName.Length);
                        PdfString test = new PdfString(data[campo].ToString());
                        if (field is PdfTextField txtField)
                        {
                            PdfTextField testField = (PdfTextField)form.Fields[fqName];
                            if (testField.ReadOnly == false)
                            {
                                testField.Value = test;
                                testField.ReadOnly = true;
                            }
                        }
                        else if (field is PdfRadioButtonField radField)
                        {
                            PdfRadioButtonField testRadField = (PdfRadioButtonField)form.Fields[fqName];
                            if (testRadField.ReadOnly == false)
                            {
                                testRadField.Value = test;
                                testRadField.ReadOnly = true;
                            }
                        }

                        else if (field is PdfCheckBoxField chkField)
                        {
                            PdfCheckBoxField testChkField = (PdfCheckBoxField)form.Fields[fqName];
                            if (testChkField.ReadOnly == false)
                            {
                                testChkField.Value = test;
                                testChkField.ReadOnly = true;
                            }
                        }
                        else if (field is PdfListBoxField lbxField)
                        {
                            PdfListBoxField testLblField = (PdfListBoxField)form.Fields[fqName];
                            if (testLblField.ReadOnly == false)
                            {
                                testLblField.Value = test;
                                testLblField.ReadOnly = true;
                            }
                        }
                        else if (field is PdfComboBoxField cbxField)
                        {
                            PdfComboBoxField testCmbField = (PdfComboBoxField)form.Fields[fqName];
                            if (testCmbField.ReadOnly == false)
                            {
                                testCmbField.Value = test;
                                testCmbField.ReadOnly = true;
                            }
                        }
                        else if (field is PdfGenericField genField)
                        {
                            PdfGenericField testGenField = (PdfGenericField)form.Fields[fqName];
                            if (testGenField.ReadOnly == false)
                            {
                                testGenField.Value = test;
                                testGenField.ReadOnly = true;
                            }
                        }

                        // Logo del Cliente
                        if (campo == "Logo")
                            logo = data[campo].ToString();

                        // Logo del Cliente
                        if (campo == "CodigoBarra")
                            codigoBarra = data[campo].ToString();
                    }

                    // TODO-IVAN: tratar logo en este servicio rompe scon su responsabilidad
                    if (logo != "")
                    {
                        try
                        {
                            var pathImagenes = "http://www.google.com";//_storageOptions.Value.RutaImagenes;
                            var httpClient = new HttpClient();
                            var req = await httpClient.GetAsync(pathImagenes + logo.Replace("\\", "//"));
                            req.EnsureSuccessStatusCode();
                            var resul = await req.Content.ReadAsStreamAsync();
                            // Get an XGraphics object for drawing
                            XGraphics gfx = XGraphics.FromPdfPage(document.Pages[0]);
                            XImage image = XImage.FromStream(resul);
                            gfx.DrawImage(image, x, y, width, height);
                            gfx.Dispose();
                            image.Dispose();
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }

                    if (codigoBarra != "")
                    {
                        try
                        {
                            //var codigoBarraImg = new Barcode();
                            //var imagen = codigoBarraImg.Encode(TYPE.CODE128, codigoBarra);
                            // Get an XGraphics object for drawing
                            XGraphics gfx = XGraphics.FromPdfPage(document.Pages[0]);
                            //XImage image = XImage.FromStream(imagen.ToStream());
                            //gfx.DrawImage(image, x2, y2, width2, height2);
                            gfx.Dispose();
                            //image.Dispose();
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }

                    using (MemoryStream result = new MemoryStream())
                    {
                        document.Save(result);
                        bytes = result.ToArray();
                        result.Dispose();
                    }
                    document.Dispose();
                }
                return bytes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<byte[]> GeneratePdfFromHTML(string htmlString)
        {
            var configurationOptions = new PdfGenerateConfig
            {
                PageOrientation = PageOrientation.Landscape,
                PageSize = PageSize.Letter,
                MarginBottom = 40,
                MarginRight = 20,
                MarginLeft = 30,
                MarginTop = 40
            };
            PdfDocument pdf = PdfGenerator.GeneratePdf(htmlString, configurationOptions);
            MemoryStream memoryStream = new MemoryStream();
            pdf.Save(memoryStream, true);
            return Task.FromResult(memoryStream.ToArray());
        }

        public Task<byte[]> GeneratePdfFromHTML(string htmlString, PageOrientation orientation, PageSize pageSize)
        {
            var configurationOptions = new PdfGenerateConfig
            {
                PageOrientation = orientation,
                PageSize = pageSize,
                MarginBottom = 40,
                MarginRight = 20,
                MarginLeft = 30,
                MarginTop = 40
            };
            PdfDocument pdf = PdfGenerator.GeneratePdf(htmlString, configurationOptions);
            MemoryStream memoryStream = new MemoryStream();
            pdf.Save(memoryStream, true);
            return Task.FromResult(memoryStream.ToArray());
        }

        public Task<byte[]> GeneratePdfFromHTML(string htmlString, PageOrientation orientation, PageSize pageSize, int marginBottom, int marginRight, int marginLeft, int marginTop)
        {
            var configurationOptions = new PdfGenerateConfig
            {
                PageOrientation = orientation,
                PageSize = pageSize,
                MarginBottom = marginBottom,
                MarginRight = marginRight,
                MarginLeft = marginLeft,
                MarginTop = marginTop
            };
            PdfDocument pdf = PdfGenerator.GeneratePdf(htmlString, configurationOptions);
            MemoryStream memoryStream = new MemoryStream();
            pdf.Save(memoryStream, true);
            return Task.FromResult(memoryStream.ToArray());
        }
    }
}
