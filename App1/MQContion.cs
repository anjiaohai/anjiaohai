using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class MQContion
    {
        public IConnection Connection(String TempUrl,String TempUserName,String TempUserPassWord , ref String TempRes)
        {
            try
            {
                ConnectionFactory TempConFac = new ConnectionFactory();
                TempConFac.UserName = TempUserName;//某个vhost下的用户
                TempConFac.Password = TempUserPassWord;//MQ密码
                TempConFac.VirtualHost = "/";
                TempConFac.RequestedHeartbeat = 60;//心跳检测配置
                Uri uri = new Uri(TempUrl);
                TempConFac.Endpoint = new AmqpTcpEndpoint(uri);
                IConnection Tempconn = TempConFac.CreateConnection();
                TempRes = "OK";
                return Tempconn;
            }
            catch (Exception TempE)
            {
                TempRes = TempE.Message;
                return null;
            }
          
        }

    }
}
