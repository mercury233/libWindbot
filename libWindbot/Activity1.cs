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

namespace libWindbot
{
    [Activity(Label = "Activity1"),
        Register("libWindbot.Activity1")]
    public class Activity1 : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Toast.MakeText(this, "咕咕咕", ToastLength.Short).Show();
            // Create your application here
        }
    }
}