using Dominio.Entities.BaseEntity;
using Dominio.Helpers;
using System;

namespace Dominio.Entities
{
    public class User : EntityBase
    {

        public string Name { get; set; }
        public string Password { get; private set; }
        public DateTime? AcessDate { get; set; } = null;

        /// <summary>
        /// Construtor para o EF.
        /// </summary>
        public User()
        {

        }


        /// <summary>
        /// Construtor para validação de nome de Usuario e senha.
        /// </summary>
        /// <param name="name"> Nome do Usuário. </param>
        /// <param name="password"> Senha do Usuário. </param>
        public User(string name, string password)
        {
            //Verifica se ambos parametros estao nulos.
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(password))
                throw new Exception("Nome de Usuario e Senha devem ser informados.");

            SetName(name);
            SetPassword(password);
        }


        /// <summary>
        /// Valida e atribui valor a property Name do Usuário.
        /// </summary>
        /// <param name="name"> Nome do Usuário. </param>
        public void SetName(string name)
        {
            Guard.NullOrEmptyValuesCheck(out name, "Nome", name, requerido: true, mensagem: "O Nome de Usuário é obrigatório");
            Name = name;
        }

        /// <summary>
        /// Valida e atribui valor a property Password do Usuário.
        /// </summary>
        /// <param name="name"> Senha do Usuário. </param>
        public void SetPassword(string pass)
        {
            Guard.VerificaEspacoString(pass, "A Senha não deve conter espaços.");
            Guard.NullOrEmptyValuesCheck(out pass, "Senha" ,pass, mensagem: "O campo senha deve ser informado.", requerido: true);
            Password = pass;
        }

        //private void ChangePassword(string senha, string senhaConfirmacao, string senhaAtual)
        //{
        //    Guard.VerificaEspacoString(pass, "A Senha não deve conter espaços.");
        //    Guard.NullOrEmptyValuesCheck(out pass, "Senha" ,pass, mensagem: "O campo senha deve ser informado.", requerido: true);
        //    Guard.AreEquals(senha, senhaConfirmacao, "A senha e confirmação de senha devem ser iguais.");
        //    Guard.StringLength("Senha", senha, NameOrPassMinValue, NameOrPassMaxValue);
        //    Guard.StringLength("Senha", senhaConfirmacao, NameOrPassMinValue, NameOrPassMaxValue);

        //    CriptografaSenha(senha);

        //    try
        //    {
        //        UnicodeEncoding ByteConverter = new UnicodeEncoding();
        //        byte[] encryptedDataOld;
        //        byte[] encryptedDataNew;
        //        //byte[] decryptedData;
        //        byte[] dataToEncryptOld = ByteConverter.GetBytes(senhaAtual);
        //        byte[] dataToEncryptNew = ByteConverter.GetBytes(senha);
        //        using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
        //        {
        //            #region MOCK Senha do BD

        //            encryptedDataOld = EncryptHelper.RSAEncrypt(dataToEncryptOld, RSA.ExportParameters(false), false);

        //            #endregion

        //            encryptedDataNew = EncryptHelper.RSAEncrypt(dataToEncryptNew, RSA.ExportParameters(false), false);

        //            //decryptedData = EncryptHelper.RSADecrypt(dataToEncryptNew, RSA.ExportParameters(false), false);

        //            if (encryptedDataOld == encryptedDataNew)
        //                throw new Exception("Senha atual deve ser diferente da nova senha.");

        //            Password = encryptedDataNew;

        //        }

        //    }
        //    catch (Exception)
        //    {
        //        throw new Exception("Problema ao cryptografar a senha.");
        //    }

        //}

        //private void CriptografaSenha(string senha)
        //{
        //    try
        //    {
        //        UnicodeEncoding ByteConverter = new UnicodeEncoding();
        //        byte[] encryptedData;
        //        //byte[] decryptedData;
        //        byte[] dataToEncrypt = ByteConverter.GetBytes(senha);
        //        using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
        //        {
        //            encryptedData = EncryptHelper.RSAEncrypt(dataToEncrypt, RSA.ExportParameters(false), false);
        //            Password = encryptedData;
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        throw new Exception("Problema ao cryptografar a senha.");
        //    }
        //}
    }
}
