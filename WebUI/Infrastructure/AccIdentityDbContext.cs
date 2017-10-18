using System.Data.Entity;
using WebUI.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using WebUI.Exceptions;
using System;
using NLog;
using System.Text;

namespace WebUI.Infrastructure
{
    public class AccIdentityDbContext:IdentityDbContext<AccUserModel>
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public AccIdentityDbContext() : base("accounting_identity") { }

        static AccIdentityDbContext()
        {
            System.Data.Entity.Database.SetInitializer<AccIdentityDbContext>(new MigrateDatabaseToLatestVersion<AccIdentityDbContext,Migrations.Configuration>());
        }

        public static AccIdentityDbContext Create()
        {
            try
            {
                return new AccIdentityDbContext();
            }
            catch (Exception ex)
            {
                var message = new StringBuilder();
                message.AppendLine("\r\n");
                message.AppendLine("Ошибка подключения к базе авторизации");
                message.AppendLine(ex.Message);
                message.AppendLine(ex.InnerException?.Message);
                message.AppendLine(ex.StackTrace);
                _logger.Error(message.ToString());
                throw new WebUiException("Ошибка подключения к базе авторизации", ex);
            }
            
        }
    }

}