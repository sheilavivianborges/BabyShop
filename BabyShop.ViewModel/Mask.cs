using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace BabyShop.ViewModel
{
    /// <summary>
    /// Classe para utilitários de máscara de campos na UI
    /// </summary>
    public class Mask : Java.Lang.Object, ITextWatcher
    {
        private readonly EditText _editText;
        private readonly string _mask;
        bool isUpdating;
        string old = "";

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="editText">String a ser aplicada a máscara</param>
        /// <param name="mask">Máscara a ser aplicada na string</param>
        public Mask( EditText editText, string mask )
        {
            _editText = editText;
            _mask = mask;
        }

        /// <summary>
        /// Remove a máscara de uma string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Unmask( string s )
        {
            return s.Replace( ".", "" ).Replace( "-", "" )
                .Replace( "/", "" ).Replace( "(", "" )
                .Replace( ")", "" );
        }

        public void AfterTextChanged( IEditable s )
        {
        }

        public void BeforeTextChanged( ICharSequence s, int start, int count, int after )
        {
        }

        /// <summary>
        /// Aplicar a máscara
        /// </summary>
        /// <param name="s"></param>
        /// <param name="start"></param>
        /// <param name="before"></param>
        /// <param name="count"></param>
        public void OnTextChanged( ICharSequence s, int start, int before, int count )
        {
            string str = Unmask( s.ToString( ) );
            string mascara = "";

            if ( isUpdating )
            {
                old = str;
                isUpdating = false;
                return;
            }

            int i = 0;

            foreach ( var m in _mask.ToCharArray( ) )
            {
                if ( m != '#' && str.Length > old.Length )
                {
                    mascara += m;
                    continue;
                }
                try
                {
                    mascara += str[ i ];
                }
                catch ( System.Exception )
                {
                    break;
                }
                i++;
            }

            isUpdating = true;

            _editText.Text = mascara;

            _editText.SetSelection( mascara.Length );
        }
    }
}