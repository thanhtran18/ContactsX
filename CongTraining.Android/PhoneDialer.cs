﻿using Android.Content;
using Android.Telephony;
using CongTraining.Droid;
using System.Linq;
using Xamarin.Forms;
using Uri = Android.Net.Uri;

namespace CongTraining.Droid {
    public class PhoneDialer {

        public bool Dial(string number) {
            var context = MainActivity.Instance;
            if (context == null)
                return false;

            var intent = new Intent(Intent.ActionCall);
            intent.SetData(Uri.Parse("tel:" + number));

            if (IsIntentAvailable(context, intent)) {
                context.StartActivity(intent);
                return true;
            }

            return false;
        }

        public static bool IsIntentAvailable(Context context, Intent intent) {
            var packageManager = context.PackageManager;

            var list = packageManager.QueryIntentServices(intent, 0)
                                     .Union(packageManager.QueryIntentServices(intent, 0));

            if (list.Any())
                return true;

            var manager = TelephonyManager.FromContext(context);
            return manager.PhoneType != PhoneType.None;
        }
    } // class
}
