using PokeApp.Core.Application.Interfaces.Repositories;
using PokeApp.Core.Application.Responses;
using PokeApp.Core.Domain.Entities;
using System.Net.Http.Json;

namespace Presentation.Infrastructure.Repositories
{
    public class PokemonRepository : IPokemonRepository
    {

        public async Task<PokemonResponse> GetHiddenAbilitiesAsync(string PokemonName, string endpoint)
        {
            List<string> hiddeoAbilities = new List<string>();
            PokemonResponse response;
            //throw new Exception("prueba");

            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetFromJsonAsync<PokemonEntity>($"{endpoint}/{PokemonName}");

                if (result != null)
                {
                    //Obtenemos las habilidsades ocultas.
                    var hidden = result.abilities.Where(e => e.is_hidden == true).ToList();

                    if (hidden != null && hidden.Any())
                    {
                        foreach (var hiddenAbility in hidden)
                        {
                            var res = await client.GetFromJsonAsync<HiddenAbilityEntity>(hiddenAbility.ability.url);
                            if (res != null)
                            {
                                res.names.RemoveAll(e => e.language.name != "es");
                                hiddeoAbilities.Add(res.names.FirstOrDefault().name);
                            }
                        };
                    }
                }

                response = new PokemonResponse()
                {
                    name = result.name,
                    ocultas = hiddeoAbilities
                };
            }
            return response;
        }
    }
}
