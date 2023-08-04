using AutoMapper;

namespace NetCore.WebAPI.Models
{
    /// <summary>
    /// base mapping-profile for entites and DTO
    /// </summary>
    /// <typeparam name="TDBContext">DBContext</typeparam>
    public class EntityMappingProfile<TDBContext> : Profile where TDBContext : class
    {
        public EntityMappingProfile()
        {
            var assembly = typeof(TDBContext).Assembly;

            foreach (var prop in typeof(TDBContext).GetProperties())
            {
                if (!prop.PropertyType.Name.Contains("DbSet")) continue;

                var entity = prop.PropertyType.GenericTypeArguments[0].FullName;
                var typeofEntity = assembly.GetType(entity);
                var typeofDTO = assembly.GetType($"{entity}DTO");

                CreateMap(typeofEntity, typeofDTO).ReverseMap();
                CreateMap(typeofEntity, typeofEntity);
                CreateMap(typeofDTO, typeofDTO);
            }
        }
    }
}

