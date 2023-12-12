using Grpc.Core;
using Grpc.Net.Client;
using Grpc_Server;

var channel = GrpcChannel.ForAddress("https://localhost:7192");
var client = new Greeter.GreeterClient(channel);

var reply = await client.SayHelloAsync(
    new HelloRequest { Name = ".Net lab3 is working..." });
var call = client.SayHelloStream();

Console.WriteLine("From server: " + reply.Message);

var readTask = Task.Run(async () =>
{
    await foreach (var response in call.ResponseStream.ReadAllAsync())
    {
        Console.WriteLine(response.Message);
    }
});

while (true)
{
    var result = Console.ReadLine();
    if(string.IsNullOrWhiteSpace(result))
    {
        break;
    }
    await call.RequestStream.WriteAsync(new HelloRequest() { Name = result });
}

await call.RequestStream.CompleteAsync();
await readTask;

