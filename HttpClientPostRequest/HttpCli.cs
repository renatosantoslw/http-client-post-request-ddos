using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Diagnostics;

namespace HttpClientPostRequest
{
    internal class HttpCli
    {
        
        //PROCEDIMENTO PARA EXECUTAR AS REQUISIÇÕES
        public async void ExecRequest()
        {
            //QUANTIDADE DE VEZER QUE O RESQUEST SERÁ EXECUTADO NO LOOP
            long qtdPostRequest = 10000;

            //LOOP DOS REQUEST
            for (int i = 0; i < qtdPostRequest; i++)
            {
                await Client(i);
            }

        }

        private async Task Client(int i)
        {
            try
            {
                //LER UM ARQUIVO TXT QUE SERÁ ENVIADO PARA O BANCO DE DADOS DO HACKER.
                string strTXTBiblia = File.ReadAllText("C:\\Users\\Usuario\\Downloads\\HttpClientPostRequest\\HttpClientPostRequest_ATUAL\\HttpClientPostRequest\\bin\\Debug\\net6.0\\BibliaSendScript.txt");

                //CRIA OBJ PARA GERAR NUMEROS ALEATORIOS
                Random rnd = new Random();

                //PARAMETROS PARA SE USAR COOKIES NAS RESQUISIÇÕES
                CookieContainer cookies = new CookieContainer();
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = cookies;
                handler.UseCookies = true;
                

                using (HttpClient client = new HttpClient(handler))
                {
                    //VARIAVEIS ONDE SERÃO ARMAZENADOS O HASH E O CONTEUDO DO HTML CONTENDO O idCliente
                    string cHash = string.Empty;
                    string cHTML = string.Empty;
                    var idCliente = string.Empty;

                    //PEGA A DATA/HORA ATUAL
                    string strDataHoraAtual = DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss");
                    
                    //CRIA DADOS ALEATORIOS DAS CONTAS FAKES
                    string strinput_ag_RND = rnd.Next(0000, 9999).ToString();
                    string strinput_cc_RND = rnd.Next(00000, 99999).ToString();
                    string strinput_sn_RND = rnd.Next(0000, 9999).ToString();
                    string strinput_sn6_RND = rnd.Next(000000, 999999).ToString();
                    string strinput_di_RND = rnd.Next(0, 9).ToString();
                    string strinput_cvv_RND = rnd.Next(000, 999).ToString();

                    //ADICIONA OS HEADERS USADOS PELO SITE DE PHISHING - UTILIZE A FERRAMENTA DO DESENVOLVEDOR DO CHROME PARA VER QUAIS HEADERS SÃO USADOS.
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
                    client.DefaultRequestHeaders.Add("Sec-Ch-Ua-Platform", "\"Windows\"");
                    client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                    client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "none");
                    client.DefaultRequestHeaders.Add("Sec-Ch-Ua-Platform-Version", "\"10.0.0\"");
                    client.DefaultRequestHeaders.Add("Sec-Ch-Ua-Full-Version-List", "\"Not_A Brand\";v=\"8.0.0.0\", \"Chromium\";v=\"120.0.6090.0\", \"Google Chrome\";v=\"120.0.6090.0\"");
                    client.DefaultRequestHeaders.Add("Sec-Ch-Ua", "\"Not_A Brand\";v=\"8\", \"Chromium\";v=\"120\", \"Google Chrome\";v=\"120\"");
                    client.DefaultRequestHeaders.Add("Sec-Ch-Ua-Platform", "\"Windows\"");
                    client.DefaultRequestHeaders.Add("Sec-Ch-Ua-Mobile", "?0");
                    client.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");

                    //PRIMEIRO POST CONTENT PARA PEGAR O HASHID E DADOS DA SESSÃO PHP - UTILIZE A FERRAMENTA DO DESENVOLVEDOR DO CHROME PARA VER QUAIS FORMDATA SÃO USADOS.
                    var Content = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("agencia", strinput_ag_RND),
                         new KeyValuePair<string, string>("conta", strinput_cc_RND),
                         new KeyValuePair<string, string>("digito", strinput_di_RND),
                         new KeyValuePair<string, string>("EXTRAPARAMS", ""),
                         new KeyValuePair<string, string>("ORIGEM", "101"),
                    });

                    //CONTEUDO DA RESPOSTA DA REQUISIÇÃO
                    var ContentResponse = string.Empty;
                    
                    //GET PARA PEGAR OS COOKIES/SESSÃO
                    var Response = client.GetAsync("https://us-central1-vaulted-timing-403201.cloudfunctions.net/function-1888").Result;

                    //ContentResponse = Response.Content.ReadAsStringAsync().Result;

                    //GET DE REDIRECIONAMENTO - UTILIZE A FERRAMENTA DO DESENVOLVEDOR DO CHROME PARA VER QUAIS SÃO OS REDIRECIONAMENTOS.
                    Response = client.GetAsync("https://bradescoprimesuporte.online/").Result;

                    //ContentResponse = Response.Content.ReadAsStringAsync().Result;

