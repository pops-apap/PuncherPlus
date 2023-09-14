namespace PuncherPlus.Models
{
    public class tablaDpointer
    {
        public long Id { get; set; }

        public DateTime? CreateAt { get; set; }

        public string Motivo { get; set; } = null!;

        public int IdUser { get; set; }

        public string? pointerType { get; set; }
        public string Nick { get; set; } = null!;
        public string latepunch { get; set; } = null!;
    }
}
