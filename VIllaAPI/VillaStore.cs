using VIllaAPI.Models.Dtos;

namespace VIllaAPI
{
    public static class VillaStore
    {
        public static List<VillaDto> VillaList= new List<VillaDto>
            { new VillaDto { Id=1, Name="TDI", Sqft=100, Occupancy=3},
              new VillaDto { Id=2 , Name="Bombay",Sqft=300,Occupancy=4}         
            };

       
    }
}
