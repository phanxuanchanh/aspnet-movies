using Data.DTO;

namespace Data.Validators
{
    public class CreateUserDtoValidator : Validator, IValidator<CreateUserDto>
    {
        public void Validate(CreateUserDto input)
        {
            if(input == null)
                AddError("@'input' must not be null");

            if (string.IsNullOrWhiteSpace(input.UserName))
                AddError("Username is required");

            if (string.IsNullOrWhiteSpace(input.Email))
                AddError("Email is required");

            if (string.IsNullOrWhiteSpace(input.Password))
                AddError("Password is required");
        }
    }
}
