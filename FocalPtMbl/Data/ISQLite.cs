using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FocalPoint.Data
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
        SQLiteAsyncConnection GetAsyncConnection();
    }
}
