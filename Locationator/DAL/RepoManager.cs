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

namespace Locationator.DAL
{
    public static class RepoManager
    {
        private static PositionRepo positionRepo;
        public static PositionRepo GetPositionRepo()
        {
            if (positionRepo == null)
                positionRepo = new PositionRepo();
            return positionRepo;
        }
    }
}