﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ReportRegister.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReportRegister.Areas.Identity.Data
{

    //Class for providing dbContext to EF Migration
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {

        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseSqlServer(File.ReadAllText("connection-string.txt"));//file excluded in gitignore

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
