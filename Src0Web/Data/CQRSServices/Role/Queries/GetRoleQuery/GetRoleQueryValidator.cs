using Data.DTO;
using Data.Validators;

namespace Data.CQRSServices.Role.Queries.GetRoleQuery
{
    public class GetRoleQueryValidator : Validator, IValidator<RoleDto>
    {
        public void Validate(RoleDto dto)
        {
            throw new System.NotImplementedException();
        }
    }
}