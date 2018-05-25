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
    public abstract class BaseRepo<IRepo>
    {
        public abstract IRepo Instance(Context context);

        private IRepo repo;
        public IRepo GetRepo<RepoType>(Context context)
        {
            if (repo == null)
                repo = (IRepo)Activator.CreateInstance(typeof(RepoType), new object[] { context });

            return repo;
        }
    }
}