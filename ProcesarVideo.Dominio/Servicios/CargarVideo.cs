using Videos.Dominio.Entidades;
using Videos.Dominio.Puertos.Repositorios;
using Google.Cloud.Storage.V1;
using Google.Cloud.PubSub.V1;
using System.Text;
using Google.Apis.Auth.OAuth2;
using System.Text.Json;

namespace Videos.Dominio.Servicios
{
    public class CargarVideo(IVideoRepositorio videoRepositorio)
    {
        private readonly IVideoRepositorio _videoRepositorio = videoRepositorio;
        GoogleCredential credential = null;
        public async Task Cargar(Video video)
        {
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

        private void PublicarMensaje(Video video)
        {
            using (var jsonStream = new FileStream("../../Recursos/experimento-ccp-8172d4037e96.json", FileMode.Open,
                FileAccess.Read, FileShare.Read))
            {
                credential = GoogleCredential.FromStream(jsonStream);
            }

            MessagePubSub mensajePubSub = new MessagePubSub();
            mensajePubSub.Id = video.Id;
            mensajePubSub.Archivo = video.Archivo;
            mensajePubSub.Nombre = video.Nombre;
            var topicName = new TopicName("experimento-ccp", "video-ccp");
            var publisher = PublisherServiceApiClient.Create();

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
