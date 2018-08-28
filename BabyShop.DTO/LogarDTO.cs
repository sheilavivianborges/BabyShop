using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BabyShop.DTO
{
    /// <summary>
    /// DTO para comunicação de dados de acesso de cliente entre a UI e a API
    /// </summary>
    public class LogarDTO
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}