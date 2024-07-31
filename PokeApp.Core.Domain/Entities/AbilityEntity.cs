namespace PokeApp.Core.Domain.Entities
{
    public class AbilitiesEntity
    {
        public Ability ability { get; set; }
        public bool is_hidden { get; set; }
        public int slot { get; set; }
    }

    public class Ability
    {
        public string name { get; set; }
        public string url { get; set; }
    }
}
