using System.Collections.Generic;

namespace MineralAirways.Models
{
    public class EnvioEmailViewModel
    {
        public string To { get; set; }
        public string FromEmail { get; set; }
        public string FromNombre { get; set; }
        public string Subject { get; set; }
        public IEnumerable<string[]> AttachmentList { get; set; }
        public string CopyCarbon { get; set; }
        public string BlindCopyCarbon { get; set; }
        public string BodyEmail { get; set; }
    }
}