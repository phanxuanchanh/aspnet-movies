namespace Data.Validators
{
    public interface IValidator<TDTO>
    {
        void Validate(TDTO dto);
    }
}