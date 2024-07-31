using System.Collections.Generic;

namespace PokeApp.Core.Application.Responses
{
    public class PokemonResponse
    {
        public string name { get; set; }

        public List<string> ocultas { get; set; }
    }
}
