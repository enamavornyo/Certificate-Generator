using System;
using Certificate_Generator.Models;
using Microsoft.EntityFrameworkCore;

namespace Certificate_Generator.Context
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> contextOptions) : base(contextOptions)

		{ }


		public DbSet <Certificate> Certificates { get; set; }
		public DbSet <Template> Templates { get; set; }
		public DbSet<Title> Titles { get; set; }
        public DbSet<Course> Courses { get; set; }

    }
}

