using System.Threading.Tasks;
using BabyShop.ViewModel;

namespace BabyShop.Servicos.Contratos
{
    /// <summary>
    /// Interface contendo os métodos disponibilizados pela API
    /// </summary>
    public interface IClienteWS
    {
        Task<ClienteViewModel> ObterClienteAsync( LogarViewModel model );
        Task<ClienteViewModel> ObterClienteAsync( string cpf );
        Task<ClienteViewModel> AtualizarClienteAsync( ClienteViewModel model );
        Task ExcluirClienteAsync( int idConta );
        Task<ClienteViewModel> SalvarClienteAsync( ClienteViewModel model );
    }
}