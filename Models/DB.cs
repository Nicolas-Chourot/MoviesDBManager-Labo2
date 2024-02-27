using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;

namespace MoviesDBManager.Models
{
    public class DB
    {
        private static readonly DB instance = new DB();
        public static MoviesRepository Movies { get; set; }
        public static ActorsRepository Actors { get; set; }
        public static CastingsRepository Castings { get; set; }
        public DB()
        {
            Movies = new MoviesRepository();
            Actors = new ActorsRepository();
            Castings = new CastingsRepository();
            InitRepositories(this);
        }
        public static DB Instance
        {
            get { return instance; }
        }
        private static void InitRepositories(DB db)
        {
            string serverPath = HostingEnvironment.MapPath(@"~/App_Data/");
            PropertyInfo[] myPropertyInfo = db.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in myPropertyInfo)
            {
                if (propertyInfo.PropertyType.Name.Contains("Repository"))
                {
                    MethodInfo method = propertyInfo.PropertyType.GetMethod("Init");
                    method.Invoke(propertyInfo.GetValue(db), new object[] { serverPath + propertyInfo.Name + ".json" });
                }
            }
        }
    }
}