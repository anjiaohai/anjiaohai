using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content;

namespace App1
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {



        Button BtnSen;
        Button BtnServer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            FindViewById<Button>(Resource.Id.button1).Click += MainActivity_Click;//生产
            FindViewById<Button>(Resource.Id.button2).Click += MainActivity_Click1; //消费1
            FindViewById<Button>(Resource.Id.button3).Click += MainActivity_Click2; //消费2
        }
        private void MainActivity_Click(object sender, System.EventArgs e)
        {
            var TempI = new Intent(this, typeof(SendActivity));
            StartActivity(TempI);


        }
        private void MainActivity_Click1(object sender, System.EventArgs e)
        {
            var TempI = new Intent(this, typeof(ServerActivity));
            StartActivity(TempI);
        }

        private void MainActivity_Click2(object sender, System.EventArgs e)
        {
            var TempI = new Intent(this, typeof(ServerActivity2));
            StartActivity(TempI);
        }
    }
}