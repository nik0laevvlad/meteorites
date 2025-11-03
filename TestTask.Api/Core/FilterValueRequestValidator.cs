using FluentValidation;
using TestTask.Core.Models.Queries;

namespace TestTask.Core;

public class FilterValueRequestValidator : AbstractValidator<FilterValueRequest>
{
    public FilterValueRequestValidator()
    {
        RuleFor(x => x.YearFrom)
            .LessThanOrEqualTo(x => x.YearTo)
            .When(x => x.YearFrom.HasValue && x.YearTo.HasValue);

        RuleFor(x => x.SortOrder)
            .Must(x => x is "asc" or "desc" or null)
            .WithMessage("SortOrder must be 'asc' or 'desc'");
    }
}
