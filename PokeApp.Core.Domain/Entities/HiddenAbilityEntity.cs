using System.Collections.Generic;

namespace PokeApp.Core.Domain.Entities
{
    public class HiddenAbilityEntity
    {
        public List<NamesHiddenAbilityEntity> names { get; set; }
    }

    public class NamesHiddenAbilityEntity
    {
        public Language language { get; set; }
        public string name { get; set; }
    }

    public class NamesAbilities
    {
        public string name { get; set; }
    }

    public class Language
    {
        public string name { get; set; }
    }

}
