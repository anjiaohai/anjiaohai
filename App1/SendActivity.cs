using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RabbitMQ.Client;
using WindowsFormsApp1;

namespace App1
{
    [Activity(Label = "Send")]
    public class SendActivity : Activity
    {



        IConnection Connection;
        private static String EXCHANGE_NAME = "test_exchange_fanout";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Send);
            FindViewById<Button>(Resource.Id.button1).Click += MainActivity_Click;//连接
            FindViewById<Button>(Resource.Id.button2).Click += MainActivity_Click1; //发送                                        
            // Create your application here
        }
        private void MainActivity_Click1(object sender, System.EventArgs e)
        {
            Test();
        }
        public String ShowContion()
        {
            String TempRes = "";
            MQContion TempMQContion = new MQContion();
            String TmepUrl = "amqp://192.168.2.163:5672/";
            String TempUserName = "admin";
            String TempUserPassWord = "pmdbrootpassword";
            Connection = TempMQContion.Connection(TmepUrl, TempUserName, TempUserPassWord, ref TempRes);
            return TempRes;

        }

        private void MainActivity_Click(object sender, System.EventArgs e)
        {
            String TempRes = ShowContion();
            if (TempRes != "OK")
            {
                Toast.MakeText(this, TempRes, ToastLength.Long);
                return;
            }
            Toast.MakeText(this, "连接成功", ToastLength.Long);
          
        }
        public void Test()
        {
            try
            {

                //从连接中获取一个通道
                IModel TempChannelModel = Connection.CreateModel();
                //声明交换机（分发:发布/订阅模式）
                TempChannelModel.ExchangeDeclare(EXCHANGE_NAME, "fanout");
                //发送消息
                for (int i = 0; i < 10; i++)
                {
                    String message = "this is user registe message" + i;

                    //发送消息
                    TempChannelModel.BasicPublish(EXCHANGE_NAME, "", null, System.Text.Encoding.UTF8.GetBytes(message));
                    TempChannelModel.ConfirmSelect();
                    //等待服务器应答确认
                    if (!TempChannelModel.WaitForConfirms())
                    {
                        Toast.MakeText(this, "发送失败", ToastLength.Long);
                    }
                    Thread.Sleep(5 * i);
                }
                TempChannelModel.Close();
                Connection.Close();
            }
            catch (Exception e)
            {
                String TempR = e.Message;
            }
        }



    }
}