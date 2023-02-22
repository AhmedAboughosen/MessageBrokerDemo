// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory() {HostName = "localhost"};

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "pubsub", type: ExchangeType.Fanout);

var message = "Hello I want to broadcast this message";

var body = Encoding.UTF8.GetBytes(message);


channel.BasicPublish(exchange: "pubsub", "", true,null,body);

//wait for 5 second or throw exception
//log when  throw exception
channel.WaitForConfirmsOrDie(new TimeSpan(0,0,5));

// or use async mehtod 

channel.BasicAcks += (sender, se) =>
{
    //when message consume by queue
    Console.WriteLine($" message recevied");
};


channel.BasicReturn += (sender, se) =>
{
    //when message does not consume by queue
    Console.WriteLine($" message does not consume");
};


Console.WriteLine($"Send message: {message}");


Console.ReadKey();
