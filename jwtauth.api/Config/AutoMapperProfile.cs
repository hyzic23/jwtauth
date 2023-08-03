using System;
using AutoMapper;
using jwtauth.api.Dtos;
using jwtauth.models;

namespace jwtauth.api.Config
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<UserInfo, UserInfoDto>().ReverseMap();
		}
	}
}

