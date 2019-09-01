using AutoMapper;
namespace MedHelper.Services.Models.Mapping
{
	using Admin.BindingModels;
	using Admin.ViewModels;
	using Data.Models;
	using Doctor.ComboModels;
	using Doctor.ViewModels;
	using Identity.BindingModel;
	using Pages.ViewModel;
	using Personnel.ComboModels;
	using Personnel.ViewModels;
	using User.ViewModels;
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			SetFacilityMapping( );
			SetQualificationMapping( );
			SetUserMappings( );
			SetVisitsMapping( );
			SetExamMappings( );
			SetPagesMappings( );
		}

		private void SetPagesMappings() => CreateMap<News, NewsCardViewModel>( );

		private void SetExamMappings()
		{
			CreateMap<ExamBindingModel, Exam>( )
				.ForMember(e => e.IssuedOn, o => o.Ignore( ))
				.ForMember(e => e.PatientId, o => o.MapFrom(m => m.PatientId));
			CreateMap<Visit, ExamBindingModel>( )
				.ForMember(em => em.PatientName, o => o.MapFrom(v => v.Patient.FullName))
				.ForMember(em => em.PatientId, o => o.MapFrom(v => v.PatientId))
				.ForMember(em => em.Facilities, o => o.Ignore( ));
			CreateMap<Exam, DoctorExamConciseViewModel>( )
				.ForMember(ec => ec.Date, o => o.MapFrom(e => e.IssuedOn))
				.ForMember(ec => ec.Status, o => o.MapFrom(e => e.Status.Status))
				.ForMember(ec => ec.FacilityName, o => o.MapFrom(e => e.Facility.Name));
			CreateMap<Exam, PatientExamDetailsViewModel>( )
				.ForMember(pe => pe.Status, o => o.MapFrom(v => v.Status.Status))
				.ForMember(pe => pe.Name, o => o.MapFrom(v => v.Patient.FullName))
				.ForMember(pe => pe.Date, o => o.MapFrom(v => v.ResultsOn));
			CreateMap<Exam, PersonnelExamConciseViewModel>( )
				.ForMember(pc => pc.Status, o => o.MapFrom(e => e.Status.Status))
				.ForMember(pc => pc.Date, o => o.MapFrom(e => e.IssuedOn))
				.ForMember(pc => pc.Name, o => o.MapFrom(e => e.Patient.FullName));
			CreateMap<Exam, PublishExamModel>( )
				.ForMember(pm => pm.Name, o => o.MapFrom(e => e.Patient.FullName));
		}
		private void SetUserMappings()
		{
			CreateMap<UserDetailsBindingModels, User>( )
				.ForMember(u => u.UserName, o => o.Ignore( ));
			CreateMap<UserRegisterBidingModel, User>( );
			CreateMap<UserExternalRegisterBindingModel, User>( );
			CreateMap<User, UserDetailsBindingModels>( );
			CreateMap<User, UserConciseViewModel>( )
				.ForMember(uc => uc.IsAdmin, o => o.Ignore( ))
				.ForMember(uc => uc.IsDoctor, o => o.Ignore( ))
				.ForMember(uc => uc.IsPersonnel, o => o.Ignore( ))
				.ForMember(uc => uc.Qualification, o => o.MapFrom(u => u.Qualification.Name))
				.ForMember(uc => uc.Is2FAEnabled, o => o.MapFrom(u => u.TwoFactorEnabled));
			CreateMap<User, UserDetailsViewModel>( )
				.ForMember(ud => ud.Qualification, o => o.MapFrom(u => u.Qualification.Name))
				.ForMember(ud => ud.Username, o => o.MapFrom(u => u.UserName));
			CreateMap<User, FacilityPersonnelViewModel>( )
				.ForMember(fc => fc.Name, o => o.MapFrom(u => u.FullName))
				.ForMember(fc => fc.FacilityId, o => o.Ignore( ));
			CreateMap<User, QualificationPersonnelViewModel>( )
				.ForMember(fc => fc.Name, o => o.MapFrom(u => u.FullName))
				.ForMember(fc => fc.QualificationId, o => o.Ignore( ));
			CreateMap<User, PersonnelConciseViewModel>( )
				.ForMember(pc => pc.Qualification, o => o.MapFrom(u => u.Qualification.Name))
				.ForMember(pc => pc.Name, o => o.MapFrom(u => u.FullName));
		}
		private void SetQualificationMapping()
		{
			CreateMap<QualificationCreateBindingModel, Qualification>( );
			CreateMap<Qualification, QualificationConciseViewModel>( )
				.ForMember(qc => qc.Doctors, o => o.Ignore( ));
		}
		private void SetFacilityMapping()
		{
			CreateMap<FacilityCreateBindingModel, Facility>( );
			CreateMap<Facility, FacilityConciseViewModel>( )
				.ForMember(fc => fc.Personnel, o => o.MapFrom(f => f.Operators.Count));
		}
		private void SetVisitsMapping()
		{
			CreateMap<Visit, UserVisitConciseViewModel>( )
				.ForMember(pv => pv.Time, o => o.MapFrom(v => v.StartTime))
				.ForMember(pv => pv.DoctorName, o => o.MapFrom(v => v.TimeTable.User.FullName));
			CreateMap<Visit, DoctorVisitConciseViewModel>( )
				.ForMember(dv => dv.DateAndTime, o => o.MapFrom(v => v.StartTime))
				.ForMember(dv => dv.PatientName, o => o.MapFrom(v => v.Patient.FullName));
			CreateMap<Visit, UserExaminationModel>( )
				.ForMember(ue => ue.PatientName, o => o.MapFrom(v => v.Patient.FullName))
				.ForMember(ue => ue.PatientId, o => o.MapFrom(v => v.PatientId))
				.ForMember(ue => ue.VisitId, o => o.MapFrom(v => v.Id));
			CreateMap<Visit, PatientVisitConciseViewModel>( )
				.ForMember(pv => pv.Date, o => o.MapFrom(v => v.StartTime))
				.ForMember(pv => pv.Doctor, o => o.MapFrom(v => v.TimeTable.User.UserName))
				.ForMember(pv => pv.Status, o => o.MapFrom(v => v.Status.Status));
			CreateMap<Visit, PatientVisitDetailsViewModel>( )
				.ForMember(pd => pd.Status, o => o.MapFrom(v => v.Status.Status))
				.ForMember(pd => pd.Name, o => o.MapFrom(v => v.Patient.FullName))
				.ForMember(pd => pd.Date, o => o.MapFrom(v => v.StartTime));

		}
	}
}
