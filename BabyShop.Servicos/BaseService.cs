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
    public abstract class BaseService : IDisposable
    {
        private string uri;
        private string serviceKey;

        public BaseService( )
        {

        }

        public BaseService( string _uri, string _serviceKey )
        {
            this.uri = _uri;
            this.serviceKey = _serviceKey;
        }

        private void ConfigurarHttpClient( HttpClient client )
        {
            client.BaseAddress = new Uri( this.uri );
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", this.serviceKey);
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public T Get<T>( string request )
        {
            try
            {
                HttpResponseMessage response;

                using ( var client = new HttpClient( ) )
                {
                    this.ConfigurarHttpClient( client );

                    response = client.GetAsync( request ).Result;
                }

                string retorno = response.Content.ReadAsStringAsync( ).Result;

                return JsonConvert.DeserializeObject<T>( retorno );
            }
            catch ( Exception ex )
            {
                throw ex;
            }
        }

        public async Task<T> Post<T>( string request, object parametro )
        {
            HttpResponseMessage response;

            using ( var client = new HttpClient( ) )
            {
                this.ConfigurarHttpClient( client );
                string parametroJSON = JsonConvert.SerializeObject( parametro );
                StringContent conteudo = new StringContent( parametroJSON, Encoding.UTF8, "application/json" );

                try
                {
                    response = await client.PostAsync( request, conteudo ).ConfigureAwait( false );
                }
                catch ( Exception ex )
                {

                    throw ex;
                }

            }

            var retorno = JsonConvert.DeserializeObject<T>( response.Content.ReadAsStringAsync( ).Result );
            return retorno;
        }

        public T Put<T>( string request, object parametro )
        {
            HttpResponseMessage response;

            using ( var client = new HttpClient( ) )
            {
                this.ConfigurarHttpClient( client );
                string parametroJSON = JsonConvert.SerializeObject( parametro );
                StringContent conteudo = new StringContent( parametroJSON, Encoding.UTF8, "application/json" );

                response = client.PutAsync( request, conteudo ).Result;
            }

            var retorno = JsonConvert.DeserializeObject<T>( response.Content.ReadAsStringAsync( ).Result );
            return retorno;
        }

        public T PostFormUrlEncodedContent<T>( string request, HttpContent parametros )
        {
            HttpResponseMessage response;

            using ( var client = new HttpClient( ) )
            {
                this.ConfigurarHttpClient( client );

                response = client.PostAsync( request, parametros ).Result;
            }

            T retorno = JsonConvert.DeserializeObject<T>( response.Content.ReadAsStringAsync( ).Result );

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
