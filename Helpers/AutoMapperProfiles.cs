using System;
using AutoMapper;
using Night_Shadow.Models;
using Okta.Sdk;

namespace Night_Shadow.Helpers
{
	public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles()
		{
			CreateMap<IUserProfile,ClientProfile>().ReverseMap();
		}

	}
}