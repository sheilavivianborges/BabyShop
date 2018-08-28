using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using BabyShop.Servicos;
using BabyShop.Servicos.Contratos;
using BabyShop.ViewModel;
using Newtonsoft.Json;
using System;

namespace BabyShop
{
    [Activity( Label = "AtualizarActivity" )]
    public partial class AtualizarActivity : Activity
    {

        #region [ Layout Controls ]
        private EditText IdCliente { get; set; }
        private EditText NomeCompleto { get; set; }
        private EditText CPF { get; set; }
        private EditText Endereco { get; set; }
        private EditText Estado { get; set; }
        private EditText Municipio { get; set; }
        private EditText Telefone { get; set; }
        private EditText DataCadastro { get; set; }
        private TextView Mensagem { get; set; }
        private EditText Email { get; set; }
        private EditText Senha { get; set; }
        #endregion

        protected override void OnCreate( Bundle savedInstanceState )
        {
            base.OnCreate( savedInstanceState );

            SetContentView( Resource.Layout.Atualizar);

            #region [ Set Layout Controls ]
            IdCliente = FindViewById<EditText>( Resource.IdAtualizar.hfIdCliente );
            NomeCompleto = FindViewById<EditText>( Resource.IdAtualizar.txtNomeCompleto );
            CPF = FindViewById<EditText>( Resource.IdAtualizar.txtCPF );
            CPF.AddTextChangedListener( new Mask( CPF, "###.###.###-##" ) );
            Email = FindViewById<EditText>( Resource.IdAtualizar.txtEmail );
            Senha = FindViewById<EditText>(Resource.IdAtualizar.txtSenha );
            Endereco = FindViewById<EditText>( Resource.IdAtualizar.txtEndereco );
            Estado = FindViewById<EditText>( Resource.IdAtualizar.txtEstado );
            Municipio = FindViewById<EditText>( Resource.IdAtualizar.txtMunicipio );
            Telefone = FindViewById<EditText>( Resource.IdAtualizar.txtTelefone );
            DataCadastro = FindViewById<EditText>( Resource.IdAtualizar.hfDataCadastro );
            Mensagem = FindViewById<TextView>( Resource.IdAtualizar.lblMensagem );
            #region [ Buttons events ]
            Button salvar = FindViewById<Button>( Resource.IdAtualizar.btnSalvar );
            salvar.Click += new EventHandler( SalvarAsync );

            Button excluir = FindViewById<Button>( Resource.IdAtualizar.btnExcluir );
            excluir.Click += new EventHandler( Excluir );
            #endregion

            #endregion

            // Chamada ao método para popular os campos na tela
            PopularLayout( );
        }
    }

    public partial class AtualizarActivity
    {
        // Instância da classe que faz a comunicação com a API
        IClienteWS contaWS = ClienteService.GetInstance( );

        #region [ Metodos ]

        /// <summary>
        /// Popula os campos na tela
        /// </summary>
        private void PopularLayout( )
        {
            string contraSerializada = Intent.GetStringExtra( "conta" );

            if ( contraSerializada == null && String.IsNullOrEmpty( contraSerializada ) )
                this.Finish( );

            // Converte para ClienteViewModel
            ClienteViewModel cliente = JsonConvert.DeserializeObject<ClienteViewModel>( contraSerializada );

            // Popula os campos do cliente
            IdCliente.Text = cliente.Id.ToString( );
            NomeCompleto.Text = cliente.NomeCompleto;
            CPF.Text = cliente.CPF;
            Email.Text = cliente.Email;
            Senha.Text = cliente.Senha;
            Endereco.Text = cliente.Endereco;
            Estado.Text = cliente.Estado;
            Municipio.Text = cliente.Municipio;
            Telefone.Text = cliente.Telefone;
            DataCadastro.Text = cliente.DataCadastro.ToString( );
            
        }

        /// <summary>
        /// Método para salvar os dados de um cliente
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

            // Em caso de sucesso, monta um objeto com os dados do cliente para envio a API
            ClienteViewModel model = new ClienteViewModel( );
            model.Id = Convert.ToInt32( IdCliente.Text );
            model.NomeCompleto = NomeCompleto.Text;
            model.CPF = Mask.Unmask( CPF.Text );
            model.Email = Email.Text;
            model.Senha = Senha.Text;
            model.Endereco = Endereco.Text;
            model.Estado = Estado.Text;
            model.Municipio = Municipio.Text;
            model.Telefone = Telefone.Text;
            model.DataCadastro = DateTime.Parse( DataCadastro.Text );

            // Envia os dados do cliente para a camada de serviços fazer a chamda na API
            ClienteViewModel conta = await contaWS.AtualizarClienteAsync( model );

            Toast.MakeText( this, Resources.GetString( Resource.String.MSG00005 ), ToastLength.Long ).Show( );

        }

        /// <summary>
        /// Método para excluir um cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Excluir( object sender, EventArgs e )
        {
            AlertDialog.Builder alert = new AlertDialog.Builder( this );
            alert.SetTitle( "Salvar" );

            // Obtém a mensagem do arquivo se resources
            alert.SetMessage( Resources.GetString( Resource.String.MSG00006 ) );

            // Exibe confirmação ao usuário, em caso positivo chama a API para exclusão do cliente
            alert.SetPositiveButton( "Sim", delegate
            {
                // Envia os dados do cliente para a camada de serviços fazer a chamda na API
                contaWS.ExcluirClienteAsync( Convert.ToInt32( IdCliente.Text ) );

                // Obtém e exibe mensagem do arquivo de resources
                Toast.MakeText( this, Resources.GetString( Resource.String.MSG00007 ), ToastLength.Long ).Show( );
                alert.Dispose( );
                this.Finish( );
            } );
            alert.SetNegativeButton( "Não", delegate
            {
                alert.Dispose( );
            } );
            alert.Show( );


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