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
    public class PositionRepo : BaseRepo<IPositionRepo>
    {
        public override IPositionRepo Instance(Context context)
        {
            switch (Settings.DataAccessMode)
            {
                case DataAccessMode.WebService:
                    return base.GetRepo<PositionWebService>(context);
                default:
                    return base.GetRepo<NoDal>(context);
            }
        }
    }
}