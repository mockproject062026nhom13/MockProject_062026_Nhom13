using FluentValidation;
using NursingHome.Application.Features.CarePlans.Commands;

namespace NursingHome.Application.Features.CarePlans.Validators;

public class UpdateCarePlanCommandValidator : AbstractValidator<UpdateCarePlanCommand>
{
    public UpdateCarePlanCommandValidator()
    {
        RuleFor(x => x.CareAreas)
            .NotEmpty().WithMessage("Cần ít nhất 1 care area.");

        RuleForEach(x => x.CareAreas).ChildRules(area =>
        {
            area.RuleForEach(a => a.Interventions!).ChildRules(iv =>
            {
                iv.RuleFor(i => i.AssignedRole)
                    .NotEmpty().WithMessage("assignedRole không được để trống.")
                    .MaximumLength(50);
            }).When(a => a.Interventions is not null);
        });
    }
}
