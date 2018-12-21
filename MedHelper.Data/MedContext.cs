using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace MedHelper.Data
{
	using Common.Constants;
	using Models;

	public class MedContext : IdentityDbContext<User>
	{
#if DEBUG
		private const string CONNECTION_STRING = "Server=localhost;Initial Catalog=MedHelperDb_Debug;Integrated Security=True;";
#else
		private const string CONNECTION_STRING = "Server=tcp:envelopeddevil.database.windows.net,1433;Initial Catalog=MedHelperDb;Persist Security Info=False;User ID=TestUser2;Password=Password_Test123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
#endif


		public DbSet<Facility> Facilities { get; set; }
		public DbSet<Qualification> Qualification { get; set; }
		public DbSet<Exam> Exams { get; set; }
		public DbSet<TimeTable> TimeTables { get; set; }
		public DbSet<Visit> Visits { get; set; }
		public DbSet<VisitStatus> VisitStatuses { get; set; }
		public DbSet<ExamStatus> ExamStatuses { get; set; }
		public DbSet<News> News { get; set; }
		public MedContext(DbContextOptions<MedContext> options) : base(options)
		{
		}
		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			if (!options.IsConfigured) options.UseSqlServer(CONNECTION_STRING);
			base.OnConfiguring(options);
		}
		public MedContext() : base(new DbContextOptionsBuilder().UseSqlServer(CONNECTION_STRING).Options)
		{

		}
		protected override void OnModelCreating(ModelBuilder model)
		{
			Constraints(model);
			Relations(model);

			base.OnModelCreating(model);
		}

		private static void Relations(ModelBuilder model)
		{
			model.Entity<Qualification>().HasMany(q => q.Users).WithOne(u => u.Qualification).HasForeignKey(uq => uq.QualificationId);

			model.Entity<Facility>().HasMany(f => f.Operators).WithOne(u => u.Facility).HasForeignKey(u => u.FacilityId);
			model.Entity<Facility>().HasMany(f => f.Exams).WithOne(e => e.Facility).HasForeignKey(e => e.FacilityId);

			model.Entity<User>().HasMany(u => u.Visits).WithOne(v => v.Patient).HasForeignKey(vo => vo.PatientId);
			model.Entity<User>().HasMany(u => u.Exams).WithOne(e => e.Patient).HasForeignKey(e => e.PatientId);
			model.Entity<User>().HasOne(u => u.TimeTable).WithOne(tt => tt.User);

			model.Entity<TimeTable>().HasMany(tt => tt.Visits).WithOne(v => v.TimeTable).HasForeignKey(v => v.TimeTableId);

			model.Entity<VisitStatus>().HasMany(vs => vs.Visits).WithOne(v => v.Status).HasForeignKey(v => v.StatusId);

			model.Entity<ExamStatus>().HasMany(es => es.Exams).WithOne(e => e.Status).HasForeignKey(e => e.StatusId);

			model.Entity<Visit>().HasOne(v => v.Exam).WithOne(e => e.Visit);

			model.Entity<Exam>().HasOne(e => e.Visit).WithOne(v => v.Exam);
		}

		private static void Constraints(ModelBuilder model)
		{
			model.Entity<User>().Property(u => u.FullName).IsRequired();
			model.Entity<User>().Property(u => u.BirthDate).IsRequired();
			model.Entity<User>().Property(u => u.RegisteredDate).HasDefaultValueSql(SqlValueGeneration.GET_DATE);

			model.Entity<Facility>().Property(f => f.Id).IsRequired().HasDefaultValueSql(SqlValueGeneration.NEW_ID);
			model.Entity<Facility>().Property(f => f.ClosingTime).IsRequired();
			model.Entity<Facility>().Property(f => f.OpeningTime).IsRequired();
			model.Entity<Facility>().Property(f => f.Name).IsRequired();
			model.Entity<Facility>().Property(f => f.IsDeleted).HasDefaultValue(false);

			model.Entity<Qualification>().Property(q => q.Id).IsRequired().HasDefaultValueSql(SqlValueGeneration.NEW_ID);
			model.Entity<Qualification>().Property(q => q.Name).IsRequired();
			model.Entity<Qualification>().Property(q => q.IsDeleted).HasDefaultValue(false);

			model.Entity<Exam>().Property(e => e.Id).IsRequired().HasDefaultValueSql(SqlValueGeneration.NEW_ID);
			model.Entity<Exam>().Property(e => e.IssuedOn).IsRequired().HasDefaultValueSql(SqlValueGeneration.GET_DATE);
			model.Entity<Exam>().Property(e => e.FacilityId).IsRequired();
			model.Entity<Exam>().Property(e => e.PatientId).IsRequired();
			model.Entity<Exam>().Property(e => e.StatusId).IsRequired();
			model.Entity<Exam>().Property(e => e.Note).IsRequired();

			model.Entity<TimeTable>().Property(tt => tt.Id).IsRequired().HasDefaultValueSql(SqlValueGeneration.NEW_ID);
			model.Entity<TimeTable>().Property(tt => tt.UserId).IsRequired();

			model.Entity<Visit>().Property(v => v.Id).IsRequired().HasDefaultValueSql(SqlValueGeneration.NEW_ID);
			model.Entity<Visit>().Property(v => v.StartTime).IsRequired();
			model.Entity<Visit>().Property(v => v.EndTime).IsRequired();
			model.Entity<Visit>().Property(v => v.PatientId).IsRequired();
			model.Entity<Visit>().Property(v => v.TimeTableId).IsRequired();
			model.Entity<Visit>().Property(v => v.StatusId).IsRequired();

			model.Entity<VisitStatus>().Property(vs => vs.Id).IsRequired().HasDefaultValue(SqlValueGeneration.NEW_ID);
			model.Entity<VisitStatus>().Property(vs => vs.Status).IsRequired();

			model.Entity<ExamStatus>().Property(vs => vs.Id).IsRequired().HasDefaultValue(SqlValueGeneration.NEW_ID);
			model.Entity<ExamStatus>().Property(vs => vs.Status).IsRequired();

			model.Entity<News>().Property(n => n.Id).IsRequired().HasDefaultValueSql(SqlValueGeneration.NEW_ID);
			model.Entity<News>().Property(n => n.Date).IsRequired().HasDefaultValueSql(SqlValueGeneration.GET_DATE);
			model.Entity<News>().Property(n => n.Title).IsRequired();
			model.Entity<News>().Property(n => n.Content).IsRequired();
		}
	}
}
