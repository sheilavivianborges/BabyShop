using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BabyShop.DTO;
using BabyShop.Servicos.Contratos;
using BabyShop.ViewModel;

namespace BabyShop.Servicos
{
    /// <summary>
    /// Classe para acesso a métodos de Clientes na API
    /// </summary>
    public class ClienteService : BaseService, IClienteWS
    {
        private static IClienteWS instance = null;

        private static readonly string _uri = "http://babyshop.tripletecnologia.com.br/servicos/";
        private static readonly string _serviceKey = "";

        /// <summary>
        /// Contrutor
        /// </summary>
        public ClienteService( ) : base( _uri, _serviceKey )
        {

        }

        /// <summary>
        /// Obter instância de ClienteService
        /// </summary>
        /// <returns>Instância de ClienteService</returns>
        public static IClienteWS GetInstance( )
        {
            if ( instance == null )
                instance = new ClienteService( );
            return instance;
        }

        /// <summary>
        /// Método para obter os dados de um clinte na API
        /// </summary>
        /// <param name="model">E-mail e senha do cliente</param>
        /// <returns>Retorna objeto com os dados do cliente</returns>
        public async Task<ClienteViewModel> ObterClienteAsync( LogarViewModel model )
        {
            string request = "cliente";

            LogarDTO Logar = new LogarDTO( )
            {
                Email = model.Email,
                Senha = model.Senha
            };

            // Faz a chamada na API
            ClienteDTO Cliente = await Post<ClienteDTO>( request, Logar );

            // Converte DTO recebido da API em ViewModel
            ClienteViewModel ClienteVM = ParseToVM( Cliente );

            return ClienteVM;
        }

        /// <summary>
        /// Método para obter um cliente na API com base no CPF
        /// </summary>
        /// <param name="CPF">CPF do cliente</param>
        /// <returns>Retorna objeto contendo os dados do cliente</returns>
        public async Task<ClienteViewModel> ObterClienteAsync( string CPF )
        {
            string request = "cliente";

            // Faz a chamada na API
            ClienteDTO Cliente = await Post<ClienteDTO>( request, CPF );

            // Converte DTO recebido da API em ViewModel
            ClienteViewModel ClienteVM = ParseToVM( Cliente );

            return ClienteVM;
        }

        /// <summary>
        /// Método para atualizar os dados de um cliente na API
        /// </summary>
        /// <param name="model">Objeto contendo os dados do cliente</param>
        /// <returns>Retorna os dados atualizados do cliente</returns>
        public async Task<ClienteViewModel> AtualizarClienteAsync( ClienteViewModel model )
        {
            string request = "cliente/atualizar";

            // Converte ViewModel em DTO
            ClienteDTO Cliente = ParseToDTO( model );

            // Faz a chamada na API
            Cliente = await Post<ClienteDTO>( request, Cliente );

            // Converte DTO recebido da API em ViewModel
            ClienteViewModel ClienteVM = ParseToVM( Cliente );

            return ClienteVM;
        }

        /// <summary>
        /// Método para inserir um novo cliente na API
        /// </summary>
        /// <param name="model">Objeto contendo os dados do novo cliente</param>
        /// <returns>Retorna os dados do cliente cadastrado</returns>
        public async Task<ClienteViewModel> SalvarClienteAsync( ClienteViewModel model )
        {
            string request = "cliente/novo";

            // Converte ViewModel em DTO
            ClienteDTO Cliente = ParseToDTO( model );

            // Faz a chamada na API
            Cliente = await Post<ClienteDTO>( request, Cliente );

            // Converte DTO recebido da API em ViewModel
            ClienteViewModel ClienteVM = ParseToVM( Cliente );

            return ClienteVM;
        }

        /// <summary>
        /// Método para excluir um cliente na API
        /// </summary>
        /// <param name="idCliente">Id do cliente a ser excluído</param>
        /// <returns>Retorna um booleano indicando se a exclusão foi feita com sucesso</returns>
        public async Task ExcluirClienteAsync( int idCliente )
        {
            string request = $"cliente/excluir";

            // Faz a chamada na API
            bool resp = await Post<bool>( request, idCliente );

        }

        #region [ Parses ]

        /// <summary>
        /// Converte ViewModel em DTO
        /// </summary>
        /// <param name="ClienteDTO">DTO obtido da API</param>
        /// <returns>Retorna ViewModel para uso na UI</returns>
        private ClienteViewModel ParseToVM( ClienteDTO ClienteDTO )
        {
            if ( ClienteDTO == null )
                return null;

            ClienteViewModel ClienteVM = new ClienteViewModel( );
            ClienteVM.Id = ClienteDTO.Id;
            ClienteVM.CPF = ClienteDTO.CPF;
            ClienteVM.Email = ClienteDTO.Email;
            ClienteVM.Endereco = ClienteDTO.Endereco;
            ClienteVM.Estado = ClienteDTO.Estado;
            ClienteVM.Municipio = ClienteDTO.Municipio;
            ClienteVM.NomeCompleto = ClienteDTO.Nome;
            ClienteVM.Senha = ClienteDTO.Senha;
            ClienteVM.Telefone = ClienteDTO.Telefone;
            ClienteVM.DataCadastro = ClienteDTO.DataCadastro;
            ClienteVM.DataExclusao = ClienteDTO.DataExclusao;

            return ClienteVM;
        }

        /// <summary>
        /// Converte DTO em ViewModel
        /// </summary>
        /// <param name="ClienteVM">ViewModel recebido da UI</param>
        /// <returns>Retorna DTO para ser enviado a API</returns>
        private ClienteDTO ParseToDTO( ClienteViewModel ClienteVM )
        {
            if ( ClienteVM == null )
                return null;

            ClienteDTO ClienteDTO = new ClienteDTO( );
            ClienteDTO.Id = ClienteVM.Id;
            ClienteDTO.CPF = ClienteVM.CPF;
            ClienteDTO.Email = ClienteVM.Email;
            ClienteDTO.Endereco = ClienteVM.Endereco;
            ClienteDTO.Estado = ClienteVM.Estado;
            ClienteDTO.Municipio = ClienteVM.Municipio;
            ClienteDTO.Nome = ClienteVM.NomeCompleto;
            ClienteDTO.Senha = ClienteVM.Senha;
            ClienteDTO.Telefone = ClienteVM.Telefone;
            ClienteDTO.DataCadastro = ClienteVM.DataCadastro;
            ClienteDTO.DataExclusao = ClienteVM.DataExclusao;

            return ClienteDTO;
        }

        #endregion
    }
}