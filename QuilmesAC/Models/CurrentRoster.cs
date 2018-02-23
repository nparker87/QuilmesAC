namespace QuilmesAC.Models
{
    public class CurrentRoster
    {
        public int PlayerID { get; set; }

        public string FullName { get; set; }

        public int? Number { get; set; }

        public int Goals { get; set; }

        public int Assists { get; set; }

        public int YellowCards { get; set; }

        public int RedCards { get; set; }
    }
}