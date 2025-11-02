using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BDCPlugins.exception;
using ModuleBase.Tool;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class MyRequest
{
    public static int SleepTime = 100;
    public static int XuHuanQingQiuCiShu = 5;
    private readonly HttpClient _httpClient = new HttpClient();



    public Dictionary<string, object> PostRequest(string url, Dictionary<string, string> requestHeaders, Object body)
    {
        // 创建HttpRequestMessage对象并设置URL和请求方法（POST）
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);

        // 设置请求头
        foreach (var header in requestHeaders)
        {
            requestMessage.Headers.Add(header.Key, header.Value);
        }

        // 将Dictionary转换为JSON格式的字符串，并作为请求体添加到HttpRequestMessage中（使用Newtonsoft.Json）
        var requestBodyJson = JsonConvert.SerializeObject(body);
        var content = new StringContent(requestBodyJson, System.Text.Encoding.UTF8, "application/json");
        requestMessage.Content = content;
        try
        {
            using (var response = _httpClient.SendAsync(requestMessage).Result) // 注意这里使用了.Result属性导致同步阻塞
            {
                // 确保请求成功（HTTP状态码200-299之间）


                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"请求成功，响应内容: {responseBody}");
                }
                else
                {
                    Console.WriteLine($"请求失败，状态码: {response.StatusCode}, 错误信息: {response.ReasonPhrase}");
                }
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"POST请求失败，HTTP状态码: {response.StatusCode}");
                }


                // 读取响应内容为JSON字符串
                var responseBodyJson = response.Content.ReadAsStringAsync().Result; // 同步读取

                // 将JSON字符串反序列化为Dictionary<string, object>（使用Newtonsoft.Json）
                var responseDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBodyJson);

                if (responseDict["code"] != null && responseDict["code"].ToString() == "500" && responseDict["msg"] != null && responseDict["msg"].ToString().Contains("No route to host"))
                {
                    Thread.Sleep(19500);
                    return PostRequest(url, requestHeaders, body);
                }

                Thread.Sleep(SleepTime);
                indexCount = 1;
                return responseDict;
            }
        }
        catch (Exception ex)
        {
            if (indexCount % XuHuanQingQiuCiShu == 0)
            {
                throw ex;
            }
            Thread.Sleep(SleepTime);
            indexCount++;
            return PostRequest(url, requestHeaders, body);
        }
        // 发送HTTP POST请求并获取响应

    }



    public Dictionary<string, object> PostRequest(string url, Dictionary<string, string> requestHeaders, Dictionary<string, object> requestBody,bool still= false)
    {
        // 创建HttpRequestMessage对象并设置URL和请求方法（POST）
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);

        // 设置请求头
        foreach (var header in requestHeaders)
        {
            requestMessage.Headers.Add(header.Key, header.Value);
        }

        // 将Dictionary转换为JSON格式的字符串，并作为请求体添加到HttpRequestMessage中（使用Newtonsoft.Json）
        var requestBodyJson = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(requestBodyJson, System.Text.Encoding.UTF8, "application/json");
        requestMessage.Content = content;
        try
        {
            using (var response = _httpClient.SendAsync(requestMessage).Result) // 注意这里使用了.Result属性导致同步阻塞
            {
                // 确保请求成功（HTTP状态码200-299之间）
                

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"请求成功，响应内容: {responseBody}");
                }
                else
                {
                    Console.WriteLine($"请求失败，状态码: {response.StatusCode}, 错误信息: {response.ReasonPhrase}");
                }
                if (!response.IsSuccessStatusCode)
                {
                    //throw new HttpRequestException($"POST请求失败，HTTP状态码: {response.StatusCode}");
                }


                // 读取响应内容为JSON字符串
                var responseBodyJson = response.Content.ReadAsStringAsync().Result; // 同步读取

                // 将JSON字符串反序列化为Dictionary<string, object>（使用Newtonsoft.Json）
               // var responseDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBodyJson);

                var responseDict = new Dictionary<string, object>();

                try
                {
                    if (responseDict["code"] != null && responseDict["code"].ToString() == "500" && responseDict["msg"] != null && responseDict["msg"].ToString().Contains("No route to host"))
                    {
                        Thread.Sleep(19500);
                        return PostRequest(url, requestHeaders, requestBody, still);
                    }
                    Thread.Sleep(SleepTime);
                    indexCount = 1;
                    return responseDict;
                }
                catch(Exception ex)
                {
                    String path =  AppTool.GetTime("请求错误.txt");
                    
                    FileTool.WriteTxt(path, new Dictionary<string, string>
                    {
                        {"url", url},
                        {"responseDict", JsonTool.ToString(responseDict)},
                    });
                    throw ex;
                }
               
            }
        }
        catch(Exception ex)
        {
            if (indexCount % XuHuanQingQiuCiShu == 0)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
            Thread.Sleep(SleepTime);
            indexCount++;
            return PostRequest(url, requestHeaders, requestBody);
        }
            // 发送HTTP POST请求并获取响应

    }

    public static JObject GetReposeDataAll(Dictionary<string, object> resultDict)
    {
        var json =JObject.FromObject(resultDict);
        if(json.GetValue("code").ToString() == "901")
        {
            throw new RequsetErrorException($"请求失败，{json.GetValue("msg").ToString()}");
        }
        return json;
    }

    internal static JObject GetReposeData(Dictionary<string, object> respose)
    {
        if(respose["code"].ToString() == "0")
        {
            object data = respose["data"];
            return data as JObject;
        }
        else
        {
            string messageCode = "msg";
            throw new RequsetErrorException($"请求失败，{respose[messageCode]}");
        }
    }

    internal static List<Dictionary<string, object>> GetReposeRows(Dictionary<string, object> respose)
    {
        JObject jObject = GetReposeData(respose);
        var arrayOfObjects = new List<Dictionary<string, object>>();

        if (jObject.TryGetValue("rows", out JToken arrayToken) && arrayToken.Type == JTokenType.Array)
        {
           

            foreach (JObject j in arrayToken.Children<JObject>())
            {
                // 将每个 JObject 转换为 Dictionary<string, object>
                var dictionary = j.ToObject<Dictionary<string, object>>();
                arrayOfObjects.Add(dictionary);
            }

            
        }
        return arrayOfObjects;
    }

   
    public static int indexCount = 1;
    
    public Dictionary<string, object> PostRequestFile(string url, Dictionary<string, string> requestHeaders, Dictionary<string, Object> bodyData, string filePath,bool Still = false)
    {
        Thread.Sleep(SleepTime*3);
        using (HttpClient client = new HttpClient())
        {

                var multipartContent = new MultipartFormDataContent();

                // 添加请求头到 HttpClient DefaultRequestHeaders
                foreach (var header in requestHeaders)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                // 序列化请求体数据为 JSON 字符串，并添加到 multipart/form-data
                string jsonRequestBody = JsonConvert.SerializeObject(bodyData);
            // multipartContent.Add(new StringContent(jsonRequestBody, Encoding.UTF8, "application/json"), "request_body");


              

            
            // 添加文件到 multipart/form-data
            var fileStreamContent = new StreamContent(File.OpenRead(filePath));
              fileStreamContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("multipart/form-data");
              multipartContent.Add(fileStreamContent, "file", Path.GetFileName(filePath));
            foreach (var item in bodyData)
            {
                if(item.Value != null)
                {
                    multipartContent.Add(new StringContent(item.Value.ToString(), Encoding.UTF8, "application/json"), item.Key);
                }
                else
                {
                    //multipartContent.Add(null, item.Key);
                }
               
            }
            try
            {
                // 发送 POST 请求
                HttpResponseMessage response = client.PostAsync(url, multipartContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"请求成功，响应内容: {responseBody}");
                }
                else
                {
                    Console.WriteLine($"请求失败，状态码: {response.StatusCode}, 错误信息: {response.ReasonPhrase}");
                }
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"POST请求失败，HTTP状态码: {response.StatusCode}");
                }

                // 读取响应内容为JSON字符串
                var responseBodyJson = response.Content.ReadAsStringAsync().Result; // 同步读取

                // 将JSON字符串反序列化为Dictionary<string, object>（使用Newtonsoft.Json）
                var responseDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBodyJson);
                if (responseDict["code"] != null && responseDict["code"].ToString() == "500" && responseDict["msg"] != null && responseDict["msg"].ToString().Contains("No route to host"))
                {
                    Thread.Sleep(19500);
                    return PostRequestFile(url, requestHeaders, bodyData, filePath);
                }
                indexCount = 1;
                return responseDict;
            }
            catch(Exception ex)
            {
                if (Still)
                {
                    Thread.Sleep(19500);
                    return PostRequestFile(url, requestHeaders, bodyData, filePath);
                }
                if (indexCount % XuHuanQingQiuCiShu == 0)
                {
                    throw ex;
                }
                Thread.Sleep(SleepTime);
                indexCount++;
                return PostRequestFile( url, requestHeaders, bodyData,  filePath);

            }

          
        }
    }

   

    /// <summary>
    /// 发送HTTP DELETE请求，并传递字典形式的参数。
    /// 如果API支持query parameters或者请求体（例如表单提交），则这里会分别处理这两种情况。
    /// 请注意，这个方法是同步阻塞的，请确保在适当的线程中调用。
    /// </summary>
    /// <param name="uri">API URI</param>
    /// <param name="parameters">要传递的参数字典</param>
    public Dictionary<string, object> DeleteSync(string uri, Dictionary<string, string> parameters, Dictionary<string, string> requestHeaders=null, Dictionary<string, Object> requestBody = null)
    {
        Thread.Sleep(SleepTime);
        var request = new HttpRequestMessage(HttpMethod.Delete, BuildUriWithQuery(uri, parameters));

        try
        {
            if(requestHeaders != null)
            {
                foreach (var header in requestHeaders)
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            if(requestBody != null)
            {
                // 将Dictionary转换为JSON格式的字符串，并作为请求体添加到HttpRequestMessage中（使用Newtonsoft.Json）
                var requestBodyJson = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(requestBodyJson, System.Text.Encoding.UTF8, "application/json");
                request.Content = content;
            }
           


            // 使用Task.Run将异步调用转换为同步
            Task<HttpResponseMessage> task = _httpClient.SendAsync(request);
            

            Thread.Sleep(SleepTime);
            var response = task.Result;
            if (response.IsSuccessStatusCode)
            {
                string responseBody = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine($"请求成功，响应内容: {responseBody}");
            }
            else
            {
                Console.WriteLine($"请求失败，状态码: {response.StatusCode}, 错误信息: {response.ReasonPhrase}");
            }
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"POST请求失败，HTTP状态码: {response.StatusCode}");
            }

            // 读取响应内容为JSON字符串
            var responseBodyJson = response.Content.ReadAsStringAsync().Result; // 同步读取

            // 将JSON字符串反序列化为Dictionary<string, object>（使用Newtonsoft.Json）
            var responseDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBodyJson);
            if (responseDict["code"] != null && responseDict["code"].ToString() == "500" && responseDict["msg"] != null && responseDict["msg"].ToString().Contains("No route to host"))
            {
                Thread.Sleep(19500);
                return DeleteSync(uri, parameters);
            }
            indexCount = 1;
            return responseDict;
        }
        catch (Exception ex)
        {
            if (indexCount % XuHuanQingQiuCiShu == 0)
            {
                throw ex;
            }
            indexCount++;
            Thread.Sleep(SleepTime);
            return DeleteSync(uri,parameters);
        }
    }

    /// <summary>
    /// 构建带有查询参数的URI
    /// </summary>
    private static Uri BuildUriWithQuery(string uri, Dictionary<string, string> parameters)
    {
        var builder = new UriBuilder(uri);

        if (parameters.Count > 0)
        {
            var queryPairs = parameters.Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}");
            builder.Query = "?" + string.Join("&", queryPairs);
        }

        return builder.Uri;
    }

    public Dictionary<string, object> GetRequest(string url, Dictionary<string, string> requestHeaders)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        foreach (var header in requestHeaders)
        {
            requestMessage.Headers.Add(header.Key, header.Value);
        }
        try
        {
            using (var response = _httpClient.SendAsync(requestMessage).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"请求成功，响应内容: {responseBody}");
                }
                else
                {
                    Console.WriteLine($"请求失败，状态码: {response.StatusCode}, 错误信息: {response.ReasonPhrase}");
                }
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"GET请求失败，HTTP状态码: {response.StatusCode}");
                }
                var responseBodyJson = response.Content.ReadAsStringAsync().Result;
                var responseDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBodyJson);
                Thread.Sleep(SleepTime);
                indexCount = 1;
                if (responseDict["code"] != null && responseDict["code"].ToString() == "500" && responseDict["msg"] != null && responseDict["msg"].ToString().Contains("No route to host"))
                {
                    Thread.Sleep(19500);
                    return GetRequest(url, requestHeaders);
                }
                return responseDict;
            }
        }
        catch (Exception ex)
        {
            if (indexCount % XuHuanQingQiuCiShu == 0)
            {
                throw ex;
            }
            Thread.Sleep(SleepTime);
            indexCount++;
            return GetRequest(url, requestHeaders);
        }
    }






































    /// <summary>
    /// 下载文件到指定路径
    /// </summary>
    /// <param name="url">下载URL</param>
    /// <param name="savePath">保存路径</param>
    /// <param name="requestHeaders">请求头</param>
    /// <param name="progressCallback">进度回调（可选）</param>
    /// <returns>下载是否成功</returns>
    public Dictionary<string, object> DownloadFile(string url, string savePath, Dictionary<string, string> requestHeaders = null, Action<long, long> progressCallback = null)
    {
        return DownloadFileAsync(url, savePath, requestHeaders, progressCallback).Result;
    }

    /// <summary>
    /// 异步下载文件到指定路径
    /// </summary>
    /// <param name="url">下载URL</param>
    /// <param name="savePath">保存路径</param>
    /// <param name="requestHeaders">请求头</param>
    /// <param name="progressCallback">进度回调（可选）</param>
    /// <returns>下载是否成功</returns>
    public async Task<Dictionary<string, object>> DownloadFileAsync(string url, string savePath, Dictionary<string, string> requestHeaders = null, Action<long, long> progressCallback = null)
    {
        Thread.Sleep(SleepTime);

        try
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, url))
            {
                // 设置请求头
                if (requestHeaders != null)
                {
                    foreach (var header in requestHeaders)
                    {
                        requestMessage.Headers.Add(header.Key, header.Value);
                    }
                }

                using (var response = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        // 确保目录存在
                        var directory = Path.GetDirectoryName(savePath);
                        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        // 获取文件总大小
                        var totalBytes = response.Content.Headers.ContentLength ?? -1L;

                        using (var contentStream = await response.Content.ReadAsStreamAsync())
                        using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                        {
                            var buffer = new byte[8192];
                            var totalBytesRead = 0L;
                            var bytesRead = 0;

                            while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                await fileStream.WriteAsync(buffer, 0, bytesRead);
                                totalBytesRead += bytesRead;

                                // 报告进度
                                progressCallback?.Invoke(totalBytesRead, totalBytes);
                            }

                            Console.WriteLine($"文件下载成功: {savePath}");
                            indexCount = 1;

                            Dictionary<string, object> result = new Dictionary<string, object>();
                            result.Add("code", "0");
                            result.Add("msg", "文件下载成功");
                            return result;



                          
                          

                        }
                    }
                    else
                    {
                        Console.WriteLine($"下载请求失败，状态码: {response.StatusCode}, 错误信息: {response.ReasonPhrase}");
                        throw new HttpRequestException($"下载请求失败，HTTP状态码: {response.StatusCode}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (indexCount % XuHuanQingQiuCiShu == 0)
            {
                Console.WriteLine($"文件下载失败: {ex.Message}");
                throw ex;
            }

            Thread.Sleep(SleepTime);
            indexCount++;
            Console.WriteLine($"下载失败，第{indexCount}次重试...");
            return await DownloadFileAsync(url, savePath, requestHeaders, progressCallback);
        }
    }

    /// <summary>
    /// 下载文件并返回字节数组
    /// </summary>
    /// <param name="url">下载URL</param>
    /// <param name="requestHeaders">请求头</param>
    /// <returns>文件字节数组</returns>
    public byte[] DownloadFileAsBytes(string url, Dictionary<string, string> requestHeaders = null)
    {
        return DownloadFileAsBytesAsync(url, requestHeaders).Result;
    }

    /// <summary>
    /// 异步下载文件并返回字节数组
    /// </summary>
    /// <param name="url">下载URL</param>
    /// <param name="requestHeaders">请求头</param>
    /// <returns>文件字节数组</returns>
    public async Task<byte[]> DownloadFileAsBytesAsync(string url, Dictionary<string, string> requestHeaders = null)
    {
        Thread.Sleep(SleepTime);

        try
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, url))
            {
                // 设置请求头
                if (requestHeaders != null)
                {
                    foreach (var header in requestHeaders)
                    {
                        requestMessage.Headers.Add(header.Key, header.Value);
                    }
                }

                using (var response = await _httpClient.SendAsync(requestMessage))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var fileBytes = await response.Content.ReadAsByteArrayAsync();
                        Console.WriteLine($"文件下载成功，大小: {fileBytes.Length} 字节");
                        indexCount = 1;
                        return fileBytes;
                    }
                    else
                    {
                        Console.WriteLine($"下载请求失败，状态码: {response.StatusCode}, 错误信息: {response.ReasonPhrase}");
                        throw new HttpRequestException($"下载请求失败，HTTP状态码: {response.StatusCode}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (indexCount % XuHuanQingQiuCiShu == 0)
            {
                Console.WriteLine($"文件下载失败: {ex.Message}");
                throw ex;
            }

            Thread.Sleep(SleepTime);
            indexCount++;
            Console.WriteLine($"下载失败，第{indexCount}次重试...");
            return await DownloadFileAsBytesAsync(url, requestHeaders);
        }
    }
}
