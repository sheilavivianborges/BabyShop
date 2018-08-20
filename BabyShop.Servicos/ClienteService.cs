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
    public class ClienteService : BaseService, IClienteWS
    {
        private static IClienteWS instance = null;


        private static readonly string _uri = "http://babyshop.tripletecnologia.com.br/servicos/";
        private static readonly string _serviceKey = "";

        public ClienteService( ) : base( _uri, _serviceKey )
        {

        }

        public static IClienteWS GetInstance( )
        {
            if ( instance == null )
                instance = new ClienteService( );
            return instance;
        }

        public async Task<ClienteViewModel> ObterClienteAsync( LogarViewModel model )
        {
            string request = "cliente";

            LogarDTO Logar = new LogarDTO( )
            {
                Email = model.Email,
                Senha = model.Senha
            };

            ClienteDTO Cliente = await Post<ClienteDTO>( request, Logar );
            ClienteViewModel ClienteVM = ParseToVM( Cliente );
            return ClienteVM;
        }

        public async Task<ClienteViewModel> ObterClienteAsync( string CPF )
        {
            string request = "cliente";

            ClienteDTO Cliente = await Post<ClienteDTO>( request, CPF );
            ClienteViewModel ClienteVM = ParseToVM( Cliente );
            return ClienteVM;
        }

        public async Task<ClienteViewModel> AtualizarClienteAsync( ClienteViewModel model )
        {
            string request = "cliente/atualizar";

            ClienteDTO Cliente = ParseToDTO( model );

            Cliente = await Post<ClienteDTO>( request, Cliente );

            ClienteViewModel ClienteVM = ParseToVM( Cliente );

            return ClienteVM;
        }


        public async Task<ClienteViewModel> SalvarClienteAsync( ClienteViewModel model )
        {
            string request = "cliente/novo";

            ClienteDTO Cliente = ParseToDTO( model );

            Cliente = await Post<ClienteDTO>( request, Cliente );

            ClienteViewModel ClienteVM = ParseToVM( Cliente );

            return ClienteVM;
        }

        public async Task ExcluirClienteAsync( int idCliente )
        {
            string request = $"cliente/excluir";

            bool resp = await Post<bool>( request, idCliente );

        }

        #region [ Parses ]

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