using System.Collections.Generic;

namespace PokeApp.Core.Domain.Entities
{
    public class PokemonEntity
    {
        public List<AbilitiesEntity> abilities { get; set; }

        public string name { get; set; }
    }
}
