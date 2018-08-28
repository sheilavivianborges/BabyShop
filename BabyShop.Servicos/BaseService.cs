using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BabyShop.Servicos
{
    /// <summary>
    /// Classe para comunicação com a API via HTTP
    /// </summary>
    public abstract class BaseService : IDisposable
    {
        private string uri;
        private string serviceKey;

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public BaseService( )
        {

        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="_uri">URI da API</param>
        /// <param name="_serviceKey">Chave de acesso a API, se houver</param>
        public BaseService( string _uri, string _serviceKey )
        {
            this.uri = _uri;
            this.serviceKey = _serviceKey;
        }

        /// <summary>
        /// Define a URL base do HTTPClient
        /// </summary>
        /// <param name="client"></param>
        private void ConfigurarHttpClient( HttpClient client )
        {
            client.BaseAddress = new Uri( this.uri );
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", this.serviceKey);
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Executa um HTTP Get na API
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser obtido da API</typeparam>
        /// <param name="request">URL a ser consultada na API</param>
        /// <returns>Retorna o objeto consultado, do tipo T</returns>
        public T Get<T>( string request )
        {
            try
            {
                HttpResponseMessage response;

                // Chamada da classe HTTPClient para acesso na API
                using ( var client = new HttpClient( ) )
                {
                    this.ConfigurarHttpClient( client );

                    response = client.GetAsync( request ).Result;
                }

                // Converte e deserializa o retorno em JSON da API
                string retorno = response.Content.ReadAsStringAsync( ).Result;
                return JsonConvert.DeserializeObject<T>( retorno );
            }
            catch ( Exception ex )
            {
                throw ex;
            }
        }

        /// <summary>
        /// Executa um HTTP Post na API
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser postado e obtido na API</typeparam>
        /// <param name="request">URL a ser consultada na API</param>
        /// <param name="parametro">Objeto a ser postado na API</param>
        /// <returns>Retorna o objeto consultado, do tipo T</returns>
        public async Task<T> Post<T>( string request, object parametro )
        {
            HttpResponseMessage response;

            // Chamada da classe HTTPClient para acesso na API
            using ( var client = new HttpClient( ) )
            {
                this.ConfigurarHttpClient( client );

                // Converte o objeto recebido em JSON
                string parametroJSON = JsonConvert.SerializeObject( parametro );
                StringContent conteudo = new StringContent( parametroJSON, Encoding.UTF8, "application/json" );

                try
                {
                    // Chamada na API
                    response = await client.PostAsync( request, conteudo ).ConfigureAwait( false );
                }
                catch ( Exception ex )
                {

                    throw ex;
                }

            }

            // Converte e deserializa o retorno em JSON da API
            var retorno = JsonConvert.DeserializeObject<T>( response.Content.ReadAsStringAsync( ).Result );
            return retorno;
        }

        #region Disposable

        // Flag: Has Dispose already been called?
        bool disposed = false;

        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle( IntPtr.Zero, true );

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose( )
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose( bool disposing )
        {
            if ( disposed )
                return;

            if ( disposing )
            {
                handle.Dispose( );
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        #endregion
    }
}
