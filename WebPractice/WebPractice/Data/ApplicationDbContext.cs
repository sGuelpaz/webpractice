using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPractice.Models;

namespace WebPractice.Data
{
    public class ApplicationDbContext:IdentityDbContext
    {
        //Constructor se crea con ctor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option):base(option)
        {

        }

        //Agregamos todos los modelos que luego seran tablas
        public DbSet<Registros> Registro { get; set; }

        public DbSet<AppUsuario> AppUsuario { get; set; }
    }
}
