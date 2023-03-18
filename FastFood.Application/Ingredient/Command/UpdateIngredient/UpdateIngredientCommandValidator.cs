﻿using FluentValidation;

namespace FastFood.Application.Ingredient.Command.UpdateIngredient
{
    public class UpdateIngredientCommandValidator : AbstractValidator<UpdateIngredientCommand>
    {
        public UpdateIngredientCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(45);

            RuleFor(c => c.Description)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(500);

            RuleFor(c => c.Prize)
                .NotEmpty()
                .NotNull()
                .GreaterThanOrEqualTo(1);
        }
    }
}