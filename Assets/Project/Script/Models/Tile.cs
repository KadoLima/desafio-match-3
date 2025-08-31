namespace Gazeus.DesafioMatch3.Models
{
    public enum SpecialType
    {
        NONE,
        CLEAR_LINE
    }

    public class Tile
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public SpecialType SpecialType { get; set; }
    }
}
