using System;

namespace aes.Models
{
    public class StanUpdate
    {
        public int Id { get; set; }
        public DateTime DateOfData { get; set; }
        public DateTime UpdateBegan { get; set; }
        public DateTime? UpdateEnded { get; set; }
        public string ExecutedBy { get; set; }
        public bool UpdateComplete { get; set; }
        public bool Interrupted { get; set; }
    }
}
