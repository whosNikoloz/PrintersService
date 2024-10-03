using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrintersService.Model;
using System.Management;

namespace PrintersService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrinterController : ControllerBase
    {

        [HttpGet("GetPrinterDetails")]
        public IActionResult GetPrinterDetails()
        {
            try
            {
                var printerList = new List<PrinterInfo>();

                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
                var printers = searcher.Get();

                foreach (var printer in printers)
                {
                    try
                    {
                        var printerInfo = new PrinterInfo
                        {
                            Name = printer["Name"]?.ToString() ?? "Unknown",
                            PortName = printer["PortName"]?.ToString() ?? "N/A",
                            DriverName = printer["DriverName"]?.ToString() ?? "N/A",
                            PrinterStatus = printer["PrinterStatus"]?.ToString() ?? "Unknown",
                            Default = (bool)(printer["Default"] ?? false),
                            DeviceID = printer["DeviceID"]?.ToString() ?? "N/A",
                            ShareName = printer["Shared"]?.ToString() == "True" ? printer["ShareName"]?.ToString() : null
                        };

                        printerList.Add(printerInfo);
                    }
                    catch (Exception innerEx)
                    {
                        printerList.Add(new PrinterInfo
                        {
                            Name = "Error retrieving printer",
                            DeviceID = printer["DeviceID"]?.ToString() ?? "N/A",
                            PrinterStatus = $"Error: {innerEx.Message}"
                        });
                    }
                }

                return Ok(printerList);
            }
            catch (ManagementException mgmtEx)
            {
                return StatusCode(500, $"WMI error: {mgmtEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
