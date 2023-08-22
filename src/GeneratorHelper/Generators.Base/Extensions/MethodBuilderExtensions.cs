using CodeGenHelpers;

namespace Generators.Base.Extensions
{
    public static class MethodBuilderExtensions
    {
        public static MethodBuilder WithReturnTypeTask(this MethodBuilder methodBuilder, string returnType = null)
        {
            if (returnType is null)
            {
                return methodBuilder.WithReturnType("Task");
            }

            return methodBuilder.WithReturnType($"Task<{returnType}>");
        }
        public static MethodBuilder WithReturnTypeTaskList(this MethodBuilder methodBuilder, string returnType)
        {
            return methodBuilder.WithReturnTypeTask($"List<{returnType}>");
        }
    }
}
