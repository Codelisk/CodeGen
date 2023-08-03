using Attributes.WebAttributes.Dto;

namespace Foundation.Dtos.Base
{
    public class BaseDto
    {
        [Id]
        public Guid Id { get; set; }
    }
}
