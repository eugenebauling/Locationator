using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Locationator.Enums;

namespace Locationator.DAL
{
    public static class PositionRepo
    {
        public static IPositionRepo Instance(Context context)
        {
            switch (Constants.DATA_ACCESS_MODE)
            {
                case DataAccessMode.WebService:
                    return GetRepo<WebService>(context);
                default:
                    return GetRepo<NoDal>(context); ;
            }

        }

        private static IPositionRepo repo;
        private static IPositionRepo GetRepo<T>(Context context)
        {
            if (repo == null)
                repo = (IPositionRepo)Activator.CreateInstance(typeof(T), new object[] { context });

            return repo;
        }

    }
}