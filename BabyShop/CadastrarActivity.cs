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
using BabyShop.Servicos;
using BabyShop.Servicos.Contratos;
using BabyShop.ViewModel;

namespace BabyShop
{
    [Activity( Label = "CadastrarActivity" )]
    public partial class CadastrarActivity : Activity
    {
        #region [ Layout Controls ]
        private EditText IdConta { get; set; }
        private EditText NomeCompleto { get; set; }
        private EditText CPF { get; set; }
        private EditText Email { get; set; }
        private EditText Senha { get; set; }
        private EditText Endereco { get; set; }
        private EditText Estado { get; set; }
        private EditText Municipio { get; set; }
        private EditText Telefone { get; set; }
        private EditText DataCadastro { get; set; }
        private TextView Mensagem { get; set; }
        #endregion

        protected override void OnCreate( Bundle savedInstanceState )
        {
            base.OnCreate( savedInstanceState );

            SetContentView( Resource.Layout.Cadastrar );

            #region [ Set Layout Controls ]
            NomeCompleto = FindViewById<EditText>( Resource.IdCadastrar.txtNomeCompleto );
            CPF = FindViewById<EditText>( Resource.IdCadastrar.txtCPF );
            CPF.AddTextChangedListener( new Mask( CPF, "###.###.###-##" ) );
            Email = FindViewById<EditText>( Resource.IdCadastrar.txtEmail );
            Senha = FindViewById<EditText>( Resource.IdCadastrar.txtSenha );
            Endereco = FindViewById<EditText>( Resource.IdCadastrar.txtEndereco );
            Estado = FindViewById<EditText>( Resource.IdCadastrar.txtEstado );
            Municipio = FindViewById<EditText>( Resource.IdCadastrar.txtMunicipio );
            Telefone = FindViewById<EditText>( Resource.IdCadastrar.txtTelefone );
            Mensagem = FindViewById<TextView>( Resource.IdCadastrar.lblMensagem );

            #region [ Buttons events ]
            Button salvar = FindViewById<Button>( Resource.IdCadastrar.btnSalvar );
            salvar.Click += new EventHandler( SalvarAsync );

            #endregion

            #endregion
        }
    }

    public partial class CadastrarActivity : Activity
    {
        // Instância da classe que faz a comunicação com a API
        IClienteWS contaWS = ClienteService.GetInstance( );

        #region [ Metodos ]

        /// <summary>
        /// Método para salvar os dados de um client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void SalvarAsync( object sender, EventArgs e )
        {
            Mensagem.Text = "";

            // Valida o formulário
            if ( !ValidarForm( ) )
            {
                // Obtém a mensagem de erro do arquivo de resorces
                Mensagem.Text = Resources.GetString( Resource.String.MSG00004 );
                return;
            }

            //Obtém os dados de um cliente com base no CPF
            ClienteViewModel conta = await ObterContaAsync( CPF.Text );

            // Mensagem de erro caso a conta não seja encontrada
            if ( conta != null && conta.Id > 0 )
            {
                // Obtém a mensagem de erro do arquivo de resorces
                Mensagem.Text = Resources.GetString( Resource.String.MSG00003 );
                return;
            }

            // Em caso de sucesso, monta um objeto com os dados do cliente para envio a API
            ClienteViewModel model = new ClienteViewModel( );
            model.NomeCompleto = NomeCompleto.Text;
            model.CPF = Mask.Unmask( CPF.Text );
            model.Email = Email.Text;
            model.Senha = Senha.Text;
            model.Endereco = Endereco.Text;
            model.Estado = Estado.Text;
            model.Municipio = Municipio.Text;
            model.Telefone = Telefone.Text;

            // Envia os dados do cliente para a camada de serviços fazer a chamda na API
            conta = await contaWS.SalvarClienteAsync( model );

            Toast.MakeText( this, Resources.GetString( Resource.String.MSG00005 ), ToastLength.Long ).Show( );
            this.Finish( );

        }

        /// <summary>
        /// Método para obter a conta de um cliente
        /// </summary>
        /// <param name="CPF">CPF do cliente</param>
        /// <returns>Retorna objeto contendo os dados de um cliente</returns>
        protected async Task<ClienteViewModel> ObterContaAsync( string CPF )
        {
            // Envia os dados do cliente para a camada de serviços fazer a chamda na API
            ClienteViewModel conta = await contaWS.ObterClienteAsync( CPF );
            return conta;
        }

        /// <summary>
        /// Método que efetua a validação de um formulário de Cliente
        /// </summary>
        /// <returns>True/False indicando se o formulário está válido</returns>
        protected bool ValidarForm( )
        {

            if ( string.IsNullOrEmpty( NomeCompleto.Text )
                || string.IsNullOrEmpty( CPF.Text )
                || string.IsNullOrEmpty( Email.Text )
                || string.IsNullOrEmpty( Senha.Text )
                || string.IsNullOrEmpty( Endereco.Text )
                || string.IsNullOrEmpty( Estado.Text )
                || string.IsNullOrEmpty( Municipio.Text )
                || string.IsNullOrEmpty( Telefone.Text )
                )
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}