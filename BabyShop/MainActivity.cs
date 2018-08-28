using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using BabyShop.Servicos.Contratos;
using BabyShop.Servicos;
using BabyShop.ViewModel;

namespace BabyShop
{
    [Activity( Label = "BabyShop", MainLauncher = true )]
    public partial class MainActivity : Activity
    {
        #region [ LayoutControls ]
        private EditText Email { get; set; }
        private EditText Senha { get; set; }
        private TextView Mensagem { get; set; }
        #endregion

        protected override void OnCreate( Bundle savedInstanceState )
        {
            base.OnCreate( savedInstanceState );

            SetContentView( Resource.Layout.Logar );

            #region [ Set Layout Controls ]

            Email = FindViewById<EditText>( Resource.IdLogar.txtEmail );
            Senha = FindViewById<EditText>( Resource.IdLogar.txtSenha );
            Mensagem = FindViewById<TextView>( Resource.IdLogar.lblMensagem );

            #region [ Buttons events ]
            Button Enviar = FindViewById<Button>( Resource.IdLogar.btnLogar );
            Enviar.Click += new System.EventHandler( LoginAsync );

            Button Cadastrar = FindViewById<Button>( Resource.IdLogar.btnNovo );
            Cadastrar.Click += new System.EventHandler( delegate
            {
                var acCadastrar = new Intent( this, typeof( CadastrarActivity ) );
                StartActivity( acCadastrar );
            } );

            #endregion

            #endregion

        }
    }

    /// <summary>
    /// Propriedades e Metodos
    /// </summary>
    public partial class MainActivity
    {
        // Instância da classe que faz a comunicação com a API
        IClienteWS contaWS = ClienteService.GetInstance( );

        #region [ Metodos ]

        /// <summary>
        /// Método para efetuar login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void LoginAsync( object sender, EventArgs e )
        {
            Mensagem.Text = "";

            // Verifica se os dados de acesso foram enviados
            if ( string.IsNullOrEmpty( Email.Text ) || string.IsNullOrEmpty( Senha.Text ) )
            {
                // Obtém a mensagem de erro do arquivo de resorces
                Mensagem.Text = Resources.GetString( Resource.String.MSG00001 );
                return;
            }

            // Cria o objeto para envio a API
            LogarViewModel model = new LogarViewModel( );
            model.Email = Email.Text;
            model.Senha = Senha.Text;
            try
            {
                // Faz a chamada na camada de serviços para obter uma conta na API
                ClienteViewModel conta = await ObterContaAsync( model );

                // Mensagem de erro caso a conta não seja encontrada
                if ( conta == null || conta.DataExclusao.HasValue )
                {
                    // Obtém a mensagem de erro do arquivo de resorces
                    Mensagem.Text = Resources.GetString( Resource.String.MSG00002 );
                }
                else
                {
                    var contaSerializada = JsonConvert.SerializeObject( conta );

                    var acConta = new Intent( this, typeof( AtualizarActivity ) );
                    acConta.PutExtra( "conta", contaSerializada );
                    Senha.Text = "";
                    StartActivity( acConta );
                }
            }
            catch ( Exception )
            {
                Toast.MakeText( this, "Ocorreu um erro.", ToastLength.Long ).Show( );
            }


        }

        /// <summary>
        /// Método para obter uma conta na API
        /// </summary>
        /// <param name="model">Model contendo os dados de acesso</param>
        /// <returns>Retorna os dados do cliente</returns>
        protected async Task<ClienteViewModel> ObterContaAsync( LogarViewModel model )
        {
            // Faz a chamada na camada de serviços para acesso na API
            ClienteViewModel conta = await contaWS.ObterClienteAsync( model );
            return conta;
        }


        #endregion
    }

}

