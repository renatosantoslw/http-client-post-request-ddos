using System.Net;

Console.WriteLine("INICIANDO POST REQUEST DDoS...");

//CRIA O OBJ HttpCli
HttpClientPostRequest.HttpCli Client = new HttpClientPostRequest.HttpCli();

//CHAMA A FABRICA DO OBJ Client
await Task.Factory.StartNew(() => Client.ExecRequest());
