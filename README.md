# Serverless Apps with Azure Functions

[#KuliahKamis](https://twitter.com/hashtag/KuliahKamis?src=hashtag_click)
[#RadyaIsMe](https://twitter.com/hashtag/RadyaIsMe?src=hashtag_click)
![fn-is-running](/assets/fn-is-running.jpeg)

<br>

# Materi

## Apa itu Azure Functions?
Merupakan layanan komputasi serverless yang memungkinkan pengguna untuk menjalankan event-based code tanpa perlu menyediakan atau mengelola infrastrukturnya ([serverless360](https://www.serverless360.com/azure-functions)).
- Memungkin untuk fokus pada kode
- Mudah ditulis dan dideploy
- Berjalan ketika ada event
- Autoscale
- Zero maintenance

## Perbedaan dengan App Service
| Tipe | Serverless | Need Storage Account | Long running |
| --- | :---: | :--: | :---: |
| App Service | :x: | :x: | :heavy_check_mark: |
| Functions | :heavy_check_mark: | :heavy_check_mark: | :x: |

## Contoh penggunaan
- Pengingat (notifikasi)
- Kalkulasi
- Web API yang ringan

<br>

# Praktik

## Tools yang dipakai
- Visual Studio Code (code editor)
- C#, Azure Functions, Azure Storage (VS Code extensions)
- Azure Functions Core Tools ([link](https://github.com/Azure/azure-functions-core-tools))
- Azure subscription

## Azure Functions yang dibuat
- Versi: 4
- Bahasa: C#
- Worker: isolated ([link](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/net-on-azure-functions-roadmap/ba-p/2197916))

## Buat project Azure Functions
- Azure > Functions > Create New Project...
- Buat HttpTrigger sederhana

## Deploy ke Azure
- Buat resource Functions via portal Azure
- Deploy via Azure Functions extension

## Monitoring dengan Applications Insight

## Buat timer trigger
- Azure > Functions > Create Function... > TimerTrigger

## Buat blob trigger
- Azure > Functions > Create Function... > Azure Blob Storage Trigger

## Buat dummy service
- Service untuk kirim email

## Tambah middleware
- Implementasi interface IFunctionsWorkerMiddleware
    ```csharp
    public class ExceptionMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var request = context.GetHttpRequestData();
                if (request != null) // ensure trigger is HttpTrigger
                {
                    var response = request.CreateResponse(HttpStatusCode.InternalServerError);
                    response.WriteString(ex.Message);
                    context.InvokeResult(response);

                    return;
                }

                throw;
            }
        }
    }
    ```
- Tambah FunctionContext extension
    ```csharp
    public static class FunctionContextExtension
    {
        internal static HttpRequestData GetHttpRequestData(this FunctionContext context)
        {
            var keyValuePair = context.Features.SingleOrDefault(f => f.Key.Name == "IFunctionBindingsFeature");
            var functionBindingsFeature = keyValuePair.Value;
            var type = functionBindingsFeature.GetType();
            var inputData = type.GetProperties().Single(p => p.Name == "InputData").GetValue(functionBindingsFeature) as IReadOnlyDictionary<string, object>;
            return inputData?.Values.SingleOrDefault(o => o is HttpRequestData) as HttpRequestData;
        }

        internal static void InvokeResult(this FunctionContext context, HttpResponseData response)
        {
            var keyValuePair = context.Features.SingleOrDefault(f => f.Key.Name == "IFunctionBindingsFeature");
            var functionBindingsFeature = keyValuePair.Value;
            var type = functionBindingsFeature.GetType();
            var result = type.GetProperties().Single(p => p.Name == "InvocationResult");
            result.SetValue(functionBindingsFeature, response);
        }
    }
    ```
- Register middleware di `Program.cs`

## Tambah Swagger doc
- Install package `Microsoft.Azure.Functions.Worker.Extensions.OpenApi`
- Tambah attribute `OpenApiOperationAttribute` di setiap HttpTrigger
- Register OpenApi di `Program.cs`