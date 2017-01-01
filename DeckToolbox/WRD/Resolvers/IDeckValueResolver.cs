using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DeckToolbox.WRD.Resolvers
{
    public interface IDeckValueResolver
    {
        string GetFactionName(int factionId);
        string GetCountryName(int factionId, int countryDeckId);
        string GetCoalitionName(int coalitionDeckId);
        string GetFactoryName(int factory);
        string GetDeckSpecializationName(int unitTypeId);
        string GetDeckEraName(int categoryId);
    }
}
