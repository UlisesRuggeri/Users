
using System.ComponentModel.DataAnnotations;

namespace Application.Validation.Attributes;

public class NotWhiteSpaceAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if(value is string str) return !string.IsNullOrEmpty(str);
        
        return false;
    }
}
