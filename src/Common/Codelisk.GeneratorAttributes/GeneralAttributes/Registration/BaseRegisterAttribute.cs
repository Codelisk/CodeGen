namespace Codelisk.GeneratorAttributes.GeneralAttributes.Registration
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class BaseRegisterAttribute : Attribute
    {
        public Type Interface { get; set; }
        public BaseRegisterAttribute()
        {

        }
        public BaseRegisterAttribute(Type interfaceType)
        {
            Interface = interfaceType;
        }
    }
}
