﻿using System;
using FluentValidation;
using jwtauth.api.Dtos;

namespace jwtauth.api.Validators
{
	public class UserInfoDtoValidator : AbstractValidator<UserInfoDto>
	{
		public UserInfoDtoValidator()
		{

            RuleFor(x => x.UserId).NotNull().WithMessage("UserId cannot be null")
            					  .GreaterThan(0).WithMessage("UserId must be greater than 0");

            RuleFor(x => x.DisplayName).NotNull().WithMessage("Display Name cannot be null")
                                       .MinimumLength(5).WithMessage("Display Name cannot be less than 5")
                                       .MaximumLength(50).WithMessage("Display Name cannot be more than 20");

            RuleFor(x => x.UserName).NotNull().WithMessage("Username cannot be null")
                                    .MinimumLength(5).WithMessage("Username cannot be less than 5")
                                    .MaximumLength(50).WithMessage("Username cannot be more than 20");

            RuleFor(x => x.Email).NotNull().WithMessage("Email cannot be null")
                                 .EmailAddress().WithMessage("Enter Valid email");

            RuleFor(x => x.Password).NotNull().WithMessage("Password cannot be null")
                                    .MinimumLength(5).WithMessage("Password cannot be less than 5")
                                    .MaximumLength(20).WithMessage("Password cannot be more than 20");
                



            //RuleSet("all", () =>
            //{
            //             RuleFor(x => x.UserId).Must(CheckId).WithMessage("UserId must be greater than 0");
            //});

            //RuleSet("UserId", () =>
            //{
            //	RuleFor(x => x.UserId).NotNull().WithMessage("UserId cannot be null")
            //						  .GreaterThan(0).WithMessage("UserId must be greater than 0");
            //});

            //         RuleSet("DisplayName", () =>
            //         {
            //             RuleFor(x => x.DisplayName).NotNull().WithMessage("Display Name cannot be null")
            //                                   .MinimumLength(5).WithMessage("Display Name cannot be less than 5")
            //                                   .MaximumLength(20).WithMessage("Display Name cannot be more than 20");
            //         });

            //         RuleSet("UserName", () =>
            //         {
            //             RuleFor(x => x.UserName).NotNull().WithMessage("Username cannot be null")
            //                                   .MinimumLength(5).WithMessage("Username cannot be less than 5")
            //                                   .MaximumLength(20).WithMessage("Username cannot be more than 20");
            //         });

            //         RuleSet("Email", () =>
            //         {
            //             RuleFor(x => x.UserId).NotNull().WithMessage("Email cannot be null")
            //                                   .GreaterThan(0).WithMessage("Email must be greater than 0");
            //         });

            //         RuleSet("Password", () =>
            //         {
            //             RuleFor(x => x.Password).NotNull().WithMessage("Password cannot be null")
            //                                   .MinimumLength(5).WithMessage("Password cannot be less than 5")
            //                                   .MaximumLength(20).WithMessage("Password cannot be more than 20");
            //         });
        }

        //     private bool CheckId(int? id)
        //     {
        //         return !id.HasValue || id.Value > 0;
        //     }
    }
}

