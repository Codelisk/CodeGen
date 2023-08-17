namespace Codelisk.GeneratorAttributes.WebAttributes.Repository
{
    public class EntityAttribute : BaseEntityAttribute
    {
        public Type Type { get; set; }
        public EntityAttribute(Type type)
        {
            Type = type;
        }
    }
}
