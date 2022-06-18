using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FocalPoint.Data.DataModel;
using SQLite;
using Xamarin.Forms;

namespace FocalPoint.Data
{
    public class DataManager
    {
        static object locker = new object();

        /// <summary>
        /// If there are upgrades to this table, do them before the first save
        /// </summary>
        public static bool FirstSave = true;

        public static string DatabaseFilename = "FocalPtSQL.db3";

        public static SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static SQLiteConnection SQLCon;

        public DataManager()
        {
            SQLCon = GetConnection();
            EnsureTablesExist();
        }

        public SQLite.SQLiteConnection GetConnection()
        {
            SQLiteConnection sqlitConnection;
            sqlitConnection = new SQLite.SQLiteConnection(DatabasePath);
            return sqlitConnection;
        }

        public enum CacheType
        {
            DataCache = 1,
            Customers = 2,
            Rentals = 4,
            Stores = 8,
            SubGroups = 16,
            PostCodes = 32,
            Vendors = 64,
            All = 0xFF,
        }

        public static Settings Settings;
        public static IEnumerable<CacheHistory> DataCaches;
        public static IEnumerable<Customer> Customers;
        public static IEnumerable<Vendor> Vendors;


        // For ease of use, due to the ListViewRenderer.
        public static IEnumerable<Customer> FilteredCustomerData;
        public static IEnumerable<Rental> FilteredRentalData;
        public static IEnumerable<Vendor> FilteredVendorData;

        public static IEnumerable<Rental> Rentals;
        public static IEnumerable<Store> Stores;
        public static IEnumerable<SubGroup> SubGroups;

