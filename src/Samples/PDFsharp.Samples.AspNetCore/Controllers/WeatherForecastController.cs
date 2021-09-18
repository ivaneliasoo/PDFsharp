using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PdfSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PDFsharp.Samples.AspNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IPdfDocumentsService _pdfService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IPdfDocumentsService pdfDocumentsService)
        {
            _logger = logger;
            _pdfService = pdfDocumentsService ?? throw new ArgumentNullException(nameof(pdfDocumentsService));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var pdf = await _pdfService.GeneratePdfFromHTML($@"<HTML>
                    <HEAD>
                        <STYLE>
                        .table
                        {{
                            width:100%;
                            max-width:100%;
                            font-size:small;
                            margin-bottom:1rem;
                            background-color:white;
                        }}
                        .table-bordered
                        {{
                         border:1px solid #dee2e6;
                        }}
                        .table-bordered td,.table-bordered th
                        {{
                            border:1px solid #dee2e6;font-size:8pt;
                        }}
                        .table-bordered thead td,.table-bordered thead th
                        {{
                            border-bottom-width:2px;
                        }}
                        thead th
                        {{
                            background-color:beige;
                            font-size:8pt;
                            margin-bottom:10px;

                        }}
                         tbody th
                        {{
                            background-color:gray;
                        }}
                        .textoArriba{{                            
                            margin-left:2px;
                            font-size: 8pt;
                            height:15px;
                            color:black;
                           }}
                         .texto{{
                            margin-left:2px;
                            font-size: 9pt;
                            height:15px;
                            color:black;                          
                           }}
                         .borde{{
                            margin-left:2px;
                            font-size: 7pt;
                            height:15px;
                            color:black;                          
                           }}
                        </STYLE>
                 </HEAD>
                <body>
                    <body><div id='contenidoHtm' runat='server' style='display:block'  >
                    <table class='table' cellspacing='0' cellpadding='0' >
                        <thead>
                            <tr>
                                <td style='width:10%; text-align:left;'><img src='#HOST_NAME#/LogoZadecon.jpg' alt='Zadecon' height='50px' /></td>  
                                <td style='width:90%; text-align:left;'></td>                                       
                            </tr>
                         </thead>
                    </table> 
                    <table class='table' cellspacing='0' cellpadding='0' >
                        <thead>
                            <tr>
                                <td class='texto' style='width:85%; text-align:left;'></td> 
                                <td class='texto' style='width:15%; text-align: left; padding-left:350px;'>{ " ALBARAN NRO.: " }</td>  
                            </tr>
                            <tr>
                                <td class='texto' style='width:85%; text-align:left;'></td>  
                                <td class='texto' style='width:15%; text-align: left; padding-left:350px;'>{ " FECHA: " }</td>            
                            </tr>
                            <tr>
                                <td class='texto' style='width:85%; text-align:left;'></td>   
                                <td class='texto' style='width:15%; text-align: left; padding-left:350px;'>{ " NRO. PEDIDO CLIENTE: "  }</td>
                            </tr>
                            <tr>
                                <td colspan='2' style='padding-top:20px></td>
                            </tr>
                         </thead>
                    </table>  
                    <table class='table' cellspacing='0' cellpadding='0' >
                        <thead>
                            <tr>
                                <td class='texto' style='width:30%; text-align: left'></td>
                                <td style='width:10%; text-align:left;'></td> 
                                <td style='width:10%; text-align:left;'></td> 
                                <td class='texto' style='width:50%; text-align: left; padding-left:100px;'>{ " ENTREGA A: " }</td>
                            </tr>
                            <tr>
                                <td class='texto' style='width:30%; text-align: left'></td>
                                <td style='width:10%; text-align:left;'></td> 
                                <td style='width:10%; text-align:left;'></td> 
                                <td class='texto' style='width:50%; text-align: left; padding-left:100px;'></td>
                            </tr>
                            <tr>
                                <td class='texto' style='width:30%; text-align: left'></td>
                                <td style='width:10%; text-align:left;'></td> 
                                <td style='width:10%; text-align:left;'></td> 
                                <td class='texto' style='width:50%; text-align: left; padding-left:100px;'></td>
                            </tr>
                            <tr>
                                <td class='texto' style='width:30%; text-align: left'></td>
                                <td rowspan='4'style='width:10%; text-align:center;'><img src='#HOST_NAME#/LogoSGS.png' alt='SGS' height='50px' /></td> 
                                <td rowspan='4'style='width:10%; text-align:center;'><img src='#HOST_NAME#/LogoSGS.png' alt='SGS' height='50px' /></td>  
                                <td class='texto' style='width:50%; text-align: left; padding-left:100px;'></td>
                            </tr>
                            <tr>
                                <td class='texto' style='width:30%; text-align: left'></td>
                                <td style='width:10%; text-align:left;'></td> 
                                <td style='width:10%; text-align:left;'></td> 
                                <td class='texto' style='width:50%; text-align: left; padding-left:100px;'></td>
                            </tr>
                            <tr>
                                <td class='texto' style='width:30%; text-align: left'></td>
                                <td style='width:10%; text-align:left;'></td> 
                                <td style='width:10%; text-align:left;'></td> 
                                <td class='texto' style='width:50%; text-align: left; padding-left:100px;'></td>
                            </tr>
                            <tr>
                                <td colspan='4' style='width:10%; text-align: left'><h3>{ " ALBARAN " }</h3></td>
                            </tr>
                         </thead>
                    </table>
                </body></HTML>", PageOrientation.Landscape, PageSize.Letter, 60, 60, 60, 60);

            return File(pdf, "application/pdf");
        }
    }
}
