using Videos.Dominio.Entidades;
using Videos.Dominio.Puertos.Repositorios;
using Google.Cloud.Storage.V1;
using Google.Cloud.PubSub.V1;
using Google.Apis.Auth.OAuth2;
using System.Text.Json;
using Grpc.Auth;
using GrpcChannelOptions = Grpc.Net.Client.GrpcChannelOptions;

namespace Videos.Dominio.Servicios
{
    public class CargarVideo(IVideoRepositorio videoRepositorio)
    {
        private readonly IVideoRepositorio _videoRepositorio = videoRepositorio;
        GoogleCredential credential = null;
        public async Task Cargar(Video video)
        {
            getGoogleClient();
            if (ValidarVideo(video))
            {
                video.Id = Guid.NewGuid();
                video.FechaCreacion = DateTime.Now;
                video.UrlVideo = "https://storage.googleapis.com/videos_ccp/" + video.Nombre;
                video.EstadoCarga = "Cargado";

                await videoRepositorio.Cargar(video);
                await AlmacenarVideo(video);
                PublicarMensaje(video);
            }
            else
            {
                throw new InvalidOperationException("Valores incorrectos para los parametros minimos");
            }
        }

        private void getGoogleClient()
        {
            if (credential == null)
            {
                using (var jsonStream = new FileStream("Recursos/experimento-ccp-8172d4037e96.json", FileMode.Open,
                FileAccess.Read, FileShare.Read))
                {
                    credential = GoogleCredential.FromStream(jsonStream);
                }
            }
        }

        private void PublicarMensaje(Video video)
        {
            MessagePubSub mensajePubSub = new MessagePubSub();
            mensajePubSub.Id = video.Id;
            mensajePubSub.Archivo = video.Archivo;
            mensajePubSub.Nombre = video.Nombre;
            var topicName = new TopicName("experimento-ccp", "video-ccp");

            // 2. Crear PublisherServiceApiSettings con ChannelCredentials personalizados
            var channelCredentials = Grpc.Core.ChannelCredentials.Create(
                Grpc.Core.ChannelCredentials.SecureSsl,
                credential.ToCallCredentials()
            );

            var channelOptions = new GrpcChannelOptions
            {
                Credentials = channelCredentials
            };

            // 3. Crear el cliente usando la configuración personalizada
            var clientBuilder = new PublisherServiceApiClientBuilder
            {
                ChannelCredentials = channelCredentials
            };

            PublisherServiceApiClient publisher = clientBuilder.Build();

            var message = System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(mensajePubSub));
            var pubsubMessage = new PubsubMessage
            {
                Data = Google.Protobuf.ByteString.CopyFrom(message)
            };
            publisher.Publish(topicName, new[] { pubsubMessage });
        }

        private async Task AlmacenarVideo(Video video)
        {
            var gcsStorage = StorageClient.Create(credential);
            byte[] binaryData = Convert.FromBase64String(video.Archivo);
            var file = System.Text.Encoding.UTF8.GetBytes(video.Archivo);

            await gcsStorage.UploadObjectAsync(
                    "videos_ccp",
                    video.Nombre,
                    "video/mp4",
                    new MemoryStream(binaryData),
                    new UploadObjectOptions
                    {
                        PredefinedAcl = PredefinedObjectAcl.PublicRead
                    });
        }

        public bool ValidarVideo(Video video)
        {
            return video.IdCliente != Guid.Empty && video.IdProducto != 0 && !string.IsNullOrEmpty(video.Nombre) && !string.IsNullOrEmpty(video.Archivo);
        }
    }
}
