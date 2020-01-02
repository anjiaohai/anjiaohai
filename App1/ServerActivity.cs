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
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using WindowsFormsApp1;

namespace App1
{
    [Activity(Label = "Server")]
    public class ServerActivity : Activity
    {



        Button BtnText;
        private static String QUEUE_NAME = "test_queue_work2";
        private static String EXCHANGE_NAME = "test_exchange_fanout";
        EventingBasicConsumer consumer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Server);
            FindViewById<Button>(Resource.Id.button1).Click += ServerActivity_Click;
            BtnText = FindViewById<Button>(Resource.Id.button2);
            // Create your application here
        }
     
        private void ServerActivity_Click(object sender, EventArgs e)
        {
            Test();
        }

        public void Test()
        {
            try
            { //(1）创建与服务器之间的连接
                String TempRes = "";
   
                MQContion TempMQContion = new MQContion();
                String TmepUrl = "amqp://192.168.2.163:5672/";
                String TempUserName = "admin";
                String TempUserPassWord = "pmdbrootpassword";

                IConnection TempConnection = TempMQContion.Connection(TmepUrl, TempUserName, TempUserPassWord, ref TempRes);
                if (TempRes != "OK")
                {
                    Toast.MakeText(this, TempRes, ToastLength.Long);
      
                }
                //从连接中获取一个通道
                var TempChannel = TempConnection.CreateModel();
                //声明交换机（分发:发布/订阅模式）
                TempChannel.ExchangeDeclare(EXCHANGE_NAME, "fanout");
                //声明队列
                TempChannel.QueueDeclare(QUEUE_NAME, true, false, false, null);
                //将队列绑定到交换机
                TempChannel.QueueBind(QUEUE_NAME, EXCHANGE_NAME, "");
                //保证一次只分发一个  
                TempChannel.BasicQos(0, 1, false);
                consumer = new EventingBasicConsumer(TempChannel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);//这里是我们订阅到的消息
                    BtnText.Text =  message;
                    //txtLog.AppendText(message);
                    //txtLog.AppendText("\r\n");

                    // 手动发送消息确认信号。消息从队列中删除, 如果 [TAGA] 处代码 第二个参数为true那么这行代码就可以不要了.一般为了系统稳定都需要手工应答确保任务成功完成,再从队列中删除消息.
                    TempChannel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                };
                //TAGA 
                TempChannel.BasicConsume(QUEUE_NAME, false, consumer);
            }
            catch (Exception TempE)
            {
                String TempR = TempE.Message;
            }
        }
    }
}