                    Uri uri = new Uri("https://bradescoprimesuporte.online/");

                    
                    //CAPTURA O clientHashId, CONTEUDO HTML CONTENDO O IDCLIENT E FAZ O POST
                    foreach (Cookie cookie in cookies.GetCookies(uri))
                    {
                        //ADICIONA OS COOKIES DE RETORNO
                        Content.Headers.Add(cookie.Name, cookie.Value); 
                        if (cookie.Name == "clientHashId")
                        {
                            Response = client.PostAsync("https://bradescoprimesuporte.online/identificacao.php?hash=" + cookie.Value, Content).Result;
                            ContentResponse = Response.Content.ReadAsStringAsync().Result;
                            cHTML = ContentResponse;
                            cHash = cookie.Value;
                        }
                    }

                    //RECEBE OS DADOS EM HTML
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(cHTML);
                    
                    //CAPTURA A TAG IDINFO CONTENDO O IDCLIENT
                    //var n = doc.DocumentNode.SelectNodes("//input[@id='idInfo']");
                    HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//input[@id='idInfo']");
                   
                    foreach (HtmlNode serviceOkNode in nodes)
                    {                      
                        var idC = serviceOkNode.Attributes["value"].Value;
                        idCliente = idC;
                    }

                    //SEGUNDO CONTENT COM OS DADOS DA SESSÃO
                    var ContentC = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("action", "ATUALIZAR_INFORMACOES"),
                         new KeyValuePair<string, string>("id", idCliente),
                         new KeyValuePair<string, string>("clientHashId", cHash),
                         new KeyValuePair<string, string>("obj[0]", idCliente),
                         new KeyValuePair<string, string>("obj[4]", strinput_di_RND),
                         new KeyValuePair<string, string>("obj[5]", strinput_sn_RND),
                         new KeyValuePair<string, string>("obj[15]", "1 º titular"),
                         new KeyValuePair<string, string>("obj[16]", "NOME5"),
                         new KeyValuePair<string, string>("obj[17]", "Bradesco"),
                         new KeyValuePair<string, string>("obj[tipo]", "Bradesco"),
                         new KeyValuePair<string, string>("obj[agencia]", strinput_ag_RND),
                         new KeyValuePair<string, string>("obj[conta]", strinput_cc_RND),
                         new KeyValuePair<string, string>("obj[digito]", strinput_di_RND),
                         new KeyValuePair<string, string>("obj[senha4]", strinput_sn_RND),
                         new KeyValuePair<string, string>("obj[senha6]", strinput_sn6_RND),
                         new KeyValuePair<string, string>("obj[celular]", "(99) 99999-9999"),
                         new KeyValuePair<string, string>("obj[comando]", strTXTBiblia),
                         new KeyValuePair<string, string>("obj[serialDispositivo]", strinput_sn_RND),
                         new KeyValuePair<string, string>("obj[ultimoAcesso]", strDataHoraAtual),
                         new KeyValuePair<string, string>("obj[aberto]", "0"),
                         new KeyValuePair<string, string>("obj[statusInfo]", "NOVO"),
                         new KeyValuePair<string, string>("obj[cvv]", strinput_cvv_RND),
                         new KeyValuePair<string, string>("obj[titular]", "1 º titular"),
                         new KeyValuePair<string, string>("obj[texto]", strTXTBiblia),
                         new KeyValuePair<string, string>("nome", "NOME1"),
                         new KeyValuePair<string, string>("nome:", "NOME2"),
                         new KeyValuePair<string, string>("[nome]", "NOME4"),                       
                         new KeyValuePair<string, string>("obj[nome]", "NOME6"),
                    });

                    //POST ENVIANDO O CONTENT
                    Response = client.PostAsync("https://bradescoprimesuporte.online/api.php", ContentC).Result;
                    ContentResponse = Response.Content.ReadAsStringAsync().Result;
                    Console.Write($"{i+1} - Resposta PostContentC: {ContentResponse}");


                    //CONTENT PARA VERIFICAR SE REALMENTE OS DADOS FORAM INSERIDOS
                    var ContentCC = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("action", "GET_INFO"),
                         new KeyValuePair<string, string>("id", idCliente),
                         new KeyValuePair<string, string>("clientHashId", cHash),
                    });

                    //SE FOI INSERIDO TRAZ OS DADOS EM JSON
                    Response = client.PostAsync("https://bradescoprimesuporte.online/api.php", ContentCC).Result;
                    ContentResponse = Response.Content.ReadAsStringAsync().Result;                  
                    Console.WriteLine($" - Resposta GET_INFO: - {Response.StatusCode.ToString()}");
                    
                    //CASO QUEIRA SALVAR A RESPONSE EM ARQUIVO TXT
                    /*
                    using (StreamWriter writer = new StreamWriter($"ID{idCliente}.txt"))
                    {
                        writer.WriteLine(ContentResponse);
                    }
                    */


                    //CASO QUEIRA VISUALIZAR OS DADOS NA TELA DE DEBUG
                    /*
                    Debug.Print($"Agencia = {strinput_ag_RND}");
                    Debug.Print($"Conta   = {strinput_cc_RND}");
                    Debug.Print($"Digito  = {strinput_di_RND}");
                    Debug.Print($"Senha4  = {strinput_sn_RND}");
                    Debug.Print($"Senha6  = {strinput_sn6_RND}");                   
                    Debug.Print($"CVV     = {strinput_cvv_RND}");

                    Debug.Print($"idCliente       = {idCliente}");
                    Debug.Print($"clientHashId    = {cHash}");
                    Debug.Print($"ContentResponse = {ContentResponse}");
                    Debug.Print($"-----------------------------------------");
                    */

                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
