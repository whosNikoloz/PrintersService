namespace PrintersService.Model
{
    public class PrinterInfo
    {
        public string Name { get; set; }
        public string PortName { get; set; }
        public string DriverName { get; set; }
        public string PrinterStatus { get; set; }
        public bool Default { get; set; }
        public string DeviceID { get; set; }
        public string ShareName { get; set; }
    }
}