        private static IEnumerable<PostCode> _postCodes = null;
        public static IEnumerable<PostCode> PostCodes
        {
            get
            {
                if (_postCodes == null || _postCodes.Count() == 0)
                {
                    _postCodes = SQLCon.Query<PostCode>("SELECT * FROM tblPostCode");
                }
                return _postCodes;
            }
            set
            {
                _postCodes = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsPostalCodeDataAvialable()
        {
            return _postCodes != null;
        }

        //private static SQLiteConnection _SQLCon;
        //public static SQLiteConnection SQLCon
        //{
        //    get
        //    {
        //        if (_SQLCon == null)
        //        {
        //            _SQLCon = new SQLiteConnection(DatabasePath);
        //        }
        //        return _SQLCon;
        //    }
        //    set
        //    {
        //        _SQLCon = value;
        //    }
        //}

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }

        public static void CreateDBTables()
        {
            SQLCon.CreateTable<CacheHistory>();
            SQLCon.CreateTable<Customer>();
            SQLCon.CreateTable<PostCode>();
            SQLCon.CreateTable<Rental>();
            SQLCon.CreateTable<Security>();
            SQLCon.CreateTable<Settings>();
            SQLCon.CreateTable<SubGroup>();
            SQLCon.CreateTable<Store>();
            SQLCon.CreateTable<Vendor>();
        }
        public static void ResetSecurityTable()
        {
            SQLCon.DropTable<FocalPoint.Data.DataModel.Security>();
            SQLCon.CreateTable<FocalPoint.Data.DataModel.Security>();
        }
        public static int InsertAllSecurities(List<FocalPoint.Data.DataModel.Security> securityAreas)
        {
            return DataManager.SQLCon.InsertAll(securityAreas);
        }
        public static bool UserIsAllowed(Visum.Services.Mobile.Entities.Security.Areas Area)
        {
            var rtn = SQLCon.Query<Security>("SELECT * FROM tblSecurity WHERE SecArea = ?", Area);

            return (rtn == null || !rtn.Any()) ? false : rtn[0].Allowed;
        }

        // Cheesey...
        private static Settings GetSettings()
        {
            Settings settings = null;

            // Cheesey...
            try
            {
                lock (locker)
                {
                    settings = SQLCon.Query<Settings>("SELECT * FROM tblSettings").FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                //FPConsole.WriteLine("GetSettings exception. Message: " + e.Message);
            }

            return (settings == null) ? new Settings() : settings;
        }

        public static string GetUserToken()
        {
            return GetSettings().UserToken;
        }

        public static void SaveStores(List<Store> stores)
        {
            //In the future, we should record a schema version
            lock (locker)
            {
                //This ensures that the subsequent call to insert will succeed with any new fields
                SQLCon.BeginTransaction();
                SQLCon.Execute("DROP TABLE tblCompany");
                SQLCon.CreateTable<Store>();
                foreach (Store store in stores)
                {
                    SQLCon.Insert(store, typeof(Store));
                }
                SQLCon.Commit();
            }
        }

        public static List<Store> LoadStores()
        {
            List<Store> stores = null;

            try
            {
                lock (locker)
                {
                    stores = SQLCon.Query<Store>("SELECT * FROM tblCompany").ToList();
                }
            }
            catch (Exception e)
            {
                //FPConsole.WriteLine("GetSettings exception. Message: " + e.Message);
            }

            return (stores == null) ? new List<Store>() : stores;
        }

        public static void LoadSettings(string appVersion)
        {
            Settings = GetSettings();

            Settings.AppVersion = appVersion;

            //Settings.LicenseCode = "lgCAAGsmccqP/s8BdgBOYW1lPUR1YW5lIEVybGFuZHNvbiNDb21wYW55PVZpc3VtLCBMTEMjRW1haWw9ZHVhbmVAdmlzdW0tY29ycC5jb20jU3Vic2NyaXB0aW9uPVRydWUjU3Vic2NyaXB0aW9uVmFsaWRUaWxsPTEyLU5vdi0yMDE1AQd03j1cLVAyytxTfKcN3IcaIfzW2Y7YU7cLOmvqV0JL9t04rSQofAJCbiyVOZ4WjVQ=";
            Settings.LicenseCode = "";
        }

        public static void SaveSettings()
        {
            //In the future, we should record a schema version
            if (FirstSave)
            {
                lock (locker)
                {
                    //This ensures that the subsequent call to insert will succeed with any new fields
                    SQLCon.BeginTransaction();
                    SQLCon.Execute("DROP TABLE tblSettings");
                    SQLCon.CreateTable<Settings>();
                    SQLCon.Insert(DataManager.Settings, typeof(Settings));
                    SQLCon.Commit();
                }
                FirstSave = false;
            }
            else
            {
                SQLCon.InsertOrReplace(DataManager.Settings, typeof(Settings));
            }

            // Uupdate the HttpClientCache properties
            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            httpClientCache.BaseUrl = Settings.ApiUri; // base URL
            httpClientCache.Store = Settings.HomeStore.ToString();
            httpClientCache.Terminal = Settings.Terminal.ToString();
            httpClientCache.Token = Settings.UserToken;
            httpClientCache.User = Settings.User;
        }

        public static void ClearSettings()
        {
            Settings = new Settings();
            SaveSettings();
        }

        public static void LoadHttpClientCache()
        {
            // Update the HttpClientCache properties
            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            httpClientCache.BaseUrl = Settings.ApiUri; // base URL
            httpClientCache.Store = Settings.HomeStore.ToString();
            httpClientCache.Terminal = Settings.Terminal.ToString();
            httpClientCache.Token = Settings.UserToken;
            httpClientCache.User = Settings.User;

        }

        public static void LoadDataCache()
        {
            try
            {
                if (DataCaches == null)
                    DataCaches = SQLCon.Query<CacheHistory>("SELECT * FROM tblCacheHistory");
                if (Stores == null)
                    Stores = SQLCon.Query<Store>("SELECT * FROM tblCompany ORDER BY CmpName");
                if (PostCodes == null)
                    PostCodes = SQLCon.Query<PostCode>("SELECT * FROM tblPostCode ORDER BY PostCodeDscr");
            }
            catch (Exception e)
            {
                //FPConsole.WriteLine("GetSettings exception. Message: " + e.Message);
            }
        }

        public static void LoadDataCache(CacheType cacheType)
        {
            if ((cacheType & CacheType.DataCache) != 0)
                DataCaches = SQLCon.Query<CacheHistory>("SELECT * FROM tblCacheHistory");
            if ((cacheType & CacheType.Stores) != 0)
                Stores = SQLCon.Query<Store>("SELECT * FROM tblCompany ORDER BY CmpName");
            if ((cacheType & CacheType.PostCodes) != 0)
                PostCodes = SQLCon.Query<PostCode>("SELECT * FROM tblPostCode ORDER BY PostCodeDscr");

        }

        public static void EnsureTablesExist()
        {
            if (!TableExists<CacheHistory>())
                SQLCon.CreateTable<CacheHistory>();

            if (!TableExists<Customer>())
                SQLCon.CreateTable<Customer>();

            if (!TableExists<PostCode>())
                SQLCon.CreateTable<PostCode>();

            if (!TableExists<Rental>())
                SQLCon.CreateTable<Rental>();

            if (!TableExists<Security>())
                SQLCon.CreateTable<Security>();

            if (!TableExists<Settings>())
                SQLCon.CreateTable<Settings>();

            if (!TableExists<SubGroup>())
                SQLCon.CreateTable<SubGroup>();

            if (!TableExists<Store>())
                SQLCon.CreateTable<Store>();

            if (!TableExists<Vendor>())
                SQLCon.CreateTable<Vendor>();
        }

        public static void EnsureTableExists<T>()
        {
            if (!TableExists<T>())
                SQLCon.CreateTable<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static bool TableExists<T>()
        {
            string qText = string.Format("SELECT name FROM sqlite_master WHERE type='table' AND name='tbl{0}'", typeof(T).Name);
            var result = SQLCon.ExecuteScalar<string>(qText);
            return result != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<T> Query<T>(string sql) where T : new()
        {
            return SQLCon.Query<T>(sql);
        }
    }
}
