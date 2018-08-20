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
        IClienteWS contaWS = ClienteService.GetInstance( );

        #region [ Metodos ]

        protected async void LoginAsync( object sender, EventArgs e )
        {
            Mensagem.Text = "";

            if ( string.IsNullOrEmpty( Email.Text ) || string.IsNullOrEmpty( Senha.Text ) )
            {
                Mensagem.Text = Resources.GetString( Resource.String.MSG00001 );
                return;
            }


            LogarViewModel model = new LogarViewModel( );
            model.Email = Email.Text;
            model.Senha = Senha.Text;
            try
            {
                ClienteViewModel conta = await ObterContaAsync( model );
                if ( conta == null || conta.DataExclusao.HasValue )
                {
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

        protected async Task<ClienteViewModel> ObterContaAsync( LogarViewModel model )
        {
            ClienteViewModel conta = await contaWS.ObterClienteAsync( model );
            return conta;
        }


        #endregion
    }

}

