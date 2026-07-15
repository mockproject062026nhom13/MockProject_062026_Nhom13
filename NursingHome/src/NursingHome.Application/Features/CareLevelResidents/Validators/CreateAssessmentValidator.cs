using FluentValidation;
using NursingHome.Application.Features.CareLevelResidents.Commands;

namespace NursingHome.Application.Features.CareLevelResidents.Validators;

public class CreateAssessmentValidator : AbstractValidator<CreateAssessmentCommand>
{
    public CreateAssessmentValidator()
    {
        //
    }
}